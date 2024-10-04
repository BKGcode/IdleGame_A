using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NavMeshEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // El prefab del enemigo que vas a generar
    public Transform spawnCenter; // El punto central desde el cual se genera el enemigo
    public float minDistance = 20f; // Distancia mínima de generación
    public float maxDistance = 30f; // Distancia máxima de generación
    public int initialNumberOfEnemies = 5; // Número inicial de enemigos en la primera oleada
    public float enemyIncreasePercentage = 20f; // Porcentaje de incremento en el número de enemigos por oleada
    public float maxNavMeshDistance = 10f; // Distancia máxima de búsqueda en el NavMesh
    public float timeUntilFirstHorde = 20f; // Tiempo antes de la primera oleada
    public float timeBetweenHordes = 30f; // Tiempo entre oleadas posteriores
    public int maxAttempts = 10; // Máximo de intentos para encontrar una posición válida en el NavMesh

    private int currentNumberOfEnemies; // Número actual de enemigos en la oleada

    private void Start()
    {
        // Establecer el número inicial de enemigos en la primera oleada
        currentNumberOfEnemies = initialNumberOfEnemies;

        // Iniciar la corrutina para el ciclo de oleadas
        StartCoroutine(HordeCycle());
    }

    // Corrutina que gestiona el ciclo de hordas
    private IEnumerator HordeCycle()
    {
        // Esperar el tiempo definido antes de generar la primera oleada
        yield return new WaitForSeconds(timeUntilFirstHorde);

        while (true)
        {
            // Generar la oleada de enemigos
            SpawnEnemies();

            // Incrementar el número de enemigos para la siguiente oleada
            currentNumberOfEnemies = Mathf.CeilToInt(currentNumberOfEnemies * (1 + enemyIncreasePercentage / 100f));

            // Esperar el tiempo entre oleadas
            yield return new WaitForSeconds(timeBetweenHordes);
        }
    }

    // Método para generar enemigos
    private void SpawnEnemies()
    {
        for (int i = 0; i < currentNumberOfEnemies; i++)
        {
            // Generar una posición aleatoria en el NavMesh
            Vector3 spawnPosition = GenerateRandomNavMeshPosition(spawnCenter.position, minDistance, maxDistance);

            if (spawnPosition != Vector3.zero)
            {
                // Instanciar el enemigo en la posición generada
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No se encontró una posición válida en el NavMesh para generar el enemigo.");
            }
        }
    }

    // Método para obtener un punto aleatorio en el NavMesh, fuera del radio mínimo
    private Vector3 GenerateRandomNavMeshPosition(Vector3 center, float minRadius, float maxRadius)
    {
        Vector3 randomDirection;
        float radius;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Generar un ángulo y distancia aleatorios
            float angle = Random.Range(0f, Mathf.PI * 2);
            radius = Random.Range(minRadius, maxRadius);

            // Calcular la dirección en la que se genera
            randomDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            // Sumar la dirección al centro
            randomDirection += center;

            // Obtener una posición válida en el NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, maxNavMeshDistance, NavMesh.AllAreas))
            {
                return hit.position; // Retornar la posición si se encuentra
            }

            attempts++;
        }

        return Vector3.zero; // Si no encuentra ninguna posición válida, devolver Vector3.zero
    }
}
