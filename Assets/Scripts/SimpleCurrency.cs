using UnityEngine;
using System;

public class SimpleCurrency : MonoBehaviour
{
    public static SimpleCurrency Instance { get; private set; } // Singleton instance

    [SerializeField] private double initialAmount = 1000;
    private double currentAmount;

    public event Action<double> OnCurrencyChanged;

    private void Awake()
    {
        // Patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            currentAmount = initialAmount;
            NotifyCurrencyChange();
        }
        else
        {
            Destroy(gameObject);
        }
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
            Debug.Log("No hay suficiente moneda para completar la transacción.");
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
