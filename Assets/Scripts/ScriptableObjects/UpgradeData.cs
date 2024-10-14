// Assets/Scripts/ScriptableObjects/UpgradeData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public Sprite upgradeIcon;

    [Header("Tipo de Mejora")]
    public UpgradeType upgradeType;

    [Header("Efecto de la Mejora")]
    public float effectValue; // Valor del efecto (por ejemplo, aumento de velocidad)
    public string description;

    [Header("Costo de la Mejora")]
    public int cost;

    public enum UpgradeType
    {
        FarmingEfficiency,
        FarmingExpansion,
        ShooterEfficiency,
        ShooterExpansion
    }
}
