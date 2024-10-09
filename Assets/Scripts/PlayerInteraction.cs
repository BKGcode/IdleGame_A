using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private SimpleCurrency simpleCurrency;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Business"))
        {
            Business business = other.GetComponent<Business>();
            if (business != null && !business.IsActivated())
            {
                TryPurchaseBusiness(business);
            }
        }

        if (other.CompareTag("Manager"))
        {
            Manager manager = other.GetComponent<Manager>();
            if (manager != null && manager.GetManagerType() != null)
            {
                TryHireManager(manager);
            }
        }
    }

    private void TryPurchaseBusiness(Business business)
    {
        float cost = business.GetBaseIncome() * 10;
        if (simpleCurrency.GetCurrentAmount() >= cost)
        {
            simpleCurrency.SpendCurrency(cost);
            business.Activate();
            Debug.Log("Purchased business: " + business.name);
        }
        else
        {
            Debug.Log("Not enough currency to purchase business: " + business.name);
        }
    }

    private void TryHireManager(Manager manager)
    {
        float hireCost = manager.GetHireCost();
        if (simpleCurrency.GetCurrentAmount() >= hireCost)
        {
            simpleCurrency.SpendCurrency(hireCost);
            manager.Initialize(manager.GetManagerType());
            Debug.Log("Hired manager: " + manager.GetManagerName());
        }
        else
        {
            Debug.Log("Not enough currency to hire manager: " + manager.GetManagerName());
        }
    }
}
