// Assets/Scripts/UI/WeaponUpgradeListener.cs
using UnityEngine;

public class WeaponUpgradeListener : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private PlayerData playerData;

    private void OnEnable()
    {
        if (weaponData != null)
        {
            weaponData.OnWeaponUpgraded += RespondToWeaponUpgrade;
        }
        else
        {
            Debug.LogError("WeaponData no está asignado en WeaponUpgradeListener.");
        }
    }

    private void OnDisable()
    {
        if (weaponData != null)
        {
            weaponData.OnWeaponUpgraded -= RespondToWeaponUpgrade;
        }
    }

    /// <summary>
    /// Responde a la aplicación de una mejora en el arma.
    /// </summary>
    /// <param name="upgrade">La mejora aplicada.</param>
    public void RespondToWeaponUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("UpgradeData es null en RespondToWeaponUpgrade.");
            return;
        }

        if (upgrade.upgradeType == UpgradeData.UpgradeType.ShooterEfficiency ||
            upgrade.upgradeType == UpgradeData.UpgradeType.ShooterExpansion)
        {
            // Lógica para responder a las mejoras del arma
            if (playerData != null)
            {
                playerData.ApplyWeaponUpgrade(upgrade);
                Debug.Log($"Mejora aplicada al arma: {upgrade.upgradeName}");
            }
            else
            {
                Debug.LogError("PlayerData no está asignado en WeaponUpgradeListener.");
            }
        }
    }
}
