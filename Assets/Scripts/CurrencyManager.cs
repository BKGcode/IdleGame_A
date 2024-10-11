using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private double initialCurrency = 1000;

    [Header("Floating Text Settings")]
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private float floatingTextDuration = 1f;
    [SerializeField] private float floatingTextDistance = 50f;
    [SerializeField] private Color incomeColor = Color.green;

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
        SpawnFloatingText(amount);
    }

    private void SpawnFloatingText(double amount)
    {
        if (floatingTextPrefab != null && currencyText != null)
        {
            GameObject floatingTextObj = Instantiate(floatingTextPrefab, currencyText.transform.position, Quaternion.identity, currencyText.transform);
            TextMeshProUGUI floatingText = floatingTextObj.GetComponent<TextMeshProUGUI>();
            
            if (floatingText != null)
            {
                floatingText.text = "+" + amount.ToString("N0");
                floatingText.color = incomeColor;

                StartCoroutine(AnimateFloatingText(floatingTextObj.transform, floatingText));
            }
        }
    }

    private IEnumerator AnimateFloatingText(Transform textTransform, TextMeshProUGUI textComponent)
    {
        Vector3 startPosition = textTransform.localPosition;
        Vector3 endPosition = startPosition + Vector3.up * floatingTextDistance;
        float elapsedTime = 0f;
        Color startColor = textComponent.color;

        while (elapsedTime < floatingTextDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / floatingTextDuration;

            textTransform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            
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