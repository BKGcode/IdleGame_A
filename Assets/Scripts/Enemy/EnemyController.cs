// Assets/Scripts/Enemy/EnemyController.cs
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int health = 50;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private Transform playerTransform;
    private NavMeshAgent agent;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró un GameObject con la etiqueta 'Player'.");
        }

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= detectionRadius)
        {
            agent.SetDestination(playerTransform.position);

            if (distance <= agent.stoppingDistance && Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            agent.ResetPath();
        }
    }

    private void AttackPlayer()
    {
        // Accede a PlayerController y aplica daño
        PlayerController playerController = playerTransform.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage((int)attackDamage);
            Debug.Log($"Enemigo atacó al jugador. Salud restante: {playerController.PlayerData.Health}");
        }
        else
        {
            Debug.LogError("PlayerController no está asignado en el jugador.");
        }
    }

    /// <summary>
    /// Aplica daño al enemigo.
    /// </summary>
    /// <param name="damage">Cantidad de daño a aplicar.</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Maneja la lógica cuando el enemigo muere.
    /// </summary>
    private void Die()
    {
        // Implementa la lógica de muerte, como desactivar el enemigo
        Destroy(gameObject);
        Debug.Log("Enemigo muerto.");
    }
}
