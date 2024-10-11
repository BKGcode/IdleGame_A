using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SimpleCurrencyUI : MonoBehaviour
{
    [System.Serializable]
    private class IncomeEffect
    {
        public TextMeshProUGUI text;
        public float elapsedTime;
        public Vector2 startPosition;
    }

    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject incomeEffectPrefab;
    [SerializeField] private RectTransform incomeEffectParent;

    [Header("Income Effect Settings")]
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color endColor = new Color(0, 1, 0, 0); // Verde transparente
    [SerializeField] private float movementSpeed = 50f;
    [SerializeField] private float movementDistance = 100f;
    [SerializeField] private AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private Vector2 effectOffset = new Vector2(100f, 0f);

    private double currentAmount;
    private List<IncomeEffect> activeEffects = new List<IncomeEffect>();

    private void Start()
    {
        UpdateCurrencyDisplay(SimpleCurrency.Instance.GetCurrentAmount());
        SimpleCurrency.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
        Business.OnIncomeGenerated += ShowIncomeEffect;
        Debug.Log("SimpleCurrencyUI: Suscrito a eventos de moneda e ingresos");
    }

    private void UpdateCurrencyDisplay(double amount)
    {
        currentAmount = amount;
        currencyText.text = FormatCurrency(currentAmount);
        Debug.Log($"Moneda actualizada: {currentAmount}");
    }

    private void ShowIncomeEffect(double incomeAmount, string businessName)
    {
        Debug.Log($"Mostrando efecto de ingreso: {incomeAmount} de {businessName}");
        GameObject effectObj = Instantiate(incomeEffectPrefab, incomeEffectParent);
        TextMeshProUGUI incomeText = effectObj.GetComponent<TextMeshProUGUI>();
        
        if (incomeText == null)
        {
            Debug.LogError("El prefab de efecto de ingreso no tiene un componente TextMeshProUGUI");
            return;
        }

        incomeText.text = $"+{FormatCurrency(incomeAmount)}";

        RectTransform effectTransform = effectObj.GetComponent<RectTransform>();
        
        // Resetear la escala, rotación y posición del efecto
        effectTransform.localScale = Vector3.one;
        effectTransform.localRotation = Quaternion.identity;
        effectTransform.anchoredPosition = Vector2.zero;

        // Configurar los anclajes para que estén en el centro
        effectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        effectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        effectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Calcular la posición del efecto relativa al texto de la moneda
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            incomeEffectParent, 
            currencyText.rectTransform.position, 
            null, 
            out localPoint
        );

        // Ajustar la posición con el offset configurable
        effectTransform.anchoredPosition = localPoint + effectOffset;

        IncomeEffect effect = new IncomeEffect
        {
            text = incomeText,
            elapsedTime = 0f,
            startPosition = effectTransform.anchoredPosition
        };

        activeEffects.Add(effect);
        Debug.Log($"Efecto de ingreso creado para {businessName}: {incomeText.text}");
    }

    private void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            IncomeEffect effect = activeEffects[i];
            effect.elapsedTime += Time.deltaTime;

            float t = effect.elapsedTime * movementSpeed / movementDistance;

            // Mover hacia arriba
            effect.text.rectTransform.anchoredPosition = effect.startPosition + Vector2.up * Mathf.Min(effect.elapsedTime * movementSpeed, movementDistance);

            // Cambiar color y alpha
            float alpha = alphaCurve.Evaluate(t);
            effect.text.color = Color.Lerp(startColor, endColor, t);
            effect.text.color = new Color(effect.text.color.r, effect.text.color.g, effect.text.color.b, alpha);

            if (t >= 1f)
            {
                Destroy(effect.text.gameObject);
                activeEffects.RemoveAt(i);
                Debug.Log("Efecto de ingreso removido");
            }
        }
    }

    private string FormatCurrency(double amount)
    {
        if (amount >= 1e12) return (amount / 1e12).ToString("0.##") + "T";
        if (amount >= 1e9) return (amount / 1e9).ToString("0.##") + "B";
        if (amount >= 1e6) return (amount / 1e6).ToString("0.##") + "M";
        if (amount >= 1e3) return (amount / 1e3).ToString("0.##") + "K";
        return amount.ToString("0.##");
    }

    private void OnDestroy()
    {
        if (SimpleCurrency.Instance != null)
        {
            SimpleCurrency.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
        }
        Business.OnIncomeGenerated -= ShowIncomeEffect;
        Debug.Log("SimpleCurrencyUI: Desuscrito de eventos de moneda e ingresos");
    }
}