using UnityEngine;
using System;

public class Business : MonoBehaviour, IAutomatable
{
    [SerializeField] private BusinessType businessType; // Tipo de negocio
    public BusinessType BusinessType => businessType; // Propiedad pública para acceder al tipo de negocio

    private float currentIncome;
    private float currentIncomeInterval;
    private int level = 1; // Nivel del negocio
    private bool isAutomated = false;
    private float efficiencyBonus = 1f;

    public event Action<float> OnIncomeGenerated;

    private void Awake()
    {
        Initialize(businessType);
    }

    public void Initialize(BusinessType type)
    {
        businessType = type;
        currentIncome = businessType.baseIncome;
        currentIncomeInterval = businessType.baseIncomeInterval;
    }

    public double GetHiringCost()
    {
        return businessType.hiringCost;
    }

    public void Automate()
    {
        isAutomated = true;
        StartCoroutine(GenerateIncomeCoroutine());
    }

    private System.Collections.IEnumerator GenerateIncomeCoroutine()
    {
        while (isAutomated)
        {
            yield return new WaitForSeconds(currentIncomeInterval);
            GenerateIncome();
        }
    }

    public void GenerateIncome()
    {
        float generatedIncome = currentIncome * efficiencyBonus;
        OnIncomeGenerated?.Invoke(generatedIncome);
    }

    public void Upgrade()
    {
        level++;
        currentIncome = businessType.baseIncome * Mathf.Pow(1.1f, level - 1);
        currentIncomeInterval = businessType.baseIncomeInterval * Mathf.Pow(0.95f, level - 1);
    }

    public void ApplyEfficiencyBonus(float bonus)
    {
        efficiencyBonus = bonus;
    }
}
