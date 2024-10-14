// Assets/Scripts/Data/ProgressionData.cs
using System;
using UnityEngine;

[Serializable]
public class ProgressionData
{
    // Eventos para notificar cambios en la progresión
    public event Action<UpgradeData> OnProgressionUpdated;

    // Propiedades de progresión
    public float FarmingEfficiency { get; private set; }
    public int FarmingArea { get; private set; }
    public float ShooterEfficiency { get; private set; }
    public int ShooterCapacity { get; private set; }
    public DifficultyLevel CurrentDifficulty { get; private set; }

    // Enum para los niveles de dificultad
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard,
        Insane
    }

    // Constructor inicial
    public ProgressionData()
    {
        FarmingEfficiency = 1f;
        FarmingArea = 5;
        ShooterEfficiency = 1f;
        ShooterCapacity = 10;
        CurrentDifficulty = DifficultyLevel.Normal;
    }

    // Métodos para gestionar la progresión
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeData.UpgradeType.FarmingEfficiency:
                FarmingEfficiency += upgrade.effectValue;
                break;
            case UpgradeData.UpgradeType.FarmingExpansion:
                FarmingArea += (int)upgrade.effectValue;
                break;
            case UpgradeData.UpgradeType.ShooterEfficiency:
                ShooterEfficiency += upgrade.effectValue;
                break;
            case UpgradeData.UpgradeType.ShooterExpansion:
                ShooterCapacity += (int)upgrade.effectValue;
                break;
        }

        OnProgressionUpdated?.Invoke(upgrade);
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        CurrentDifficulty = difficulty;
        // Puedes añadir un evento para notificar el cambio de dificultad si es necesario
    }

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(float farmingEff, int farmingArea, float shooterEff, int shooterCap, DifficultyLevel difficulty)
    {
        FarmingEfficiency = farmingEff;
        FarmingArea = farmingArea;
        ShooterEfficiency = shooterEff;
        ShooterCapacity = shooterCap;
        CurrentDifficulty = difficulty;

        // Puedes notificar sobre todas las actualizaciones si es necesario
    }
}
