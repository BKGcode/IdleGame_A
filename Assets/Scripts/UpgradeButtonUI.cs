using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeCostText; 
    [SerializeField] private TextMeshProUGUI upgradeUsesText; 
    [SerializeField] private Image upgradeImage; 
    [SerializeField] private Image unavailableOverlayImage;
    [SerializeField] private Button upgradeButton;

    private UpgradeData assignedUpgrade;

    // Método para inicializar el botón con la información de la mejora
    public void Initialize(UpgradeData upgrade)
    {
        assignedUpgrade = upgrade;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeCostText.text = upgrade.baseCost.ToString(); // Asigna solo el valor del coste
        upgradeUsesText.text = UpgradeManager.Instance.GetRemainingUses(upgrade).ToString(); // Asigna solo el valor de usos
        
        // Actualizar el estado del botón cuando se inicializa
        UpdateButtonState();
    }

    // Método que se ejecuta en cada frame
    private void Update()
    {
        // Asegurarse de que el estado del botón se actualiza constantemente
        UpdateButtonState();
    }

    // Actualiza el estado visual del botón
    private void UpdateButtonState()
    {
        bool isAvailable = UpgradeManager.Instance.IsUpgradeAvailable(assignedUpgrade);
        unavailableOverlayImage.gameObject.SetActive(!isAvailable);
        upgradeButton.interactable = isAvailable;
    }

    // Método que se llama cuando el jugador intenta comprar la mejora
    public void OnUpgradeButtonPressed()
    {
        if (UpgradeManager.Instance.TryPurchaseUpgrade(assignedUpgrade))
        {
            UpdateButtonState(); // Actualizar el estado visual del botón después de la compra

            // Actualizar el texto de usos después de la compra
            upgradeUsesText.text = UpgradeManager.Instance.GetRemainingUses(assignedUpgrade).ToString();
        }
    }
}
