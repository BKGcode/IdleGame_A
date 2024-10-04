using UnityEngine;
using System;

[Serializable]
public class Business : MonoBehaviour
{
    [Header("Business Settings")]
    [SerializeField] private string businessName;
    [SerializeField] private float incomeBase = 1f;
    [SerializeField] private float cost = 10f;
    [SerializeField] private float timeToComplete = 5f;
    [SerializeField] private int level = 1;

    [Header("Automation Settings")]
    [SerializeField] private bool isAutomated = false; // Si el negocio está automatizado

    [Header("UI Components")]
    [SerializeField] private TMPro.TextMeshProUGUI incomeText;
    [SerializeField] private UnityEngine.UI.Slider progressSlider;

    private float progress = 0f;
    private float currentIncome;
    private bool isGeneratingIncome = false;

    private void Start()
    {
        UpdateCurrentIncome();
        UpdateIncomeText();
    }

    private void Update()
    {
        // Si el negocio está automatizado, empezamos a generar ingresos automáticamente
        if (isAutomated && !isGeneratingIncome)
        {
            StartGeneratingIncome(); 
        }

        // Generar progreso si el negocio está en proceso de generar ingresos
        if (isGeneratingIncome)
        {
            GenerateIncomeProgress();
        }
    }

    private void UpdateCurrentIncome()
    {
        // Actualizamos el ingreso tomando en cuenta el multiplicador de ingresos
        currentIncome = incomeBase * level * GameManager.Instance.GetIncomeMultiplier();
    }

    private void GenerateIncomeProgress()
    {
        // Actualizamos el progreso del slider
        progress += Time.deltaTime / (timeToComplete * GameManager.Instance.GetCooldownReduction());
        progressSlider.value = progress;

        if (progress >= 1f)
        {
            CollectIncome();
            progress = 0f; // Reiniciamos el progreso después de recolectar ingresos
        }
    }

    public void CollectIncome()
    {
        GameManager.Instance.AddMoney((int)currentIncome); // Añadir ingresos al jugador
        UpdateCurrentIncome(); // Actualizamos el ingreso para el próximo ciclo
    }

    public void BuyBusiness()
    {
        if (GameManager.Instance != null && GameManager.Instance.CanAfford((int)cost))
        {
            GameManager.Instance.SpendMoney((int)cost);
            level++;
            cost *= 1.15f; // Aumentar el costo para el próximo nivel
            UpdateCurrentIncome(); // Actualizamos el ingreso basado en el nuevo nivel
            UpdateIncomeText();
        }
    }

    private void UpdateIncomeText()
    {
        // Mostrar solo la cantidad de ingresos sin texto adicional
        incomeText.text = $"{currentIncome.ToString("F1")}";
    }

    public void AutomateBusiness()
    {
        isAutomated = true; // Automatizar el negocio
        Debug.Log(businessName + " ha sido automatizado.");
    }

    private void StartGeneratingIncome()
    {
        isGeneratingIncome = true;
    }
}
