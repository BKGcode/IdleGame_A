using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables para la velocidad de movimiento y boost
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boostMultiplier = 2f;
    [SerializeField] private float boostDuration = 3f;
    [SerializeField] private float boostCooldown = 5f;

    private Camera mainCamera;
    private bool isBoosting = false;
    private float remainingBoostTime;
    private float cooldownTimer = 0f;

    // Referencias para el sistema de vida
    public LifeSystem lifeSystem;

    // Sonido y efectos visuales
    [SerializeField] private AudioClip damageSound; // Sonido cuando el Player recibe daño
    [SerializeField] private ParticleSystem damageFXPrefab; // Prefab del efecto visual de daño
    private AudioSource audioSource; // Reproductor de audio

    private void Start()
    {
        mainCamera = Camera.main;
        remainingBoostTime = boostDuration;

        // Obtener el AudioSource del Player
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RotateTowardsMouse();
        HandleMovement();
        HandleBoostTimers();
        HandleBoostInput();
    }

    // Rotación del Player hacia el puntero del ratón
    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; 
            transform.forward = direction;
        }
    }

    // Movimiento del Player
    private void HandleMovement()
    {
        float vertical = Input.GetAxis("Vertical");
        if (vertical > 0)
        {
            float currentSpeed = isBoosting ? moveSpeed * boostMultiplier : moveSpeed;
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }

    // Manejo del temporizador de boost y cooldown
    private void HandleBoostTimers()
    {
        if (isBoosting)
        {
            remainingBoostTime -= Time.deltaTime; 
            if (remainingBoostTime <= 0)
            {
                isBoosting = false;
                remainingBoostTime = 0;
                cooldownTimer = boostCooldown;
            }
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // Manejo de la entrada para activar o desactivar el boost
    private void HandleBoostInput()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && remainingBoostTime > 0 && cooldownTimer <= 0)
        {
            isBoosting = true;
        }
        else
        {
            isBoosting = false;
        }
    }

    // Función para manejar el impacto con el enemigo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // El Player recibe daño
            lifeSystem.ReduceLife(1); // Reduce una vida usando el sistema de vida

            // Reproducir sonido de daño
            if (damageSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(damageSound);
            }

            // Instanciar y reproducir FX de daño
            if (damageFXPrefab != null)
            {
                ParticleSystem damageFX = Instantiate(damageFXPrefab, transform.position, Quaternion.identity);
                damageFX.Play();
                Destroy(damageFX.gameObject, damageFX.main.duration); // Destruir el FX después de que termine
            }

            // Destruir al enemigo después del impacto
            Destroy(other.gameObject);
        }
    }
}
