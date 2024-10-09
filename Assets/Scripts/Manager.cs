using UnityEngine;
using System.Collections.Generic;

public class Manager : MonoBehaviour
{
    [SerializeField] private ManagerType managerType;
    private List<Business> managedBusinesses = new List<Business>();

    public void Initialize(ManagerType type)
    {
        managerType = type;
    }

    public bool CanManageBusiness(Business business)
    {
        if (managerType == null) return false;
        return managerType.managedBusinessTypes.Contains(business.GetBusinessType());
    }

    public void ManageBusiness(Business business)
    {
        if (managerType == null)
        {
            Debug.LogError("Cannot manage business: ManagerType not set.");
            return;
        }

        if (CanManageBusiness(business) && !managedBusinesses.Contains(business))
        {
            managedBusinesses.Add(business);
            business.SetManager(this);
            ApplyEfficiencyBonus(business);
        }
    }

    private void ApplyEfficiencyBonus(Business business)
    {
        if (managerType == null)
        {
            Debug.LogError("Cannot apply efficiency bonus: ManagerType not set.");
            return;
        }
        business.ApplyEfficiencyBonus(managerType.efficiencyBonus);
    }

    public float GetHireCost() => managerType != null ? managerType.hireCost : 0f;

    public string GetManagerName() => managerType != null ? managerType.managerName : "Unknown Manager";

    public ManagerType GetManagerType()
    {
        return managerType;
    }
}
