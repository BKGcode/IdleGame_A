// Assets/Scripts/ScriptableObjects/WeaponData.cs
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Información de la Arma")]
    public string weaponName;
    public Sprite weaponIcon;
    public float fireRate;          // Tasa de disparo (balas por segundo)
    public int magazineSize;        // Tamaño del cargador

    [Header("Configuración de Disparo")]
    public GameObject bulletPrefab; // Prefab de la bala

    [Header("Configuración de la Bala")]
    public float bulletSpeed = 20f; // Velocidad de la bala

    // Eventos para notificar mejoras aplicadas al arma
    public event Action<UpgradeData> OnWeaponUpgraded;

    /// <summary>
    /// Método para aplicar una mejora al arma.
    /// </summary>
    /// <param name="upgrade">La mejora a aplicar.</param>
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("UpgradeData es null en ApplyUpgrade.");
            return;
        }

        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.ShooterEfficiency:
                fireRate += upgrade.value;
                break;
            case UpgradeData.UpgradeType.ShooterExpansion:
                magazineSize += (int)upgrade.value;
                break;
            // Añadir más casos según las mejoras disponibles
            default:
                Debug.LogWarning($"Tipo de mejora no manejado: {upgrade.upgradeType}");
                break;
        }

        // Notificar que se ha aplicado una mejora al arma
        OnWeaponUpgraded?.Invoke(upgrade);
    }
}
