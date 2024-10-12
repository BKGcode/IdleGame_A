using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Floating Text Settings")]
    [SerializeField] private TextMeshPro floatingTextPrefab;
    [SerializeField] private float floatingTextDuration = 1f;
    [SerializeField] private float floatingTextDistance = 50f;
    [SerializeField] private Color incomeColor = Color.green;

    [Header("Currency Settings")]
    [SerializeField] private double initialCurrency = 1000;
    private double currentCurrency;

    private List<Business> activeBusinesses = new List<Business>();

    public delegate void CurrencyChangedHandler(double newAmount);
    public event CurrencyChangedHandler OnCurrencyChanged;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentCurrency = initialCurrency;
            UpdateCurrencyDisplay();
            Debug.Log("CurrencyManager instance created and set to DontDestroyOnLoad.");
        }
        else
        {
            Debug.LogWarning("Multiple instances of CurrencyManager detected. Destroying the new instance.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
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
    }

    public void AddCurrencyWithFloatingText(double amount)
    {
        AddCurrency(amount);
        SpawnFloatingText(amount, Camera.main.WorldToScreenPoint(transform.position));
    }

    public bool SpendCurrency(double amount)
    {
        if (currentCurrency >= amount)
        {
            currentCurrency -= amount;
            UpdateCurrencyDisplay();
            OnCurrencyChanged?.Invoke(currentCurrency);
            return true;
        }
        return false;
    }

    private void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = currentCurrency.ToString("N0");
        }
    }

    public void RegisterBusiness(Business business)
    {
        if (!activeBusinesses.Contains(business))
        {
            activeBusinesses.Add(business);
        }
    }

    public void UnregisterBusiness(Business business)
    {
        activeBusinesses.Remove(business);
    }

    private void AddBusinessIncome(double amount, string businessName)
    {
        AddCurrency(amount);
        SpawnFloatingText(amount, Camera.main.WorldToScreenPoint(transform.position));
    }

    private void SpawnFloatingText(double amount, Vector3 position)
    {
        if (floatingTextPrefab != null && currencyText != null)
        {
            // Convert screen position to world position
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            worldPosition.z = 0; // Ensure it's on the correct layer

            TextMeshPro floatingTextInstance = Instantiate(floatingTextPrefab, worldPosition, Quaternion.identity);
            floatingTextInstance.text = "+" + amount.ToString("N0");
            floatingTextInstance.color = incomeColor;

            StartCoroutine(AnimateFloatingText(floatingTextInstance.transform, floatingTextInstance));
        }
    }

    private IEnumerator AnimateFloatingText(Transform textTransform, TextMeshPro textComponent)
    {
        Vector3 startPosition = textTransform.position;
        Vector3 endPosition = startPosition + Vector3.up * floatingTextDistance;
        float elapsedTime = 0f;
        Color startColor = textComponent.color;

        while (elapsedTime < floatingTextDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / floatingTextDuration;

            // Interpolate between the initial and final positions
            textTransform.position = Vector3.Lerp(startPosition, endPosition, t);

            // Fade out the text
            float alpha = Mathf.Lerp(1f, 0f, Mathf.SmoothStep(0f, 1f, t));
            textComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        textComponent.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        Destroy(textTransform.gameObject);
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
    }
}
