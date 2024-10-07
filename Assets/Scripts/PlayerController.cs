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

    // Sonido y efectos visuales para daño
    [SerializeField] private AudioClip damageSound; 
    [SerializeField] private ParticleSystem damageFXPrefab; 
    private AudioSource audioSource;

    private void Start()
    {
        mainCamera = Camera.main;
        remainingBoostTime = boostDuration;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RotateTowardsMouse();
        HandleMovement();
        HandleBoostTimers();
        HandleBoostInput();
    }

    // Rotar hacia la posición del ratón en un plano 3D
    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Mantener rotación en el plano
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
            transform.Translate(Vector3.forward * currentSpeed * vertical * Time.deltaTime);
        }
    }

    // Manejo de temporizadores para el boost
    private void HandleBoostTimers()
    {
        if (isBoosting)
        {
            remainingBoostTime -= Time.deltaTime;
            if (remainingBoostTime <= 0)
            {
                isBoosting = false;
                cooldownTimer = boostCooldown;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    // Entrada para activar el boost
    private void HandleBoostInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isBoosting && cooldownTimer <= 0)
        {
            isBoosting = true;
            remainingBoostTime = boostDuration;
        }
    }

    // Método para aplicar daño al Player con efectos de sonido y visuales
    public void TakeDamage()
    {
        if (damageSound != null)
            audioSource.PlayOneShot(damageSound);

        if (damageFXPrefab != null)
            Instantiate(damageFXPrefab, transform.position, Quaternion.identity);
    }
}
