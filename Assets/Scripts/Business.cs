using UnityEngine;
using System;

public class Business : MonoBehaviour
{
    [SerializeField] private BusinessType businessType;
    private int level = 1;
    private float currentIncome;
    private float currentIncomeInterval;
    private float timer;
    private bool isAutomated = false;
    private Manager assignedManager;
    private float efficiencyBonus = 1f;
    private bool isActivated = false;

    public event Action<float> OnIncomeGenerated;

    private void Awake()
    {
        if (businessType == null)
        {
            Debug.LogError("BusinessType not assigned to Business!");
        }
    }

    public void Initialize(BusinessType type)
    {
        businessType = type;
        currentIncome = businessType.baseIncome;
        currentIncomeInterval = businessType.baseIncomeInterval;
    }

    private void Update()
    {
        if (isActivated && (isAutomated || assignedManager != null))
        {
            timer += Time.deltaTime;
            if (timer >= currentIncomeInterval)
            {
                GenerateIncome();
                timer = 0f;
            }
        }
    }

    public void GenerateIncome()
    {
        float generatedIncome = currentIncome * efficiencyBonus;
        OnIncomeGenerated?.Invoke(generatedIncome);
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void Upgrade()
    {
        level++;
        currentIncome = businessType.baseIncome * Mathf.Pow(1.1f, level - 1);
        currentIncomeInterval = businessType.baseIncomeInterval * Mathf.Pow(0.95f, level - 1);
    }

    public void Automate()
    {
        isAutomated = true;
    }

    public void SetManager(Manager manager)
    {
        assignedManager = manager;
        isAutomated = true;
    }

    public void ApplyEfficiencyBonus(float bonus)
    {
        efficiencyBonus = bonus;
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    public bool IsAutomated()
    {
        return isAutomated;
    }

    public bool HasManager()
    {
        return assignedManager != null;
    }

    public float GetBaseIncome() => businessType.baseIncome;

    public float GetUpgradeCost()
    {
        return businessType.initialCost * Mathf.Pow(1.5f, level); // Ejemplo de cálculo de costo de mejora
    }

    public BusinessType GetBusinessType()
    {
        return businessType;
    }
}
