using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private ManagerType managerType; // Tipo de manager
    public ManagerType ManagerType => managerType; // Propiedad pública para acceder al tipo de manager

    private Business assignedBusiness;
    private float appliedBonus;

    public double GetHiringCost()
    {
        return managerType.hiringCost;
    }

    public void AssignBusiness(Business business)
    {
        // Solo asigna si el tipo de negocio coincide con el tipo que el manager puede manejar
        if (business.BusinessType == managerType.businessType)
        {
            assignedBusiness = business;
            ApplyManagerBonus();
            business.Automate();
        }
        else
        {
            Debug.LogWarning("This manager can only automate businesses of type: " + managerType.businessType.businessName);
        }
    }

    private void ApplyManagerBonus()
    {
        if (assignedBusiness != null)
        {
            assignedBusiness.ApplyEfficiencyBonus(managerType.bonusAmount);
        }
    }
}
