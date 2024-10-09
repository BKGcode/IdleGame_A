using UnityEngine;

public class BusinessInteractor : MonoBehaviour
{
    [SerializeField] private SimpleCurrency simpleCurrency;

    public void HireBusiness(Business business)
    {
        double hiringCost = business.GetHiringCost();
        if (simpleCurrency.SpendCurrency(hiringCost))
        {
            business.gameObject.SetActive(true); // Activamos el negocio si se contrata
        }
        else
        {
            Debug.Log("Not enough currency to hire this business.");
        }
    }

    public void HireManager(Manager manager)
    {
        double hiringCost = manager.GetHiringCost();
        if (simpleCurrency.SpendCurrency(hiringCost))
        {
            manager.gameObject.SetActive(true); // Activamos el manager si se contrata
        }
        else
        {
            Debug.Log("Not enough currency to hire this manager.");
        }
    }

    public void UpgradeBusiness(Business business, double upgradeCost)
    {
        if (simpleCurrency.SpendCurrency(upgradeCost))
        {
            business.Upgrade();
        }
    }
}
