// Assets/Scripts/UI/BuyUpgradeButton.cs
using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeButton : MonoBehaviour
{
    [SerializeField] private int upgradeCost = 50;          // Costo de la mejora
    [SerializeField] private Button buyButton;              // Referencia al botón de compra
    [SerializeField] private PlayerController playerController; // Referencia al PlayerController
    [SerializeField] private UpgradeData upgradeData;       // Referencia a la mejora que se compra

    private void Start()
    {
        if (buyButton == null)
        {
            Debug.LogError("BuyButton no está asignado en BuyUpgradeButton.");
            return;
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController no está asignado en BuyUpgradeButton.");
            return;
        }

        if (upgradeData == null)
        {
            Debug.LogError("UpgradeData no está asignado en BuyUpgradeButton.");
            return;
        }

        // Asignar el método al evento OnClick del botón
        buyButton.onClick.AddListener(OnBuyButtonClicked);
    }

    private void OnBuyButtonClicked()
    {
        PlayerData playerData = playerController.PlayerData;
        if (playerData.SpendResources(upgradeCost))
        {
            // Aplicar la mejora al jugador
            playerData.ApplyWeaponUpgrade(upgradeData);
            Debug.Log($"Mejora comprada: {upgradeData.upgradeName}. Moneda restante: {playerData.Resources}");
        }
        else
        {
            Debug.LogWarning("No hay suficiente moneda para comprar la mejora.");
        }
    }

    private void OnDestroy()
    {
        if (buyButton != null)
        {
            buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }
    }
}
