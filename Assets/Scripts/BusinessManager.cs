using UnityEngine;
using System.Collections.Generic;
using System;

public class BusinessManager : MonoBehaviour
{
    [SerializeField] private List<BusinessType> availableBusinessTypes;
    [SerializeField] private List<ManagerType> availableManagerTypes;
    [SerializeField] private Transform businessParent;
    [SerializeField] private SimpleCurrency simpleCurrency;

    private List<Business> activeBusinesses = new List<Business>();
    private List<Manager> hiredManagers = new List<Manager>();

    public event Action<double> OnMoneyChanged;

    private void Start()
    {
        simpleCurrency.AddCurrency(0); 
        simpleCurrency.OnCurrencyChanged += HandleCurrencyChanged;
        OnMoneyChanged?.Invoke(simpleCurrency.GetCurrentAmount());
    }

    private void OnDestroy()
    {
        simpleCurrency.OnCurrencyChanged -= HandleCurrencyChanged;
    }

    private void HandleCurrencyChanged(double amount)
    {
        OnMoneyChanged?.Invoke(amount);
    }

    public bool PurchaseBusiness(BusinessType businessType)
    {
        double cost = businessType.initialCost;
        if (simpleCurrency.GetCurrentAmount() >= cost)
        {
            simpleCurrency.SpendCurrency(cost);
            GameObject businessObject = new GameObject(businessType.businessName);
            businessObject.transform.SetParent(businessParent);

            Business newBusiness = businessObject.AddComponent<Business>();
            newBusiness.Initialize(businessType);

            activeBusinesses.Add(newBusiness);
            newBusiness.OnIncomeGenerated += AddMoney;

            return true;
        }
        return false;
    }

    public bool UpgradeBusiness(Business business)
    {
        float upgradeCost = business.GetUpgradeCost();
        if (simpleCurrency.GetCurrentAmount() >= upgradeCost)
        {
            simpleCurrency.SpendCurrency(upgradeCost);
            business.Upgrade();
            return true;
        }
        return false;
    }

    public bool AutomateBusiness(Business business)
    {
        float automationCost = business.GetBaseIncome() * 10;
        if (simpleCurrency.GetCurrentAmount() >= automationCost && !business.IsAutomated())
        {
            simpleCurrency.SpendCurrency(automationCost);
            business.Automate();
            return true;
        }
        return false;
    }

    public bool HireManager(ManagerType managerType)
    {
        double hireCost = managerType.hireCost;
        if (simpleCurrency.GetCurrentAmount() >= hireCost)
        {
            simpleCurrency.SpendCurrency(hireCost);
            GameObject managerObject = new GameObject(managerType.managerName);
            managerObject.transform.SetParent(transform);

            Manager newManager = managerObject.AddComponent<Manager>();
            newManager.Initialize(managerType);

            hiredManagers.Add(newManager);

            AssignManagerToCompatibleBusiness(newManager);

            return true;
        }
        return false;
    }

    private void AssignManagerToCompatibleBusiness(Manager manager)
    {
        foreach (Business business in activeBusinesses)
        {
            if (manager.CanManageBusiness(business) && !business.HasManager())
            {
                manager.ManageBusiness(business);
                break;
            }
        }
    }

    private void AddMoney(float amount)
    {
        simpleCurrency.AddCurrency(amount);
    }
}
