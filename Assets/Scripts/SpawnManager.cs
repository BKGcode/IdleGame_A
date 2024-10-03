using UnityEngine;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab; // Prefab del jugador a instanciar
    public Transform spawnPoint;     // Punto de spawn en la escena
    public float countdownTime = 10f; // Tiempo en segundos antes de spawnear al jugador

    private float currentTime;
    private bool hasSpawned = false;

    private void Start()
    {
        currentTime = countdownTime;
    }

    private void Update()
    {
        if (!hasSpawned)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                SpawnPlayer();
                hasSpawned = true;
            }
        }
    }

    private void SpawnPlayer()
    {
        if (spawnPoint != null && playerPrefab != null)
        {
            // Calcular la posición de spawn (Z -3 respecto al SpawnPoint)
            Vector3 spawnPosition = spawnPoint.position + new Vector3(0, 0, -3f);

            // Instanciar al jugador
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            // Asignar el jugador a los campos Follow y Look At de la cámara a través del GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.SetCameraTarget(playerInstance.transform);
            }
            else
            {
                Debug.LogError("GameManager.instance es null. Asegúrate de que el GameManager está en la escena.");
            }
        }
    }

    // Método opcional para reiniciar el spawn (si es necesario)
    public void ResetSpawn()
    {
        hasSpawned = false;
        currentTime = countdownTime;
    }
}
