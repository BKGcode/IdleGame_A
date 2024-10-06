using UnityEngine;
using System.Collections;

public class EnemyPatrolController : MonoBehaviour
{
    public EnemyData enemyData;  // Datos del enemigo

    public float patrolRadius = 10f;  // Radio en el que patrullará el enemigo
    public float detectionRadius = 5f;  // Radio de detección del jugador
    public float chaseSpeed = 3f;  // Velocidad de persecución del jugador
    public float patrolSpeed = 2f;  // Velocidad de patrulla
    public float stopDistance = 1.5f;  // Distancia mínima para detenerse cerca del jugador

    public bool destroyOnImpact = true;  // Si se destruye el enemigo al impactar
    public float damageCooldown = 2f;  // Tiempo de cooldown antes de hacer más daño al Player
    public float destructionDelay = 1.5f;  // Cooldown antes de destruir el enemigo

    private Vector3 startPosition;
    private Vector3 patrolPoint;
    private int currentLives;
    private bool isChasing = false;
    private bool canDamagePlayer = true;  // Controla si el enemigo puede hacer daño

    private Transform player;
    private EnemyFXController enemyFX;  // Referencia al controlador de efectos del enemigo

    private void Start()
    {
        startPosition = transform.position;
        currentLives = enemyData.maxLives;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Intentamos obtener el controlador de FX del enemigo
        enemyFX = GetComponent<EnemyFXController>();

        SetRandomPatrolPoint();
    }

    private void Update()
    {
        DetectPlayer();  // Detecta si el jugador está cerca

        if (isChasing)
        {
            ChasePlayer();  // Persigue al jugador si está en rango
        }
        else
        {
            Patrol();  // Patrulla si no está persiguiendo al jugador
        }
    }

    // Método para patrullar entre puntos aleatorios
    private void Patrol()
    {
        MoveTo(patrolPoint, patrolSpeed);

        if (Vector3.Distance(transform.position, patrolPoint) < 0.5f)
        {
            SetRandomPatrolPoint();
        }
    }

    private void SetRandomPatrolPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * patrolRadius;
        randomPoint += startPosition;
        patrolPoint = new Vector3(randomPoint.x, transform.position.y, randomPoint.z);  // Mantenemos la altura constante
    }

    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && distanceToPlayer > stopDistance)
        {
            isChasing = true;  // El enemigo empieza a perseguir al jugador
        }
        else
        {
            isChasing = false;
        }
    }

    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > stopDistance)
        {
            MoveTo(player.position, chaseSpeed);  // Perseguir al jugador
        }
    }

    // Método para mover al enemigo sin Rigidbody
    private void MoveTo(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Método para infligir daño al jugador
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamagePlayer)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.TakeDamage(enemyData.damage);  // Inflige daño al jugador

                // Emite sonido y partículas
                if (enemyFX != null)
                {
                    enemyFX.PlayImpactFX();
                }

                if (destroyOnImpact)
                {
                    StartCoroutine(DieWithDelay());
                }

                canDamagePlayer = false;
                StartCoroutine(DamageCooldown());
            }
        }
    }

    // Enfriamiento para evitar daño continuo
    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canDamagePlayer = true;
    }

    // Método para destruir al enemigo después de un retraso
    private IEnumerator DieWithDelay()
    {
        yield return new WaitForSeconds(destructionDelay);
        Destroy(gameObject);
    }

    // Mostrar las áreas de patrulla y detección en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
