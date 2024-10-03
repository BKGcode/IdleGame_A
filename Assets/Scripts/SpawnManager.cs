using UnityEngine;
using Cinemachine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab; // Prefab del jugador a instanciar
    public Transform spawnPoint;     // Punto de spawn en la escena
    public float countdownTime = 10f; // Tiempo en segundos antes de spawnear al jugador

    [Header("Audio Settings")]
    public AudioClip spawnSound;      // Sonido a reproducir al spawnear al jugador
    private AudioSource audioSource;

    [Header("Camera Settings")]
    public CinemachineVirtualCamera virtualCamera; // Referencia a la Cinemachine Virtual Camera

    private float currentTime;
    private bool hasSpawned = false;

    private void Start()
    {
        currentTime = countdownTime;

        // Obtener o añadir el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Asignar el AudioClip al AudioSource
        if (spawnSound != null)
        {
            audioSource.clip = spawnSound;
        }
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

            // Reproducir el sonido de spawn
            if (audioSource != null && spawnSound != null)
            {
                audioSource.PlayOneShot(spawnSound);
            }

            // Asignar el jugador a los campos Follow y Look At de la cámara
            if (virtualCamera != null)
            {
                virtualCamera.Follow = playerInstance.transform;
                virtualCamera.LookAt = playerInstance.transform;
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
