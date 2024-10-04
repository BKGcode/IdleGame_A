using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName; // Nombre de la mejora
    public int baseCost; // Costo inicial de la mejora
    public float effectAmount; // Cantidad de efecto que tiene la mejora (por ejemplo, +10% de ingresos)
    public float cooldownTime; // Tiempo de espera para volver a usar la mejora
    public int maxUses; // Número máximo de veces que se puede usar la mejora
    public UpgradeType upgradeType; // Tipo de la mejora (por ejemplo, IncomeMultiplier)
}

public enum UpgradeType
{
    IncomeMultiplier,
    TimeReduction,
    DamageIncrease,
    CooldownReduction
}
