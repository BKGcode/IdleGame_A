using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float maxDistance;
    private float explosionRadius;
    private Vector3 startPosition;
    public float speed = 20f; // Velocidad del proyectil
    private Rigidbody rb; // Para controlar el movimiento del proyectil

    private void Start()
    {
        startPosition = transform.position;

        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();

        // Asignar la velocidad al proyectil
        rb.linearVelocity = transform.forward * speed;

        // Ignorar colisiones con cualquier objeto que tenga el tag "Player"
        Collider projectileCollider = GetComponent<Collider>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            if (playerCollider != null)
            {
                Physics.IgnoreCollision(projectileCollider, playerCollider);
            }
        }
    }

    private void Update()
    {
        // Verificar si el proyectil ha recorrido su distancia máxima
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            DestroyProjectile();
        }
    }

    // Método para configurar la distancia máxima y el radio de explosión del proyectil
    public void SetParameters(float distance, float radius)
    {
        maxDistance = distance;
        explosionRadius = radius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosionRadius > 0)
        {
            Explode();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destruir al enemigo al impactar
            Destroy(collision.gameObject);
            DestroyProjectile();
        }
    }

    // Método para manejar la explosión del proyectil
    private void Explode()
    {
        // Detectar todos los objetos dentro del radio de explosión
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                // Destruir todos los enemigos dentro del radio de explosión
                Destroy(nearbyObject.gameObject);
            }
        }

        // Destruir el proyectil después de explotar
        DestroyProjectile();
    }

    // Método para destruir el proyectil
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
