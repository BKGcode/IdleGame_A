using UnityEngine;

[CreateAssetMenu(fileName = "New Manager Data", menuName = "Game Data/Manager Data")]
public class ManagerData : ScriptableObject
{
    public string managerName;
    public float efficiencyBonus;
    public BusinessData businessToManage; // Cambiado de string a BusinessData
    public Sprite managerSprite;
    public float baseCost;
    public float costMultiplier;
    public string description;

    [Header("Upgrade Data")]
    public float[] upgradeCosts;
    public float[] upgradeEfficiencyBonuses;

    public float GetUpgradeCost(int level)
    {
        if (level < 0 || level >= upgradeCosts.Length)
        {
            Debug.LogWarning($"Invalid upgrade level {level} for manager {managerName}");
            return 0f;
        }
        return upgradeCosts[level];
    }

    public float GetUpgradeEfficiencyBonus(int level)
    {
        if (level < 0 || level >= upgradeEfficiencyBonuses.Length)
        {
            Debug.LogWarning($"Invalid upgrade level {level} for manager {managerName}");
            return 0f;
        }
        return upgradeEfficiencyBonuses[level];
    }

    public int GetMaxUpgradeLevel()
    {
        return Mathf.Min(upgradeCosts.Length, upgradeEfficiencyBonuses.Length);
    }

    public float CalculateCost(int quantity)
    {
        return baseCost * Mathf.Pow(costMultiplier, quantity);
    }
}