using UnityEngine;

/// <summary>
/// Controla el movimiento del jugador utilizando físicas, incluyendo rotación hacia el ratón y un sistema de boost de velocidad.
/// Implementa el patrón Singleton para facilitar el acceso desde otros scripts.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; } // Instancia Singleton

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f; // Velocidad base de movimiento
    [SerializeField] private float rotationSpeed = 10f; // Velocidad de rotación

    [Header("Boost")]
    [SerializeField] private float boostMultiplier = 2f; // Multiplicador de velocidad durante el boost
    [SerializeField] private float boostDuration = 3f; // Duración del boost en segundos
    [SerializeField] private float boostCooldown = 5f; // Tiempo de cooldown del boost en segundos
    [SerializeField] private KeyCode boostKey = KeyCode.LeftShift; // Tecla para activar el boost

    [Header("Componentes")]
    [SerializeField] private Camera mainCamera; // Referencia a la cámara principal

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask groundLayerMask; // LayerMask para detectar el suelo

    [Header("Feedback")]
    [SerializeField] private AudioClip collisionSound; // Sonido al colisionar con un enemigo
    [SerializeField] private ParticleSystem collisionParticles; // Partículas al colisionar con un enemigo

    // Variables para el sistema de boost
    private bool isBoosting = false; // Indica si el boost está activo
    private float boostTimer = 0f; // Temporizador del boost
    private float cooldownTimer = 0f; // Temporizador del cooldown

    private Rigidbody rb; // Referencia al Rigidbody del Player
    private AudioSource audioSource; // Referencia al AudioSource del Player

    private void Awake()
    {
        // Implementación del Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: Mantener el Player entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Obtener la referencia al Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un componente Rigidbody en el jugador.");
        }

        // Obtener o añadir el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Asignar la cámara principal si no está asignada manualmente
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("No se ha asignado una cámara principal al PlayerController.");
            }
        }

        // Validar referencias de Feedback
        if (collisionSound == null)
        {
            Debug.LogWarning("No se ha asignado un AudioClip para collisionSound.");
        }

        if (collisionParticles == null)
        {
            Debug.LogWarning("No se ha asignado un ParticleSystem para collisionParticles.");
        }
    }

    private void Update()
    {
        HandleBoostInput();
    }

    private void FixedUpdate()
    {
        HandleMovementInput();
        HandleRotation();
        UpdateBoostTimers();
    }

    /// <summary>
    /// Maneja la entrada del usuario para el movimiento.
    /// </summary>
    private void HandleMovementInput()
    {
        // Obtener entradas de movimiento
        float verticalInput = Input.GetAxis("Vertical"); // W/S o Up/Down
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D o Left/Right

        // Determinar la velocidad actual (base o boost)
        float currentSpeed = isBoosting ? moveSpeed * boostMultiplier : moveSpeed;

        // Calcular el movimiento basado en la orientación del Player
        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput).normalized * currentSpeed;

        // Calcular la nueva posición
        Vector3 newPosition = rb.position + movement * Time.fixedDeltaTime;

        // Mover al jugador utilizando Rigidbody
        rb.MovePosition(newPosition);
    }

    /// <summary>
    /// Maneja la rotación del jugador hacia la posición del ratón.
    /// </summary>
    private void HandleRotation()
    {
        if (mainCamera == null) return;

        // Crear un rayo desde la posición del ratón en la pantalla hacia el mundo
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        // Realizar el raycast solo en la capa GroundLayer
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, groundLayerMask))
        {
            Vector3 hitPoint = hitInfo.point;
            Vector3 direction = (hitPoint - rb.position).normalized;
            direction.y = 0f; // Asegurarse de que la dirección sea horizontal

            if (direction != Vector3.zero)
            {
                // Calcular la rotación deseada
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Rotación suave hacia la dirección objetivo
                Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

                // Rotar al jugador utilizando Rigidbody
                rb.MoveRotation(newRotation);
            }
        }
    }

    /// <summary>
    /// Maneja la entrada del usuario para activar el boost.
    /// </summary>
    private void HandleBoostInput()
    {
        if (Input.GetKeyDown(boostKey))
        {
            if (!isBoosting && cooldownTimer <= 0f)
            {
                ActivateBoost();
            }
            else if (isBoosting)
            {
                boostTimer = boostDuration;
            }
        }
    }

    /// <summary>
    /// Activa el boost de velocidad.
    /// </summary>
    private void ActivateBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
    }

    /// <summary>
    /// Actualiza los temporizadores del boost y el cooldown.
    /// </summary>
    private void UpdateBoostTimers()
    {
        if (isBoosting)
        {
            boostTimer -= Time.fixedDeltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
                cooldownTimer = boostCooldown;
                boostTimer = 0f;
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.fixedDeltaTime;
            cooldownTimer = Mathf.Max(cooldownTimer, 0f);
        }
    }

    /// <summary>
    /// Indica si el boost está disponible.
    /// </summary>
    /// <returns>True si el boost está disponible, de lo contrario, false.</returns>
    public bool IsBoostAvailable()
    {
        return !isBoosting && cooldownTimer <= 0f;
    }
}
