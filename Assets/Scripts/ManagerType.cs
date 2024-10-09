using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Manager Type", menuName = "Business Game/Manager Type")]
public class ManagerType : ScriptableObject
{
    public string managerName;
    public float hireCost;
    public Sprite portrait;
    public List<BusinessType> managedBusinessTypes; // Los tipos de negocios que este manager puede gestionar
    public float efficiencyBonus; // Bonus de eficiencia (por ejemplo, 1.1 para un 10% de aumento en la producción)
}