using UnityEngine;

/// <summary>
/// Permite al jugador recoger ítems que aumentan sus vidas.
/// </summary>
public class LifePickup : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int livesToAdd = 1; // Número de vidas a añadir

    [Header("Efectos")]
    [SerializeField] private AudioClip pickupSound; // Sonido al recoger el ítem
    [SerializeField] private ParticleSystem pickupEffectPrefab; // Efecto de partículas al recoger

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtener el componente PlayerHealth del jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.AddLife();
            }

            // Reproducir sonido de recogida
            AudioSource audioSource = other.GetComponent<AudioSource>();
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }

            // Instanciar efecto de partículas
            if (pickupEffectPrefab != null)
            {
                Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
            }

            // Destruir el ítem después de recogerlo
            Destroy(gameObject);
        }
    }
}
