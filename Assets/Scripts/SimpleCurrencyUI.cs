using UnityEngine;
using TMPro; // Importar el namespace de TextMeshPro

public class SimpleCurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText; // Cambiado a TextMeshProUGUI

    private void Start()
    {
        UpdateCurrencyDisplay(SimpleCurrency.Instance.GetCurrentAmount());
        SimpleCurrency.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
    }

    private void UpdateCurrencyDisplay(double amount)
    {
        currencyText.text = FormatCurrency(amount);
    }

    private string FormatCurrency(double amount)
    {
        if (amount >= 1e12) return (amount / 1e12).ToString("0.##") + "T";
        if (amount >= 1e9) return (amount / 1e9).ToString("0.##") + "B";
        if (amount >= 1e6) return (amount / 1e6).ToString("0.##") + "M";
        if (amount >= 1e3) return (amount / 1e3).ToString("0.##") + "K";
        return amount.ToString("0");
    }

    private void OnDestroy()
    {
        SimpleCurrency.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
    }
}
