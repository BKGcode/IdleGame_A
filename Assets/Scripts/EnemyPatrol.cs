using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    // Variables para patrullar
    public float patrolRadius = 10f; // Radio de patrulla desde la posición inicial
    public float idleTimeMin = 2f; // Tiempo mínimo de espera entre movimientos
    public float idleTimeMax = 5f; // Tiempo máximo de espera entre movimientos

    // Variables para detección del jugador
    public float detectionRadius = 5f; // Radio de detección del jugador
    public int chaseProbability = 5; // Probabilidad de persecución (0 a 10)
    public float chaseSpeed = 6f; // Velocidad de persecución

    // Variables para efectos al colisionar con el jugador o proyectil
    public AudioClip collisionSound; // Sonido al colisionar
    public ParticleSystem collisionParticles; // Partículas al colisionar
    public AudioClip hitSound; // Sonido cuando el enemigo es impactado por un proyectil
    public ParticleSystem hitParticles; // Partículas al ser impactado por un proyectil
    public float destructionDelay = 2f; // Tiempo antes de destruir al enemigo tras la colisión

    // Variables internas
    private Vector3 initialPosition; // Posición inicial del enemigo
    private NavMeshAgent agent; // Componente NavMeshAgent
    private GameObject player; // Referencia al jugador
    private float defaultSpeed; // Velocidad por defecto del enemigo
    private bool isChasingPlayer = false; // Verifica si está persiguiendo al jugador
    private AudioSource audioSource; // Fuente de audio para reproducir el sonido

    private void Start()
    {
        // Guardamos la posición inicial y obtenemos el NavMeshAgent
        initialPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player"); // Asignar manualmente si es necesario
        defaultSpeed = agent.speed; // Guardar la velocidad por defecto

        // Componente de audio
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Patrol()); // Iniciar el sistema de patrulla
    }

    // Corrutina de patrulla
    private IEnumerator Patrol()
    {
        while (true)
        {
            // Elegir un punto aleatorio dentro del radio de patrulla
            Vector3 randomPoint = RandomNavSphere(initialPosition, patrolRadius, -1);

            // Mover al agente al punto
            agent.SetDestination(randomPoint);

            // Esperar a que el agente llegue al destino
            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                // Mientras patrulla, verificar si el jugador está en rango de detección
                DetectPlayer();
                yield return null;
            }

            // Esperar un tiempo aleatorio antes de ir al siguiente punto
            yield return new WaitForSeconds(Random.Range(idleTimeMin, idleTimeMax));
        }
    }

    // Método para detección y persecución del jugador
    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            // Probabilidad de persecución
            if (Random.Range(0, 11) <= chaseProbability)
            {
                // Cambiar la velocidad a la de persecución y perseguir al jugador
                agent.speed = chaseSpeed;
                agent.SetDestination(player.transform.position);
                isChasingPlayer = true; // Marcar que está persiguiendo
            }
            else
            {
                // Volver a la velocidad normal
                agent.speed = defaultSpeed;
                isChasingPlayer = false; // No está persiguiendo
            }
        }
        else
        {
            isChasingPlayer = false; // Si está fuera del rango, no persigue
        }
    }

    // Método para encontrar un punto aleatorio dentro del NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    // Método para manejar colisiones con el jugador y proyectiles
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reproducir el sonido de colisión si está asignado
            if (collisionSound != null)
            {
                audioSource.PlayOneShot(collisionSound);
            }

            // Emitir las partículas si están asignadas
            if (collisionParticles != null)
            {
                Instantiate(collisionParticles, transform.position, Quaternion.identity);
            }

            // Destruir el enemigo después de un retraso
            Destroy(gameObject, destructionDelay);
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            // Reproducir el sonido del impacto de proyectil si está asignado
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // Emitir las partículas si están asignadas
            if (hitParticles != null)
            {
                Instantiate(hitParticles, transform.position, Quaternion.identity);
            }

            // Destruir el enemigo inmediatamente al ser impactado por un proyectil
            Destroy(gameObject);
        }
    }

    // Método para dibujar los gizmos de los radios de patrulla y detección en el editor
    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de patrulla
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(initialPosition != Vector3.zero ? initialPosition : transform.position, patrolRadius);

        // Dibujar el radio de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Dibujar una línea hacia el jugador cuando esté persiguiéndolo
        if (isChasingPlayer && player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}
