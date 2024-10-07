using UnityEngine;
using System.Collections;

public class EnemyPatrolController : MonoBehaviour
{
    // Variables de patrulla
    [SerializeField] private float patrolRadius = 10f; // Radio de patrulla
    [SerializeField] private float patrolSpeed = 2f; // Velocidad de movimiento durante la patrulla
    [SerializeField] private float minPatrolCooldown = 2f; // Tiempo mínimo antes de moverse a otro punto
    [SerializeField] private float maxPatrolCooldown = 5f; // Tiempo máximo antes de moverse a otro punto
    
    // Variables de detección del Player
    [SerializeField] private float detectionRadius = 5f; // Radio de detección del Player
    [SerializeField] private float chaseSpeed = 4f; // Velocidad durante la persecución
    [SerializeField] private int chaseProbability = 7; // Probabilidad de 0 a 10 de perseguir al Player

    private Vector3 patrolCenter; // Centro de la patrulla
    private Transform player; // Referencia al Player
    private bool isChasing = false; // Estado de persecución
    private bool isPatrolling = true; // Estado de patrulla
    private float cooldownTimer = 0f; // Tiempo de cooldown antes de moverse a otro punto
    private Vector3 nextPatrolPoint; // Siguiente punto de patrulla

    private void Start()
    {
        // Definir el centro de patrulla como la posición inicial del enemigo
        patrolCenter = transform.position;

        // Encontrar al Player usando tags
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Elegir el primer punto de patrulla
        ChooseNextPatrolPoint();
    }

    private void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isPatrolling)
        {
            Patrol();
        }

        DetectPlayer();
    }

    // Elegir un nuevo punto de patrulla aleatorio dentro del radio
    private void ChooseNextPatrolPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius; // Elegir un punto aleatorio en el círculo de patrulla
        nextPatrolPoint = patrolCenter + new Vector3(randomPoint.x, 0, randomPoint.y); // Definir la posición del siguiente punto de patrulla
        cooldownTimer = Random.Range(minPatrolCooldown, maxPatrolCooldown); // Reiniciar el cooldown
    }

    // Patrullar hacia el siguiente punto de patrulla
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, nextPatrolPoint) <= 0.2f)
        {
            ChooseNextPatrolPoint(); // Elegir el siguiente punto si ya llegó al actual
        }
        else
        {
            MoveToPoint(nextPatrolPoint, patrolSpeed); // Moverse al siguiente punto
        }
    }

    // Perseguir al Player si está en el radio de detección y pasa la probabilidad de persecución
    private void ChasePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > detectionRadius)
        {
            // Si el Player se ha salido del radio, volver a patrullar
            isChasing = false;
            isPatrolling = true;
        }
        else
        {
            MoveToPoint(player.position, chaseSpeed); // Perseguir al Player
        }
    }

    // Detectar si el Player está en el radio de detección
    private void DetectPlayer()
    {
        if (player == null) return;

        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            int randomChance = Random.Range(0, 11); // Generar un valor aleatorio de 0 a 10
            if (randomChance <= chaseProbability)
            {
                isChasing = true;
                isPatrolling = false; // Dejar de patrullar cuando persiga
            }
        }
    }

    // Moverse hacia un punto con una velocidad específica y orientarse hacia el movimiento
    private void MoveToPoint(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            RotateTowardsMovement(target, 5f); // Rotar hacia el objetivo
        }
        transform.position += direction * speed * Time.deltaTime; // Moverse hacia el punto objetivo
    }

    // Método para actualizar la rotación del enemigo en la dirección de movimiento
    private void RotateTowardsMovement(Vector3 targetPosition, float rotationSpeed)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Dibujar Gizmos para mostrar el radio de patrulla y de detección en la vista de escena
    private void OnDrawGizmosSelected()
    {
        // Radio de patrulla
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        // Radio de detección del Player
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
