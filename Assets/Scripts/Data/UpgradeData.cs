// Assets/Scripts/ScriptableObjects/UpgradeData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public UpgradeType upgradeType;
    public float value; // Valor de la mejora

    public enum UpgradeType
    {
        ShooterEfficiency,  // Mejora en la eficiencia del disparo (e.g., aumento de fireRate)
        ShooterExpansion,   // Mejora en la expansión del disparo (e.g., aumento de magazineSize)
        FarmingEfficiency,  // Mejora en la eficiencia de la granja
        FarmingExpansion    // Mejora en la expansión de la granja
        // Añadir más tipos según las necesidades del juego
    }
}
