// Assets/Scripts/Data/ProgressionData.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProgressionData
{
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    // Eventos para notificar cambios en la progresión
    public event Action<UpgradeData> OnProgressionUpdated;

    // Propiedades de progresión
    public float FarmingEfficiency { get; private set; }
    public int FarmingArea { get; private set; }
    public float ShooterEfficiency { get; private set; }
    public int ShooterCapacity { get; private set; }
    public DifficultyLevel CurrentDifficulty { get; private set; }

    [Header("Lista de Mejoras")]
    public List<UpgradeData> UpgradeList;
    private int currentUpgradeIndex = 0;

    // Constructor inicial
    public ProgressionData()
    {
        FarmingEfficiency = 1f;
        FarmingArea = 1;
        ShooterEfficiency = 1f;
        ShooterCapacity = 1;
        CurrentDifficulty = DifficultyLevel.Normal;
    }

    // Métodos para gestionar la progresión
    public bool CanApplyNextUpgrade()
    {
        return currentUpgradeIndex < UpgradeList.Count;
    }

    public UpgradeData GetNextUpgrade()
    {
        if (CanApplyNextUpgrade())
            return UpgradeList[currentUpgradeIndex];
        return null;
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
            return;

        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.FarmingEfficiency:
                FarmingEfficiency += upgrade.value;
                break;
            case UpgradeData.UpgradeType.FarmingExpansion:
                FarmingArea += (int)upgrade.value;
                break;
            case UpgradeData.UpgradeType.ShooterEfficiency:
                ShooterEfficiency += upgrade.value;
                break;
            case UpgradeData.UpgradeType.ShooterExpansion:
                ShooterCapacity += (int)upgrade.value;
                break;
        }

        OnProgressionUpdated?.Invoke(upgrade);
        currentUpgradeIndex++;
    }

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(float farmingEfficiency, int farmingArea, float shooterEfficiency, int shooterCapacity, DifficultyLevel difficulty)
    {
        FarmingEfficiency = farmingEfficiency;
        FarmingArea = farmingArea;
        ShooterEfficiency = shooterEfficiency;
        ShooterCapacity = shooterCapacity;
        CurrentDifficulty = difficulty;

        // Opcional: Notificar cambios
    }
}
