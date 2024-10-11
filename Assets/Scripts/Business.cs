using UnityEngine;
using System.Collections;

public class Business : MonoBehaviour
{
    [SerializeField] private BusinessData businessData;
    private bool isHired = false;
    private bool isAutomated = false;
    private double currentIncome;
    private Coroutine incomeGenerationCoroutine;
    private float efficiencyBonus = 1f;

    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem idleFX;
    [SerializeField] private ParticleSystem hiredFX;

    public delegate void IncomeGeneratedHandler(double amount, string businessName);
    public static event IncomeGeneratedHandler OnIncomeGenerated;

    public delegate void BusinessAutomationChangedHandler(Business business, bool isAutomated);
    public static event BusinessAutomationChangedHandler OnBusinessAutomationChanged;

    public void Initialize(BusinessData data, bool hired)
    {
        businessData = data;
        SetHired(hired);
    }

    public void SetHired(bool hired)
    {
        isHired = hired;
        Debug.Log($"Negocio {businessData.businessName} contratado: {isHired}");
        if (isHired)
        {
            if (idleFX != null) idleFX.Stop();
            if (hiredFX != null) hiredFX.Play();
            if (animator != null) animator.SetBool("IsHired", true);
            CurrencyManager.Instance.RegisterBusiness(this);
            if (isAutomated)
            {
                StartGeneratingIncome();
            }
        }
        else
        {
            if (idleFX != null) idleFX.Play();
            if (hiredFX != null) hiredFX.Stop();
            if (animator != null) animator.SetBool("IsHired", false);
            StopGeneratingIncome();
            CurrencyManager.Instance.UnregisterBusiness(this);
        }
        DebugStatus();
    }

    public void AutomateWithManager(Manager manager)
    {
        if (!isHired) 
        {
            Debug.LogWarning($"Intento de automatizar {businessData.businessName} pero no está contratado.");
            return;
        }
        isAutomated = true;
        efficiencyBonus = 1f + manager.GetManagerData().efficiencyBonus;
        Debug.Log($"El manager {manager.GetManagerData().managerName} está automatizando el negocio {businessData.businessName}. Nuevo efficiency bonus: {efficiencyBonus}");
        StartGeneratingIncome();
        OnBusinessAutomationChanged?.Invoke(this, true);
        DebugStatus();
    }

    private void StartGeneratingIncome()
    {
        Debug.Log($"Iniciando generación de ingresos para {businessData.businessName}");
        if (incomeGenerationCoroutine != null)
        {
            StopCoroutine(incomeGenerationCoroutine);
        }
        incomeGenerationCoroutine = StartCoroutine(GenerateIncomeRoutine());
    }

    private void StopGeneratingIncome()
    {
        if (incomeGenerationCoroutine != null)
        {
            StopCoroutine(incomeGenerationCoroutine);
            incomeGenerationCoroutine = null;
        }
        isAutomated = false;
        OnBusinessAutomationChanged?.Invoke(this, false);
        Debug.Log($"Deteniendo generación de ingresos para {businessData.businessName}");
    }

    private IEnumerator GenerateIncomeRoutine()
    {
        Debug.Log($"Iniciando rutina de generación de ingresos para {businessData.businessName}");
        while (isAutomated && isHired)
        {
            Debug.Log($"Esperando {businessData.baseIncomeInterval} segundos para generar ingreso de {businessData.businessName}");
            yield return new WaitForSeconds(businessData.baseIncomeInterval);
            GenerateIncome();
        }
    }

    private void GenerateIncome()
    {
        if (!isAutomated || !isHired) 
        {
            Debug.LogWarning($"Intento de generar ingreso para {businessData.businessName} pero no está automatizado o contratado.");
            return;
        }
        
        currentIncome = businessData.baseIncome * efficiencyBonus;
        OnIncomeGenerated?.Invoke(currentIncome, businessData.businessName);
        Debug.Log($"Generando ingreso para {businessData.businessName}: {currentIncome}");
    }

    public void RemoveManager()
    {
        StopGeneratingIncome();
        efficiencyBonus = 1f;
        Debug.Log($"Manager removido del negocio {businessData.businessName}. Generación de ingresos detenida.");
    }

    public BusinessData GetBusinessData() => businessData;
    public bool IsHired() => isHired;
    public bool IsAutomated() => isAutomated;
    public float GetEfficiencyBonus() => efficiencyBonus;

    public void DebugStatus()
    {
        Debug.Log($"Estado del negocio {businessData.businessName}: Contratado: {isHired}, Automatizado: {isAutomated}, Eficiencia: {efficiencyBonus}, Intervalo de ingreso: {businessData.baseIncomeInterval}");
    }
}