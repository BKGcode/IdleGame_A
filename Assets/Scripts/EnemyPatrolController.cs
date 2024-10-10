using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrolController : MonoBehaviour
{
    [Header("Patrulla")]
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float minPatrolWaitTime = 1f;
    [SerializeField] private float maxPatrolWaitTime = 3f;

    [Header("Persecución")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float chaseProbability = 5f;
    [SerializeField] private float chaseSpeed = 7f;

    [Header("Movimiento")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Destrucción")]
    [SerializeField] private GameObject destructionParticlesPrefab;
    [SerializeField] private AudioClip destructionSound;

    [Header("Salud")]
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;

    private Rigidbody rb;
    private Vector3 patrolCenter;
    private Vector3 targetPoint;
    private bool isPatrolling = true;
    private bool isChasing = false;
    private float patrolTimer = 0f;
    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        patrolCenter = transform.position;
        SetRandomTargetPoint();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún GameObject con la etiqueta 'Player'.");
        }

        currentHealth = maxHealth;
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
        else
        {
            MoveTowards(targetPoint, patrolSpeed);
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            MoveTowards(player.position, chaseSpeed);
        }
    }

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

    private void SetRandomTargetPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += patrolCenter;
        randomDirection.y = transform.position.y;

        targetPoint = randomDirection;
    }

    private float GetRandomPatrolWaitTime()
    {
        return Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
    }

    private void MoveTowards(Vector3 destination, float speed)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Vector3 velocity = direction * speed;

        Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Orientar al enemigo en la dirección del movimiento
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        if (destructionParticlesPrefab != null)
        {
            Instantiate(destructionParticlesPrefab, transform.position, Quaternion.identity);
        }

        if (destructionSound != null)
        {
            AudioSource.PlayClipAtPoint(destructionSound, transform.position);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}