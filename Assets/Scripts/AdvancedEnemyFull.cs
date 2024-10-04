using UnityEngine;
using UnityEngine.AI;
using System.Collections; 

public class AdvancedEnemyFull : MonoBehaviour
{
    public float patrolRadius = 15f; 
    public float idleTime = 2f; 
    public float agentSpeed = 3.5f; 
    public float detectionRadius = 10f; 
    public float chaseSpeed = 6f; 
    public int chaseProbability = 7; 
    public AudioClip collisionSound; 
    public ParticleSystem collisionParticles; 
    public float destructionDelay = 2f; 
    public AudioClip hitSound; // Sonido cuando el enemigo es impactado por un proyectil
    public ParticleSystem hitParticles; // Partículas al ser impactado por un proyectil

    private NavMeshAgent agent;
    private GameObject player;
    private AudioSource audioSource;
    private bool isChasingPlayer = false; 
    private Vector3 initialPosition; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogWarning("El NavMeshAgent no está en el NavMesh correctamente.");
            return;
        }

        initialPosition = transform.position; 
        agent.speed = agentSpeed;
        StartCoroutine(Patrol()); 
    }

    // Corrutina para el patrullaje continuo
    private IEnumerator Patrol()
    {
        while (true)
        {
            if (!isChasingPlayer) 
            {
                Vector3 patrolPoint = GetRandomPatrolPoint(transform.position, patrolRadius);

                if (patrolPoint != Vector3.zero)
                {
                    agent.SetDestination(patrolPoint);
                }
            }

            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                DetectPlayer(); 
                yield return null;
            }

            yield return new WaitForSeconds(idleTime);
        }
    }

    // Detectar si el jugador está dentro del radio de detección
    private void DetectPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                if (Random.Range(0, 11) <= chaseProbability)
                {
                    StartChasingPlayer();
                }
            }
        }
    }

    // Iniciar la persecución del jugador
    private void StartChasingPlayer()
    {
        if (!isChasingPlayer) 
        {
            isChasingPlayer = true;
            agent.speed = chaseSpeed; 
            agent.SetDestination(player.transform.position);
            Debug.Log("Iniciando persecución del jugador.");
        }
    }

    private void Update()
    {
        if (isChasingPlayer)
        {
            agent.SetDestination(player.transform.position); 
        }
    }

    // Manejar colisiones con el jugador o con proyectiles
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }

            if (collisionParticles != null)
            {
                Instantiate(collisionParticles, transform.position, Quaternion.identity);
            }

            Destroy(gameObject, destructionDelay); 
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            if (hitSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            if (hitParticles != null)
            {
                Instantiate(hitParticles, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); 
        }
    }

    // Generar un punto aleatorio dentro del NavMesh para patrullar
    private Vector3 GetRandomPatrolPoint(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero; 
    }

    // Dibujar el radio de detección en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
