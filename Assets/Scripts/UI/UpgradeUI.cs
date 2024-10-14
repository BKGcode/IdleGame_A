// Assets/Scripts/UI/UpgradeUI.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private ProgressionData progressionData;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeDescriptionText;

    private void OnEnable()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(ApplyUpgrade);
        }
        else
        {
            Debug.LogError("UpgradeButton no está asignado en UpgradeUI.");
        }
    }

    private void OnDisable()
    {
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveListener(ApplyUpgrade);
        }
    }

    /// <summary>
    /// Aplica una mejora seleccionada.
    /// </summary>
    public void ApplyUpgrade()
    {
        // Implementa la lógica para seleccionar y aplicar una mejora
        // Por ejemplo, seleccionar la próxima mejora disponible y aplicarla

        // Ejemplo:
        if (progressionData != null && progressionData.CanApplyNextUpgrade())
        {
            UpgradeData nextUpgrade = progressionData.GetNextUpgrade();
            progressionData.ApplyUpgrade(nextUpgrade);
            upgradeDescriptionText.text = $"Mejora aplicada: {nextUpgrade.upgradeName}";
            Debug.Log($"Mejora aplicada: {nextUpgrade.upgradeName}");
        }
        else
        {
            Debug.Log("No hay más mejoras disponibles o ProgressionData no está asignado.");
        }
    }
}
