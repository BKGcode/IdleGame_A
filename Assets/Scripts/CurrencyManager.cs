using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private double initialCurrency = 1000;

    private double currentCurrency;
    private List<Business> activeBusinesses = new List<Business>();

    public delegate void CurrencyChangedHandler(double newAmount);
    public static event CurrencyChangedHandler OnCurrencyChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentCurrency = initialCurrency;
    }

    private void Start()
    {
        UpdateCurrencyDisplay();
        Business.OnIncomeGenerated += AddBusinessIncome;
    }

    private void OnDestroy()
    {
        Business.OnIncomeGenerated -= AddBusinessIncome;
    }

    public void AddCurrency(double amount)
    {
        currentCurrency += amount;
        UpdateCurrencyDisplay();
        OnCurrencyChanged?.Invoke(currentCurrency);
        Debug.Log($"Moneda añadida: {amount}. Nueva cantidad total: {currentCurrency}");
    }

    public bool SpendCurrency(double amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            UpdateCurrencyDisplay();
            OnCurrencyChanged?.Invoke(currentCurrency);
            Debug.Log($"Moneda gastada: {amount}. Nueva cantidad total: {currentCurrency}");
            return true;
        }
        Debug.Log($"No se pudo gastar {amount}. Moneda actual: {currentCurrency}");
        return false;
    }

    private void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = $"${currentCurrency:N0}";
            Debug.Log($"Actualizando display de moneda: {currencyText.text}");
        }
        else
        {
            Debug.LogError("CurrencyText no está asignado en CurrencyManager");
        }
    }

    public void RegisterBusiness(Business business)
    {
        if (!activeBusinesses.Contains(business))
        {
            activeBusinesses.Add(business);
            Debug.Log($"Negocio registrado: {business.GetBusinessData().businessName}");
        }
    }

    public void UnregisterBusiness(Business business)
    {
        activeBusinesses.Remove(business);
        Debug.Log($"Negocio desregistrado: {business.GetBusinessData().businessName}");
    }

    private void AddBusinessIncome(double amount, string businessName)
    {
        AddCurrency(amount);
        Debug.Log($"Ingreso generado por {businessName}: {amount}");
    }

    public double GetCurrentCurrency()
    {
        return currentCurrency;
    }

    public void ResetCurrency()
    {
        currentCurrency = initialCurrency;
        UpdateCurrencyDisplay();
        OnCurrencyChanged?.Invoke(currentCurrency);
        Debug.Log($"Moneda reseteada a: {currentCurrency}");
    }
}