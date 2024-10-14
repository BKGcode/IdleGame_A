// Assets/Scripts/Data/ShooterData.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShooterData
{
    // Eventos para notificar cambios en los datos del shooter
    public event Action<int> OnLevelChanged;
    public event Action<WeaponData> OnWeaponUnlocked;
    public event Action<string> OnPowerUpActivated;
    public event Action<int> OnResourcesCollected; // Nuevo evento para recolectar recursos

    // Propiedades del shooter
    public int CurrentLevel { get; private set; }
    public List<WeaponData> AvailableWeapons { get; private set; }
    public List<string> ActivePowerUps { get; private set; }

    // Constructor inicial
    public ShooterData()
    {
        CurrentLevel = 1;
        AvailableWeapons = new List<WeaponData>();
        ActivePowerUps = new List<string>();
    }

    // Métodos para gestionar el shooter
    public void LevelUp()
    {
        CurrentLevel++;
        OnLevelChanged?.Invoke(CurrentLevel);
    }

    public void UnlockWeapon(WeaponData weapon)
    {
        if (!AvailableWeapons.Contains(weapon))
        {
            AvailableWeapons.Add(weapon);
            OnWeaponUnlocked?.Invoke(weapon);
        }
    }

    public void ActivatePowerUp(string powerUp)
    {
        if (!ActivePowerUps.Contains(powerUp))
        {
            ActivePowerUps.Add(powerUp);
            OnPowerUpActivated?.Invoke(powerUp);
        }
    }

    public void DeactivatePowerUp(string powerUp)
    {
        if (ActivePowerUps.Contains(powerUp))
        {
            ActivePowerUps.Remove(powerUp);
            // Puedes añadir un evento para notificar la desactivación si es necesario
        }
    }

    public void CollectResources(int amount)
    {
        OnResourcesCollected?.Invoke(amount);
    }

    // Método para aplicar una mejora (upgrade)
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.FarmingEfficiency:
                // Implementa la lógica específica para FarmingEfficiency
                break;
            case UpgradeData.UpgradeType.FarmingExpansion:
                // Implementa la lógica específica para FarmingExpansion
                break;
            case UpgradeData.UpgradeType.ShooterEfficiency:
                // Implementa la lógica específica para ShooterEfficiency
                break;
            case UpgradeData.UpgradeType.ShooterExpansion:
                // Implementa la lógica específica para ShooterExpansion
                break;
        }

        // Notificar que se ha aplicado una mejora
        OnWeaponUnlocked?.Invoke(null); // O utiliza otro evento si es más apropiado
    }

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(int level, List<WeaponData> weapons, List<string> powerUps)
    {
        CurrentLevel = level;
        AvailableWeapons = weapons;
        ActivePowerUps = powerUps;

        OnLevelChanged?.Invoke(CurrentLevel);
        // Puedes notificar también sobre las armas desbloqueadas y power-ups activos
    }
}
