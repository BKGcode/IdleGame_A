// Assets/Scripts/ScriptableObjects/UpgradeData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public enum UpgradeType
    {
        FarmingEfficiency,
        FarmingExpansion,
        ShooterEfficiency,
        ShooterExpansion
    }

    [Header("Información de la Mejora")]
    public string upgradeName;
    public UpgradeType upgradeType;
    public string description;

    [Header("Efectos de la Mejora")]
    public float value; // Valor del efecto, por ejemplo, porcentaje de aumento

    // Puedes añadir más campos según las necesidades de tus mejoras
}
