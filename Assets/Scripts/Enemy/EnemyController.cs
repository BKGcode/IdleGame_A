using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] private int health = 50;
    [SerializeField] private float attackDamage = 1f;

    [Header("Configuración de Patrulla")]
    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float minPatrolWaitTime = 2f;
    [SerializeField] private float maxPatrolWaitTime = 5f;

    [Header("Configuración de Detección")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] [Range(0, 10)] private int chaseChance = 5;

    [Header("Configuración de Persecución")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private Vector3 randomDestination;
    private bool isChasing = false;
    private bool isWaiting = false;
    private Vector3 lastPosition;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en el objeto enemigo.");
            return;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Player'.");
        }

        agent.speed = patrolSpeed;
        SetRandomDestination();
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (!isWaiting)
        {
            Patrol();
        }

        DetectPlayer();
        HandleRotation();
    }

    private void Patrol()
    {
        if (agent.remainingDistance < 0.1f)
        {
            if (!isWaiting)
            {
                StartCoroutine(WaitAtPatrolPoint());
            }
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        randomDestination = hit.position;
        agent.SetDestination(randomDestination);
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        isWaiting = true;
        agent.isStopped = true;
        float waitTime = Random.Range(minPatrolWaitTime, maxPatrolWaitTime);
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
        SetRandomDestination();
        isWaiting = false;
    }

    private void DetectPlayer()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= detectionRadius)
        {
            if (Random.Range(0, 10) < chaseChance)
            {
                StartChasing();
            }
        }
    }

    private void StartChasing()
    {
        if (!isChasing)
        {
            isChasing = true;
            agent.speed = chaseSpeed;
            agent.stoppingDistance = 1f;
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                AttackPlayer();
            }
        }
    }

    private void AttackPlayer()
    {
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage((int)attackDamage);
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void HandleRotation()
    {
        if (isChasing && playerTransform != null)
        {
            // Orientar hacia el jugador cuando lo está persiguiendo
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Orientar en la dirección del movimiento durante la patrulla
            Vector3 movement = transform.position - lastPosition;
            if (movement.magnitude > 0.01f)
            {
                movement.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(movement.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        lastPosition = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}