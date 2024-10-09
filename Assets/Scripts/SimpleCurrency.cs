using UnityEngine;
using System;

public class SimpleCurrency : MonoBehaviour
{
    [SerializeField] private string currencyName = "Coins";
    [SerializeField] private double initialAmount = 1000;
    private double currentAmount;

    public event Action<double> OnCurrencyChanged;

    private void Awake()
    {
        currentAmount = initialAmount;
        NotifyCurrencyChange();
    }

    public void AddCurrency(double amount)
    {
        currentAmount += amount;
        NotifyCurrencyChange();
    }

    public bool SpendCurrency(double amount)
    {
        if (currentAmount >= amount)
        {
            currentAmount -= amount;
            NotifyCurrencyChange();
            return true;
        }
        else
        {
            Debug.Log("Not enough currency to complete the transaction.");
            return false;
        }
    }

    public double GetCurrentAmount()
    {
        return currentAmount;
    }

    private void NotifyCurrencyChange()
    {
        OnCurrencyChanged?.Invoke(currentAmount);
    }
}
