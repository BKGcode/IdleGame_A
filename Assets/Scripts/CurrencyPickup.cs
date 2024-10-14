// Assets/Scripts/Items/CurrencyPickup.cs
using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] private int currencyAmount = 10; // Cantidad de moneda que otorga

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el Player
        if (other.CompareTag("Player"))
        {
            // Obtener el PlayerController
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                // Añadir moneda al jugador
                playerController.PlayerData.AddResources(currencyAmount);
                Debug.Log($"Moneda recogida: {currencyAmount}. Total: {playerController.PlayerData.Resources}");

                // Destruir el objeto de moneda
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("PlayerController no encontrado en el Player.");
            }
        }
    }
}
