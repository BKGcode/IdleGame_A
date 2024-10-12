using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrolController : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyTypeSO enemyType;

    [Header("Destrucción")]
    public GameObject destructionParticlesPrefab;
    public AudioClip destructionSound;
    public double currencyReward;

    [Header("Salud")]
    public float maxHealth;
    private float currentHealth;

    [Header("Referencias de UI")]
    public TextMeshProUGUI floatingTextPrefab; // Cambiado a TextMeshProUGUI

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

        // Inicializar valores desde enemyType
        if (enemyType != null)
        {
            maxHealth = enemyType.maxHealth;
            currencyReward = enemyType.currencyReward;
            destructionParticlesPrefab = enemyType.destructionParticlesPrefab;
            destructionSound = enemyType.destructionSound;
        }
        else
        {
            Debug.LogError("EnemyTypeSO no está asignado en EnemyPatrolController.");
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
            MoveTowards(targetPoint, enemyType.patrolSpeed);
            Vector3 movementDirection = (targetPoint - transform.position).normalized;
            OrientTowards(movementDirection);
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            MoveTowards(player.position, enemyType.chaseSpeed);

            Vector3 direction = (player.position - transform.position).normalized;
            OrientTowards(direction);
        }
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float distanciaAlJugador = Vector3.Distance(transform.position, player.position);
        if (distanciaAlJugador <= enemyType.detectionRadius)
        {
            int randomChance = Random.Range(0, 11);
            if (randomChance <= enemyType.chaseProbability)
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
        Vector3 randomDirection = Random.insideUnitSphere * enemyType.patrolRadius;
        randomDirection += patrolCenter;
        randomDirection.y = transform.position.y;

        targetPoint = randomDirection;
    }

    private float GetRandomPatrolWaitTime()
    {
        return Random.Range(enemyType.minPatrolWaitTime, enemyType.maxPatrolWaitTime);
    }

    private void MoveTowards(Vector3 destination, float speed)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Vector3 velocity = direction * speed;

        Vector3 newPosition = transform.position + velocity * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private void OrientTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, enemyType.rotationSpeed * Time.fixedDeltaTime));
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

        // Añadir la recompensa de moneda y mostrar el texto flotante
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCurrency(currencyReward);
            ShowFloatingText(currencyReward);
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance está vacío. No se puede añadir moneda ni mostrar texto flotante.");
        }

        // Actualizar el contador de enemigos destruidos
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IncrementEnemyDestroyedCount(enemyType);
        }
        else
        {
            Debug.LogWarning("GameManager.Instance está vacío. No se puede incrementar el contador de enemigos destruidos.");
        }

        Destroy(gameObject);
    }

    private void ShowFloatingText(double amount)
    {
        if (floatingTextPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up;
            TextMeshProUGUI floatingText = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);
            floatingText.text = "+" + amount.ToString();
            // Puedes agregar animaciones o efectos adicionales aquí
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyType != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, enemyType.patrolRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyType.detectionRadius);
        }
    }
}
