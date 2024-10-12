using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    [Header("Prefab Settings")]
    [Tooltip("Prefab que se va a spawnear.")]
    public GameObject prefabToSpawn;

    [Tooltip("Cantidad de prefabs a spawnear.")]
    [Min(1)]
    public int numberOfPrefabs = 5;

    [Header("Spawn Area Settings")]
    [Tooltip("Centro del área de spawn.")]
    public Vector3 spawnCenter = Vector3.zero;

    [Tooltip("Radio dentro del cual se spawnearán los prefabs.")]
    [Min(0)]
    public float spawnRadius = 10f;

    [Tooltip("Distancia mínima desde el centro para spawnear.")]
    [Min(0)]
    public float minDistance = 2f;

    [Tooltip("Número máximo de intentos para encontrar una posición válida.")]
    [Range(1, 100)]
    public int maxSpawnAttempts = 10;

    private void Start()
    {
        SpawnPrefabs();
        Destroy(gameObject);
    }

    private void SpawnPrefabs()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError("PrefabSpawner: No se ha asignado ningún prefab para spawnear.");
            return;
        }

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector3? spawnPosition = GetRandomPosition();
            if (spawnPosition.HasValue)
            {
                Instantiate(prefabToSpawn, spawnPosition.Value, Quaternion.identity);
                Debug.Log($"Prefab spawneado en posición: {spawnPosition.Value}");
            }
            else
            {
                Debug.LogWarning($"PrefabSpawner: No se pudo encontrar una posición válida para el prefab {i + 1}.");
            }
        }
    }

    private Vector3? GetRandomPosition()
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection.y = spawnCenter.y; // Mantener en el mismo plano si es necesario
            Vector3 spawnPos = spawnCenter + randomDirection;

            if (Vector3.Distance(spawnPos, spawnCenter) >= minDistance)
            {
                return spawnPos;
            }
        }

        // Retorna null si no se encuentra una posición válida después de los intentos
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de spawn
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnCenter, spawnRadius);

        // Dibujar la zona mínima de spawn
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnCenter, minDistance);

        // Opcional: Dibujar el centro de spawn
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(spawnCenter, 0.2f);
    }
}
