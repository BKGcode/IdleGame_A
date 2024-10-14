// Assets/Scripts/UI/ShooterUpgradeListener.cs
using UnityEngine;

public class ShooterUpgradeListener : MonoBehaviour
{
    [SerializeField] private FarmData farmData;
    [SerializeField] private ShooterData shooterData;

    private void OnEnable()
    {
        if (farmData != null)
        {
            farmData.OnUpgradeApplied += RespondToFarmUpgrade;
        }
        else
        {
            Debug.LogError("FarmData no está asignado en ShooterUpgradeListener.");
        }
    }

    private void OnDisable()
    {
        if (farmData != null)
        {
            farmData.OnUpgradeApplied -= RespondToFarmUpgrade;
        }
    }

    /// <summary>
    /// Responde a la aplicación de una mejora en la granja.
    /// </summary>
    /// <param name="upgrade">La mejora aplicada.</param>
    public void RespondToFarmUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("UpgradeData es null en RespondToFarmUpgrade.");
            return;
        }

        if (upgrade.upgradeType == UpgradeData.UpgradeType.FarmingEfficiency ||
            upgrade.upgradeType == UpgradeData.UpgradeType.FarmingExpansion)
        {
            // Lógica para responder a las mejoras de la granja en el shooter
            if (shooterData != null)
            {
                shooterData.ApplyUpgrade(upgrade);
                Debug.Log($"Mejora aplicada al shooter: {upgrade.upgradeName}");
            }
            else
            {
                Debug.LogError("ShooterData no está asignado en ShooterUpgradeListener.");
            }
        }
    }
}
