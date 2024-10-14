// Assets/Scripts/Player/PlayerController.cs
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Referencias y Componentes

    [Header("Referencias de Datos")]
    [SerializeField] private PlayerData playerData;          // Referencia a PlayerData
    [SerializeField] private ShooterData shooterData;        // Referencia a ShooterData

    [Header("Componentes del Jugador")]
    [SerializeField] private CharacterController characterController; // Componente para el movimiento
    [SerializeField] private Transform cameraTransform;       // Referencia a la cámara del jugador

    [Header("Configuración de Movimiento")]
    [SerializeField] private float moveSpeed = 5f;           // Velocidad de movimiento
    [SerializeField] private float jumpHeight = 2f;          // Altura de salto
    [SerializeField] private float gravity = -9.81f;         // Gravedad aplicada al jugador

    [Header("Configuración de Disparo")]
    [SerializeField] private Transform firePoint;            // Punto desde donde se disparan las balas
    [SerializeField] private GameObject bulletPrefab;        // Prefab de la bala

    [Header("UI Referencias")]
    [SerializeField] private HealthUI healthUI;              // Referencia al script HealthUI
    [SerializeField] private WeaponUI weaponUI;              // Referencia al script WeaponUI
    [SerializeField] private ResourcesUI resourcesUI;        // Referencia al script ResourcesUI
    [SerializeField] private TimeUI timeUI;                  // Referencia al script TimeUI
    [SerializeField] private UpgradeUI upgradeUI;            // Referencia al script UpgradeUI

    // Variables Privadas
    private Vector3 velocity;                                 // Velocidad actual del jugador
    private bool isGrounded;                                  // Verifica si el jugador está en el suelo
    private float nextFireTime = 0f;                          // Tiempo para el próximo disparo

    #endregion

    #region Propiedades Públicas

    /// <summary>
    /// Exposición pública de PlayerData para acceso externo (por ejemplo, desde EnemyController)
    /// </summary>
    public PlayerData PlayerData => playerData;

    #endregion

    #region Métodos de Ciclo de Vida

    private void Start()
    {
        // Verificar asignaciones en el Inspector
        if (playerData == null)
        {
            Debug.LogError("PlayerData no está asignado en el Inspector.");
        }

        if (shooterData == null)
        {
            Debug.LogError("ShooterData no está asignado en el Inspector.");
        }

        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("CharacterController no está asignado y no se encontró en el GameObject.");
            }
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform no está asignado en el Inspector.");
        }

        if (firePoint == null)
        {
            Debug.LogError("Fire Point no está asignado en el Inspector.");
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab no está asignado en el Inspector.");
        }

        if (healthUI == null)
        {
            Debug.LogError("HealthUI no está asignado en el Inspector.");
        }

        if (weaponUI == null)
        {
            Debug.LogError("WeaponUI no está asignado en el Inspector.");
        }

        if (resourcesUI == null)
        {
            Debug.LogError("ResourcesUI no está asignado en el Inspector.");
        }

        if (timeUI == null)
        {
            Debug.LogError("TimeUI no está asignado en el Inspector.");
        }

        if (upgradeUI == null)
        {
            Debug.LogError("UpgradeUI no está asignado en el Inspector.");
        }

        // Inicializar UI con los datos actuales
        healthUI.UpdateHealthUI(playerData.Health);
        weaponUI.UpdateWeaponUI(playerData.CurrentWeapon);
        weaponUI.UpdateAmmoUI(playerData.Ammo);
        resourcesUI.UpdatePlayerResourcesUI(playerData.Resources);
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();

        // Manejar recarga
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        // Manejar cambio de arma (ejemplo con teclas numéricas)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Cambiar a la primera arma disponible
            if (shooterData.AvailableWeapons.Count > 0)
            {
                ChangeWeapon(shooterData.AvailableWeapons[0]);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Cambiar a la segunda arma disponible
            if (shooterData.AvailableWeapons.Count > 1)
            {
                ChangeWeapon(shooterData.AvailableWeapons[1]);
            }
        }
        // Añadir más cambios de arma según sea necesario
    }

    #endregion

    #region Manejo de Movimiento

    /// <summary>
    /// Maneja el movimiento del jugador, incluyendo el salto y la gravedad.
    /// </summary>
    private void HandleMovement()
    {
        // Verificar si el jugador está en el suelo
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (characterController.height / 2), 0.1f, LayerMask.GetMask("Ground"));

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reiniciar la velocidad vertical al estar en el suelo
        }

        // Obtener entradas de movimiento
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcular la dirección de movimiento relativa a la cámara
        Vector3 move = cameraTransform.right * moveX + cameraTransform.forward * moveZ;
        move.y = 0f; // Ignorar la componente vertical

        // Mover al jugador
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Manejar el salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    #endregion

    #region Manejo de Disparo

    /// <summary>
    /// Maneja el disparo de armas, incluyendo el manejo de la tasa de disparo y la munición.
    /// </summary>
    private void HandleShooting()
    {
        if (playerData.CurrentWeapon == null)
            return;

        // Obtener el arma actual
        WeaponData currentWeapon = playerData.CurrentWeapon;

        // Verificar si hay munición disponible
        if (playerData.Ammo <= 0)
            return;

        // Manejar el disparo continuo basado en la fireRate
        if (currentWeapon.fireRate > 0)
        {
            if (Input.GetButton("Fire1"))
            {
                if (Time.time >= nextFireTime)
                {
                    Shoot(currentWeapon);
                    nextFireTime = Time.time + 1f / currentWeapon.fireRate;
                }
            }
        }
        else
        {
            // Manejar el disparo con botón pulsado
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot(currentWeapon);
            }
        }

        // Actualizar la UI de la munición
        weaponUI.UpdateAmmoUI(playerData.Ammo);
    }

    /// <summary>
    /// Instancia una bala y maneja el decremento de la munición.
    /// </summary>
    /// <param name="weapon">El arma desde la cual se dispara.</param>
    private void Shoot(WeaponData weapon)
    {
        if (playerData.Ammo <= 0)
            return;

        // Instanciar la bala en el punto de disparo
        GameObject bullet = Instantiate(weapon.bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * weapon.bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet Prefab no tiene un componente Rigidbody.");
        }

        // Decrementar la munición
        playerData.UseAmmo(1);

        // Opcional: añadir efectos de sonido o visuales
        Debug.Log($"Disparado con {weapon.weaponName}. Munición restante: {playerData.Ammo}");
    }

    /// <summary>
    /// Maneja la recarga del arma.
    /// </summary>
    private void Reload()
    {
        if (playerData.CurrentWeapon == null)
            return;

        WeaponData currentWeapon = playerData.CurrentWeapon;

        // Verificar si hay cargadores disponibles (asumiendo que tienes una lógica para esto)
        // Por simplicidad, reestablecemos la munición al tamaño del cargador
        playerData.Reload(currentWeapon.magazineSize);

        // Actualizar la UI de la munición
        weaponUI.UpdateAmmoUI(playerData.Ammo);

        // Opcional: añadir animación de recarga
        Debug.Log($"Recargando {currentWeapon.weaponName}. Munición actual: {playerData.Ammo}");
    }

    #endregion

    #region Cambio de Arma

    /// <summary>
    /// Cambia el arma actual del jugador.
    /// </summary>
    /// <param name="newWeapon">El nuevo arma a equipar.</param>
    public void ChangeWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null)
            return;

        playerData.ChangeWeapon(newWeapon);

        // Actualizar la UI del arma
        weaponUI.UpdateWeaponUI(newWeapon);

        // Actualizar la UI de la munición
        weaponUI.UpdateAmmoUI(playerData.Ammo);

        Debug.Log($"Arma cambiada a {newWeapon.weaponName}");
    }

    #endregion

    #region Recolección de Recursos

    /// <summary>
    /// Método llamado por otros scripts para añadir recursos al jugador.
    /// </summary>
    /// <param name="amount">Cantidad de recursos a añadir.</param>
    public void CollectResource(int amount)
    {
        playerData.AddResources(amount);

        // Actualizar la UI de recursos
        resourcesUI.UpdatePlayerResourcesUI(playerData.Resources);

        Debug.Log($"Recurso recolectado: {amount}. Recursos totales: {playerData.Resources}");
    }

    #endregion

    #region Manejo de Salud

    /// <summary>
    /// Método para aplicar daño al jugador.
    /// </summary>
    /// <param name="damage">Cantidad de daño a aplicar.</param>
    public void TakeDamage(int damage)
    {
        playerData.TakeDamage(damage);

        // Actualizar la UI de salud
        healthUI.UpdateHealthUI(playerData.Health);

        Debug.Log($"Jugador recibió {damage} de daño. Salud restante: {playerData.Health}");

        // Verificar si el jugador ha muerto
        if (playerData.Health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Método para curar al jugador.
    /// </summary>
    /// <param name="amount">Cantidad de salud a añadir.</param>
    public void Heal(int amount)
    {
        playerData.Heal(amount);

        // Actualizar la UI de salud
        healthUI.UpdateHealthUI(playerData.Health);

        Debug.Log($"Jugador curado en {amount}. Salud actual: {playerData.Health}");
    }

    /// <summary>
    /// Maneja la lógica cuando el jugador muere.
    /// </summary>
    private void Die()
    {
        // Implementa la lógica de muerte, como reiniciar la escena o mostrar una pantalla de Game Over
        Debug.Log("Jugador ha muerto. Fin del juego.");

        // Ejemplo: reiniciar la escena después de 2 segundos
        StartCoroutine(RestartGame());
    }

    /// <summary>
    /// Reinicia el juego después de un breve retraso.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    #endregion
}
