using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI upgradeNameText;
    [SerializeField] private TextMeshProUGUI upgradeInfoText;
    [SerializeField] private Image upgradeImage; // Imagen que ya tiene el prefab
    [SerializeField] private Image unavailableOverlayImage; // Imagen gris que aparece si no está disponible
    [SerializeField] private Button upgradeButton;

    private UpgradeData assignedUpgrade;

    // Método para inicializar el botón con la información de la mejora
    public void Initialize(UpgradeData upgrade)
    {
        assignedUpgrade = upgrade;
        upgradeNameText.text = upgrade.upgradeName;
        upgradeInfoText.text = $"Cost: {upgrade.baseCost} | Uses: {UpgradeManager.Instance.GetRemainingUses(upgrade)}";
        
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
        // Verificamos si la mejora sigue estando disponible
        bool isAvailable = UpgradeManager.Instance.IsUpgradeAvailable(assignedUpgrade);

        // Mostramos u ocultamos la imagen gris superpuesta según la disponibilidad
        unavailableOverlayImage.gameObject.SetActive(!isAvailable);

        // Habilitamos o deshabilitamos el botón dependiendo de si la mejora está disponible
        upgradeButton.interactable = isAvailable;

        // Añadimos un Log para verificar el estado del botón y la imagen gris
        Debug.Log("Actualizando estado del botón. Disponible: " + isAvailable);
    }

    // Método que se llama cuando el jugador intenta comprar la mejora
    public void OnUpgradeButtonPressed()
    {
        Debug.Log("Intentando comprar la mejora: " + assignedUpgrade.upgradeName);

        if (UpgradeManager.Instance.TryPurchaseUpgrade(assignedUpgrade))
        {
            Debug.Log("Mejora comprada: " + assignedUpgrade.upgradeName);
            UpdateButtonState(); // Actualizar el estado visual del botón después de la compra
        }
        else
        {
            Debug.LogWarning("No se pudo comprar la mejora: " + assignedUpgrade.upgradeName);
        }
    }
}
