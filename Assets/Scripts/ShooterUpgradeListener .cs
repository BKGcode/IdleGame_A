// Assets/Scripts/UI/ShooterUpgradeListener.cs
using UnityEngine;

public class ShooterUpgradeListener : MonoBehaviour
{
    [SerializeField] private FarmData farmData;
    [SerializeField] private ShooterData shooterData;

    private void OnEnable()
    {
        farmData.OnUpgradeApplied += RespondToFarmUpgrade;
    }

    private void OnDisable()
    {
        farmData.OnUpgradeApplied -= RespondToFarmUpgrade;
    }

    private void RespondToFarmUpgrade(UpgradeData upgrade)
    {
        if (upgrade.upgradeType == UpgradeData.UpgradeType.FarmingEfficiency ||
            upgrade.upgradeType == UpgradeData.UpgradeType.FarmingExpansion)
        {
            // Lógica para responder a las mejoras de la granja en el shooter
            shooterData.ApplyUpgrade(upgrade);
        }
    }
}
