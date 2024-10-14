using UnityEngine;
using ShooterGame.Player;

namespace ShooterGame.Items
{
    public class CollectableResource : MonoBehaviour
    {
        [SerializeField] private CollectableResourceData resourceData;
        [SerializeField] private ParticleSystem collectEffect;
        [SerializeField] private AudioSource collectSound;

        public int ResourceAmount => resourceData.amount;

        // Evento para notificar cuando se recoge un recurso
        public event System.Action<int> OnResourceCollected;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    Collect(playerController);
                }
                else
                {
                    Debug.LogWarning("PlayerController no encontrado en el objeto con tag Player.");
                }
            }
        }

        private void Collect(PlayerController collector)
        {
            collector.CollectResource(ResourceAmount);
            OnResourceCollected?.Invoke(ResourceAmount);
            PlayCollectEffects();
            Debug.Log($"Recurso recogido: {ResourceAmount}");
            Destroy(gameObject);
        }

        private void PlayCollectEffects()
        {
            if (collectEffect != null)
            {
                ParticleSystem effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration);
            }
            else
            {
                Debug.LogWarning("Efecto de partículas no asignado en CollectableResource.");
            }

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound.clip, transform.position);
            }
            else
            {
                Debug.LogWarning("Sonido de recolección no asignado en CollectableResource.");
            }
        }

        // Método para configurar los datos del recurso en tiempo de ejecución
        public void SetResourceData(CollectableResourceData newResourceData)
        {
            if (newResourceData != null)
            {
                resourceData = newResourceData;
            }
            else
            {
                Debug.LogError("Intento de asignar CollectableResourceData nulo.");
            }
        }

        // Método para obtener el tipo de recurso (si se implementa en CollectableResourceData)
        public string GetResourceType()
        {
            return resourceData != null ? resourceData.resourceType : "Desconocido";
        }

        // Método para verificar si el recurso es coleccionable (podría usarse para recursos que requieren ciertos requisitos)
        public bool IsCollectable(PlayerController player)
        {
            // Aquí se podría implementar lógica adicional, por ejemplo, verificar si el jugador tiene cierto nivel o habilidad
            return true;
        }

        // Método para aplicar efectos adicionales al recoger el recurso
        private void ApplyAdditionalEffects(PlayerController player)
        {
            // Aquí se podrían implementar efectos adicionales, como dar experiencia, aplicar buffs, etc.
            Debug.Log($"Efectos adicionales aplicados al jugador al recoger {GetResourceType()}");
        }
    }
}