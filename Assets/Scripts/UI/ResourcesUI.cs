// Assets/Scripts/UI/ResourcesUI.cs
using UnityEngine;
using TMPro; // Si usas TextMeshPro

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText; // Referencia al texto de moneda
    [SerializeField] private PlayerController playerController; // Referencia al PlayerController

    private PlayerData playerData;

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController no está asignado en ResourcesUI.");
            return;
        }

        playerData = playerController.PlayerData;

        if (playerData == null)
        {
            Debug.LogError("PlayerData no está asignado en PlayerController.");
            return;
        }

        if (currencyText == null)
        {
            Debug.LogError("CurrencyText no está asignado en ResourcesUI.");
            return;
        }

        // Inicializar el texto con la cantidad actual de moneda
        UpdatePlayerResourcesUI(playerData.Resources);

        // Suscribirse al evento para actualizar el texto cuando cambie la moneda
        playerData.OnResourcesChanged += UpdatePlayerResourcesUI;
    }

    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnResourcesChanged -= UpdatePlayerResourcesUI;
        }
    }

    /// <summary>
    /// Actualiza el texto de la moneda en la UI.
    /// </summary>
    /// <param name="currentResources">La cantidad actual de moneda.</param>
    public void UpdatePlayerResourcesUI(int currentResources)
    {
        if (currencyText != null)
        {
            currencyText.text = $"Moneda: {currentResources}";
        }
    }
}
