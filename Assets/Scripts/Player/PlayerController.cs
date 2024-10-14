using UnityEngine;
using System.Collections;
using ShooterGame.Data;

namespace ShooterGame.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Referencias y Componentes

        [Header("Referencias de Datos")]
        [SerializeField] private PlayerData playerData;
        [SerializeField] private ShooterData shooterData;

        [Header("Componentes del Jugador")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform cameraTransform;

        [Header("Configuración de Movimiento")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        [Header("Configuración de Boost")]
        [SerializeField] private float boostMultiplier = 1.5f;
        [SerializeField] private float boostDuration = 3f;
        [SerializeField] private float boostCooldown = 5f;

        [Header("Configuración de Disparo")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;

        [Header("UI Referencias")]
        [SerializeField] private HealthUI healthUI;
        [SerializeField] private WeaponUI weaponUI;
        [SerializeField] private ResourcesUI resourcesUI;
        [SerializeField] private TimeUI timeUI;
        [SerializeField] private UpgradeUI upgradeUI;

        [Header("Efectos de Daño")]
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private ParticleSystem hitEffect;

        // Variables Privadas
        private Vector3 moveDirection;
        private Vector3 lookDirection;
        private float nextFireTime = 0f;
        private bool canBoost = true;
        private float currentBoostTime = 0f;
        private float boostCooldownTime = 0f;

        #endregion

        #region Propiedades Públicas

        public PlayerData PlayerData => playerData;

        #endregion

        #region Métodos de Ciclo de Vida

        private void Start()
        {
            InitializeComponents();
            SetupRigidbody();
            UpdateUI();
        }

        private void Update()
        {
            CalculateMouseDirection();
            HandleRotation();
            HandleShooting();
            HandleWeaponChange();
            HandleBoostInput();
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        #endregion

        #region Inicialización y Configuración

        private void InitializeComponents()
        {
            if (playerData == null)
                Debug.LogError("PlayerData no está asignado en el Inspector.");

            if (shooterData == null)
                Debug.LogError("ShooterData no está asignado en el Inspector.");

            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                if (rb == null)
                    Debug.LogError("Rigidbody no está asignado y no se encontró en el GameObject.");
            }

            if (cameraTransform == null)
                Debug.LogError("Camera Transform no está asignado en el Inspector.");

            if (firePoint == null)
                Debug.LogError("Fire Point no está asignado en el Inspector.");

            if (bulletPrefab == null)
                Debug.LogError("Bullet Prefab no está asignado en el Inspector.");

            if (healthUI == null)
                Debug.LogError("HealthUI no está asignado en el Inspector.");

            if (weaponUI == null)
                Debug.LogError("WeaponUI no está asignado en el Inspector.");

            if (resourcesUI == null)
                Debug.LogError("ResourcesUI no está asignado en el Inspector.");

            if (timeUI == null)
                Debug.LogError("TimeUI no está asignado en el Inspector.");

            if (upgradeUI == null)
                Debug.LogError("UpgradeUI no está asignado en el Inspector.");
        }

        private void SetupRigidbody()
        {
            if (rb != null)
            {
                rb.useGravity = false;
                rb.freezeRotation = true;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
        }

        private void UpdateUI()
        {
            healthUI.UpdateHealthUI(playerData.Health);
            weaponUI.UpdateWeaponUI(playerData.CurrentWeapon);
            weaponUI.UpdateAmmoUI(playerData.Ammo);
            resourcesUI.UpdatePlayerResourcesUI(playerData.Resources);
        }

        #endregion

        #region Manejo de Movimiento y Boost

        private void CalculateMouseDirection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                lookDirection = (point - transform.position).normalized;
                lookDirection.y = 0f;
            }
        }

        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = (transform.right * horizontal + transform.forward * vertical).normalized;
            float currentSpeed = moveSpeed;

            if (Input.GetKey(KeyCode.LeftShift) && canBoost)
            {
                currentSpeed *= boostMultiplier;
            }

            rb.linearVelocity = movement * currentSpeed;

            if (movement.magnitude == 0)
            {
                rb.linearVelocity = Vector3.zero;
            }
        }

        private void HandleRotation()
        {
            if (lookDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleBoostInput()
        {
            if (Input.GetKey(KeyCode.LeftShift) && canBoost)
            {
                currentBoostTime += Time.deltaTime;
                if (currentBoostTime >= boostDuration)
                {
                    canBoost = false;
                    boostCooldownTime = boostCooldown;
                }
            }
            else if (!canBoost)
            {
                boostCooldownTime -= Time.deltaTime;
                if (boostCooldownTime <= 0)
                {
                    canBoost = true;
                    currentBoostTime = 0f;
                }
            }
            else
            {
                currentBoostTime = Mathf.Max(0, currentBoostTime - Time.deltaTime);
            }
        }

        #endregion

        #region Manejo de Disparo

        private void HandleShooting()
        {
            if (playerData.CurrentWeapon == null || playerData.Ammo <= 0)
                return;

            WeaponData currentWeapon = playerData.CurrentWeapon;

            if (currentWeapon.fireRate > 0)
            {
                if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
                {
                    Shoot(currentWeapon);
                    nextFireTime = Time.time + 1f / currentWeapon.fireRate;
                }
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                Shoot(currentWeapon);
            }

            weaponUI.UpdateAmmoUI(playerData.Ammo);
        }

        private void Shoot(WeaponData weapon)
        {
            GameObject bullet = Instantiate(weapon.bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = firePoint.forward * weapon.bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet Prefab no tiene un componente Rigidbody.");
            }

            playerData.UseAmmo(1);
            Debug.Log($"Disparado con {weapon.weaponName}. Munición restante: {playerData.Ammo}");
        }

        private void HandleWeaponChange()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && shooterData.AvailableWeapons.Count > 0)
            {
                ChangeWeapon(shooterData.AvailableWeapons[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && shooterData.AvailableWeapons.Count > 1)
            {
                ChangeWeapon(shooterData.AvailableWeapons[1]);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        public void ChangeWeapon(WeaponData newWeapon)
        {
            if (newWeapon == null)
                return;

            playerData.ChangeWeapon(newWeapon);
            weaponUI.UpdateWeaponUI(newWeapon);
            weaponUI.UpdateAmmoUI(playerData.Ammo);
            Debug.Log($"Arma cambiada a {newWeapon.weaponName}");
        }

        private void Reload()
        {
            if (playerData.CurrentWeapon == null)
                return;

            WeaponData currentWeapon = playerData.CurrentWeapon;
            playerData.Reload(currentWeapon.magazineSize);
            weaponUI.UpdateAmmoUI(playerData.Ammo);
            Debug.Log($"Recargando {currentWeapon.weaponName}. Munición actual: {playerData.Ammo}");
        }

        #endregion

        #region Recolección de Recursos

        public void CollectResource(int amount)
        {
            playerData.AddResources(amount);
            resourcesUI.UpdatePlayerResourcesUI(playerData.Resources);
            Debug.Log($"Recurso recolectado: {amount}. Recursos totales: {playerData.Resources}");
        }

        #endregion

        #region Manejo de Salud

        public void TakeDamage(int damage)
        {
            playerData.TakeDamage(damage);
            healthUI.UpdateHealthUI(playerData.Health);
            Debug.Log($"Jugador recibió {damage} de daño. Salud restante: {playerData.Health}");

            PlayHitEffect();

            if (playerData.Health <= 0)
            {
                Die();
            }
        }

        private void PlayHitEffect()
        {
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            if (hitEffect != null)
            {
                hitEffect.Play();
            }
        }

        public void Heal(int amount)
        {
            playerData.Heal(amount);
            healthUI.UpdateHealthUI(playerData.Health);
            Debug.Log($"Jugador curado en {amount}. Salud actual: {playerData.Health}");
        }

        private void Die()
        {
            Debug.Log("Jugador ha muerto. Fin del juego.");
            StartCoroutine(RestartGame());
        }

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(2f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        #endregion

        #region Mejoras de Armas

        public void ApplyWeaponUpgrade(UpgradeData upgrade)
        {
            playerData.ApplyWeaponUpgrade(upgrade);
            weaponUI.UpdateWeaponUI(playerData.CurrentWeapon);
            weaponUI.UpdateAmmoUI(playerData.Ammo);
            Debug.Log($"Mejora aplicada: {upgrade.upgradeName}");
        }

        #endregion
    }
}