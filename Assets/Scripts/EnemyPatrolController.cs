using UnityEngine;

/// <summary>
/// Controla el comportamiento de patrulla y persecución de un enemigo usando Rigidbody.
/// Elimina al enemigo al colisionar con el jugador.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrolController : MonoBehaviour
{
    [Header("Patrulla")]
    [Tooltip("Radio de patrulla alrededor del centro.")]
    [SerializeField] private float patrolRadius = 10f;
    
    [Tooltip("Tiempo mínimo de espera en cada punto de patrulla.")]
    [SerializeField] private float minPatrolWaitTime = 1f;
    
    [Tooltip("Tiempo máximo de espera en cada punto de patrulla.")]
    [SerializeField] private float maxPatrolWaitTime = 3f;

    [Header("Persecución")]
    [Tooltip("Radio de detección del jugador.")]
    [SerializeField] private float detectionRadius = 5f;
    
    [Tooltip("Probabilidad (0-10) de iniciar persecución.")]
    [SerializeField] private float chaseProbability = 5f;
    
    [Tooltip("Velocidad durante la persecución.")]
    [SerializeField] private float chaseSpeed = 7f;

    [Header("Movimiento")]
    [Tooltip("Velocidad de patrulla.")]
    [SerializeField] private float patrolSpeed = 3f;

    private Rigidbody rb; // Referencia al Rigidbody del enemigo
    private Vector3 patrolCenter; // Centro de patrulla
    private Vector3 targetPoint; // Punto actual de destino
    private bool isPatrolling = true; // Estado de patrulla
    private bool isChasing = false; // Estado de persecución
    private float patrolTimer = 0f; // Temporizador para esperar en puntos de patrulla

    private Transform player; // Referencia al Transform del jugador

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        patrolCenter = transform.position;
        SetRandomTargetPoint();

        // Buscar al jugador por la etiqueta "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún GameObject con la etiqueta 'Player'. Asegúrate de que el jugador esté etiquetado correctamente.");
        }
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void FixedUpdate()
    {
        if (isPatrolling)
        {
            Patrol();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
    }

    /// <summary>
    /// Maneja el comportamiento de patrulla hacia un punto aleatorio.
    /// </summary>
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, targetPoint) < 0.5f)
        {
            patrolTimer += Time.fixedDeltaTime;
            if (patrolTimer >= GetRandomPatrolWaitTime())
            {
                SetRandomTargetPoint();
                patrolTimer = 0f;
            }
        }

        MoveTowards(targetPoint, patrolSpeed);
    }

    /// <summary>
    /// Maneja el comportamiento de persecución hacia el jugador.
    /// </summary>
    private void ChasePlayer()
    {
        if (player != null)
        {
            MoveTowards(player.position, chaseSpeed);
            OrientTowards(player.position);
        }
    }

    /// <summary>
    /// Detecta al jugador dentro del radio de detección y decide si iniciar la persecución.
    /// </summary>
    private void DetectPlayer()
    {
        if (player == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, player.position);
        if (distanciaAlJugador <= detectionRadius)
        {
            int randomChance = Random.Range(0, 11);
            if (randomChance <= chaseProbability)
            {
                isChasing = true;
                isPatrolling = false;
            }
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                isPatrolling = true;
                SetRandomTargetPoint();
            }
        }
    }

    /// <summary>
    /// Asigna un nuevo punto aleatorio dentro del radio de patrulla.
    /// </summary>
    private void SetRandomTargetPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += patrolCenter;
        randomDirection.y = transform.position.y; // Mantener la altura

        targetPoint = randomDirection;
    }

    /// <summary>
    /// Obtiene un tiempo de espera aleatorio entre el mínimo y máximo definido.
    /// </summary>
    /// <returns>Tiempo aleatorio de espera.</returns>
    private float GetRandomPatrolWaitTime()
    {
        return Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
    }

    /// <summary>
    /// Mueve al enemigo hacia una posición específica a una velocidad dada.
    /// </summary>
    /// <param name="destination">Destino al que se moverá.</param>
    /// <param name="speed">Velocidad de movimiento.</param>
    private void MoveTowards(Vector3 destination, float speed)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Vector3 velocity = direction * speed;

        // Actualizar posición usando MovePosition para kinematic Rigidbody
        Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    /// <summary>
    /// Orienta al enemigo hacia una posición específica instantáneamente.
    /// </summary>
    /// <param name="targetPosition">Posición hacia la cual orientarse.</param>
    private void OrientTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(targetRotation);
        }
    }

    /// <summary>
    /// Maneja la colisión con otros objetos.
    /// </summary>
    /// <param name="collision">Información de la colisión.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Dibuja Gizmos en el Editor para visualizar los radios de patrulla y detección.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Dibuja el radio de patrulla
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        // Dibuja el radio de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
