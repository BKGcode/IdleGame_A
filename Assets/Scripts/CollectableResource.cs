// Assets/Scripts/UI/CollectableResource.cs
using UnityEngine;

public class CollectableResource : MonoBehaviour
{
    [SerializeField] private CollectableResourceData resourceData;
    [SerializeField] private ShooterData shooterData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Supongamos que el jugador tiene un script con PlayerData accesible
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.CollectResource(resourceData.amount);
                shooterData.CollectResources(resourceData.amount); // Dispara el evento
                Destroy(gameObject);
            }
        }
    }
}
