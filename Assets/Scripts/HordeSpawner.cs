using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeSpawner : MonoBehaviour
{
    public EnemyData[] enemyTypes;  // Array de tipos de enemigos para generar
    public float spawnRadius = 10f;  // Radio dentro del cual los enemigos serán generados
    public int initialEnemyCount = 5;  // Número inicial de enemigos en la primera oleada
    public float timeBetweenWaves = 10f;  // Tiempo entre oleadas
    public float spawnMultiplier = 1.5f;  // Multiplicador del número de enemigos por cada ola
    public float initialSpawnDelay = 5f;  // Tiempo antes de la primera generación

    private int currentWave = 0;  // Contador de la ola actual
    private bool spawning = true;  // Control para detener o continuar la generación de hordas

    private void Start()
    {
        // Inicia el proceso de generación de hordas después de un tiempo inicial
        StartCoroutine(SpawnHordes());
    }

    private IEnumerator SpawnHordes()
    {
        // Espera inicial antes de generar la primera oleada
        yield return new WaitForSeconds(initialSpawnDelay);

        while (spawning)
        {
            // Calcula el número de enemigos para la oleada actual
            int enemyCount = Mathf.CeilToInt(initialEnemyCount * Mathf.Pow(spawnMultiplier, currentWave));

            // Genera los enemigos de esta oleada
            for (int i = 0; i < enemyCount; i++)
            {
                // Selecciona un tipo de enemigo al azar
                EnemyData enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                
                // Genera el enemigo en una posición aleatoria dentro del radio basado en la posición del objeto que tiene este script
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                spawnPosition.y = transform.position.y;  // Ajustamos la altura si es necesario

                // Instancia el prefab del enemigo
                GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPosition, Quaternion.identity);

                // Ajusta otros parámetros del enemigo si es necesario
            }

            // Espera el tiempo definido entre oleadas antes de generar la siguiente
            yield return new WaitForSeconds(timeBetweenWaves);

            // Aumenta el contador de la oleada
            currentWave++;
        }
    }

    // Método para detener la generación de enemigos
    public void StopSpawning()
    {
        spawning = false;
    }

    // Método para dibujar el helper visual en la escena
    private void OnDrawGizmosSelected()
    {
        // Dibuja un gizmo de color verde para visualizar el radio de generación
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
