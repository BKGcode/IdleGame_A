using UnityEngine;

public class EnemyPatrolController : MonoBehaviour
{
    // Variables de patrulla y detección
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private int chaseProbability = 5;

    private Vector3 patrolCenter;
    private Vector3 targetPoint;
    private bool isPatrolling = true;
    private bool isChasing = false;
    private Transform player;

    private void Start()
    {
        patrolCenter = transform.position;
        SetRandomTargetPoint();
    }

    private void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        DetectPlayer();
    }

    // Patrullaje hacia un punto aleatorio
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, targetPoint) < 0.5f)
        {
            SetRandomTargetPoint();
        }
        else
        {
            MoveToPoint(targetPoint, patrolSpeed);
        }
    }

    // Perseguir al Player si está dentro del radio de detección
    private void ChasePlayer()
    {
        if (player != null)
        {
            MoveToPoint(player.position, chaseSpeed);
        }
    }

    // Detectar si el Player está en el radio de detección y establecer la probabilidad de persecución
    private void DetectPlayer()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            int randomChance = Random.Range(0, 11);
            if (randomChance <= chaseProbability)
            {
                isChasing = true;
                isPatrolling = false;
            }
        }
    }

    // Moverse hacia un punto con una velocidad específica
    private void MoveToPoint(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            RotateTowardsMovement(target, 5f);
        }
        transform.position += direction * speed * Time.deltaTime;
    }

    // Rotar hacia el movimiento
    private void RotateTowardsMovement(Vector3 targetPosition, float rotationSpeed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Asignar un nuevo punto aleatorio dentro del radio de patrulla
    private void SetRandomTargetPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        targetPoint = patrolCenter + new Vector3(randomPoint.x, 0, randomPoint.y);
    }

    // Dibujar Gizmos para visualizar el radio de patrulla y detección
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
