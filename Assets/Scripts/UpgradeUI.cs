using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeSystem upgradeSystem;  // Referencia al sistema de mejoras
    public UpgradeData upgradeData;  // Referencia a los datos de la mejora
    public Button purchaseButton;  // Botón para comprar la mejora

    private void Start()
    {
        purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    // Método para manejar la compra de la mejora
    private void OnPurchaseClicked()
    {
        bool success = upgradeSystem.PurchaseUpgrade(upgradeData);
        if (success)
        {
            purchaseButton.interactable = false;  // Deshabilitamos el botón si la mejora se ha comprado
        }
    }
}
