using UnityEngine;

[CreateAssetMenu(fileName = "New Business Type", menuName = "Business Game/Business Type")]
public class BusinessType : ScriptableObject
{
    public string businessName;
    public float initialCost;
    public float baseIncome;
    public float baseIncomeInterval;
    public Sprite icon;
}