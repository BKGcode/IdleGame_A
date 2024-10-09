using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private SimpleCurrency simpleCurrency;
    [SerializeField] private BusinessInteractor businessInteractor;

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si colisiona con un objeto con tag "Business"
        if (other.CompareTag("Business"))
        {
            Business business = other.GetComponent<Business>();
            if (business != null)
            {
                // Intentar contratar el negocio
                HireBusiness(business);
            }
        }

        // Comprobar si colisiona con un objeto con tag "Manager"
        if (other.CompareTag("Manager"))
        {
            Manager manager = other.GetComponent<Manager>();
            if (manager != null)
            {
                // Intentar contratar el manager
                HireManager(manager);
            }
        }
    }

    private void HireBusiness(Business business)
    {
        double hiringCost = business.GetHiringCost();
        if (simpleCurrency.SpendCurrency(hiringCost))
        {
            business.gameObject.SetActive(true);
            Debug.Log("Business hired: " + business.BusinessType.businessName);
        }
        else
        {
            Debug.Log("Not enough currency to hire this business.");
        }
    }

    private void HireManager(Manager manager)
    {
        double hiringCost = manager.GetHiringCost();
        if (simpleCurrency.SpendCurrency(hiringCost))
        {
            manager.gameObject.SetActive(true);
            Debug.Log("Manager hired: " + manager.ManagerType.managerName);
        }
        else
        {
            Debug.Log("Not enough currency to hire this manager.");
        }
    }
}
