// Assets/Scripts/UI/UpgradeUI.cs
using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private ProgressionData progressionData;
    [SerializeField] private TextMeshProUGUI farmingEfficiencyText;
    [SerializeField] private TextMeshProUGUI farmingAreaText;
    [SerializeField] private TextMeshProUGUI shooterEfficiencyText;
    [SerializeField] private TextMeshProUGUI shooterCapacityText;

    private void OnEnable()
    {
        progressionData.OnProgressionUpdated += UpdateProgressionUI;
        // Inicializar UI con los valores actuales
        UpdateProgressionUI(null);
    }

    private void OnDisable()
    {
        progressionData.OnProgressionUpdated -= UpdateProgressionUI;
    }

    private void UpdateProgressionUI(UpgradeData upgrade)
    {
        farmingEfficiencyText.text = $"Eficiencia Farming: {progressionData.FarmingEfficiency}";
        farmingAreaText.text = $"Área Farming: {progressionData.FarmingArea}";
        shooterEfficiencyText.text = $"Eficiencia Shooter: {progressionData.ShooterEfficiency}";
        shooterCapacityText.text = $"Capacidad Shooter: {progressionData.ShooterCapacity}";
    }
}
