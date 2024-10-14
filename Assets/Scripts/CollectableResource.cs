// Assets/Scripts/Shooter/CollectableResource.cs
using UnityEngine;

public class CollectableResource : MonoBehaviour
{
    [SerializeField] private CollectableResourceData resourceData;

    public int ResourceAmount => resourceData.amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.CollectResource(resourceData.amount);
                Destroy(gameObject);
                Debug.Log("Recurso recogido: " + resourceData.amount);
            }
        }
    }
}
