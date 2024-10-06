using UnityEngine;

public class EnemyFXController : MonoBehaviour
{
    public AudioSource audioSource;  // El componente AudioSource que manejará el audio
    public AudioClip impactSound;  // El sonido que se reproduce al impactar
    public GameObject impactParticlesPrefab;  // Prefab del sistema de partículas a instanciar
    public Transform particleSpawnPoint;  // Punto donde se instanciarán las partículas

    private void Start()
    {
        // Autoasignación del AudioSource si no ha sido asignado manualmente
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Puedes añadir una advertencia si no se encuentra el AudioSource
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on the object. Please add one.");
        }
    }

    // Método para reproducir los efectos de impacto
    public void PlayImpactFX()
    {
        if (impactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(impactSound);  // Reproduce el sonido una vez
        }

        if (impactParticlesPrefab != null)
        {
            // Instanciamos las partículas en la posición del impacto
            GameObject particles = Instantiate(impactParticlesPrefab, particleSpawnPoint.position, Quaternion.identity);

            // Destruimos las partículas después de su duración
            Destroy(particles, 2f);  // Ajusta el tiempo según la duración del sistema de partículas
        }
    }
}
