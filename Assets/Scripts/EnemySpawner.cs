using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnRadius = 10f;
    public float timeToFirstWave = 5f;
    public float timeBetweenWaves = 30f;
    public int enemiesInFirstWave = 5;
    public float additionalEnemiesPercentage = 20f;

    [Header("Enemy Types")]
    public List<EnemyTypeSO> enemyTypes;

    [Header("Debug Visualization")]
    public bool showSpawnArea = true;
    public bool showSpawnPoints = true;
    public int debugSpawnPointCount = 20;
    public Color spawnAreaColor = new Color(1f, 0f, 0f, 0.2f);
    public Color spawnPointColor = Color.yellow;

    private int currentWave = 0;
    private int enemiesToSpawn;
    private List<Vector3> debugSpawnPoints = new List<Vector3>();

    private void Start()
    {
        StartCoroutine(SpawnWaves());
        GenerateDebugSpawnPoints();
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(timeToFirstWave);

        while (true)
        {
            currentWave++;
            enemiesToSpawn = CalculateEnemiesForWave();
            
            // Reproducir el sonido una vez al inicio de la oleada
            SpawnSoundManager.Instance.PlaySpawnSound();

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }

            Debug.Log($"Wave {currentWave} completed. Spawned {enemiesToSpawn} enemies.");
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = transform.position + new Vector3(spawnPoint.x, 0, spawnPoint.y);

        EnemyTypeSO enemyTypeToSpawn = enemyTypes[Random.Range(0, enemyTypes.Count)];
        GameObject spawnedEnemy = Instantiate(enemyTypeToSpawn.enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Asignar el tipo de enemigo al controlador
        EnemyPatrolController controller = spawnedEnemy.GetComponent<EnemyPatrolController>();
        if (controller != null)
        {
            controller.enemyType = enemyTypeToSpawn; // Ahora accesible
        }
        else
        {
            Debug.LogWarning("El prefab del enemigo no tiene un EnemyPatrolController adjunto.");
        }
    }

    private int CalculateEnemiesForWave()
    {
        if (currentWave == 1) return enemiesInFirstWave;
        
        float increasePercentage = 1f + (additionalEnemiesPercentage / 100f);
        return Mathf.RoundToInt(enemiesInFirstWave * Mathf.Pow(increasePercentage, currentWave - 1));
    }

    private void GenerateDebugSpawnPoints()
    {
        debugSpawnPoints.Clear();
        for (int i = 0; i < debugSpawnPointCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            debugSpawnPoints.Add(transform.position + new Vector3(randomPoint.x, 0, randomPoint.y));
        }
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnArea && !showSpawnPoints) return;

        if (showSpawnArea)
        {
            Gizmos.color = spawnAreaColor;
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        if (showSpawnPoints)
        {
            Gizmos.color = spawnPointColor;
            foreach (Vector3 point in debugSpawnPoints)
            {
                Gizmos.DrawSphere(point, 0.2f);
            }
        }
    }

    private void OnValidate()
    {
        GenerateDebugSpawnPoints();
    }
}
