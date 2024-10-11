using UnityEngine;
using System.Collections;

public class Business : MonoBehaviour
{
    [SerializeField] private BusinessData businessData;
    private bool isHired = false;
    private bool isAutomated = false;
    private double currentIncome;
    private Coroutine incomeGenerationCoroutine;

    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem idleFX;
    [SerializeField] private ParticleSystem hiredFX;

    public delegate void IncomeGeneratedHandler(double amount, string businessName);
    public static event IncomeGeneratedHandler OnIncomeGenerated;

    public delegate void BusinessAutomationChangedHandler(Business business, bool isAutomated);
    public static event BusinessAutomationChangedHandler OnBusinessAutomationChanged;

    public void Initialize(BusinessData data, bool hired)
    {
        if (data == null)
        {
            Debug.LogError("BusinessData es null en Initialize");
            return;
        }

        businessData = data;
        SetHired(hired);
        Debug.Log($"Negocio inicializado: {businessData.businessName}, Contratado: {hired}");
    }

    public void SetHired(bool hired)
    {
        isHired = hired;
        UpdateVisuals();
        
        if (isHired)
        {
            RegisterWithCurrencyManager();
            if (isAutomated)
            {
                StartGeneratingIncome();
            }
        }
        else
        {
            StopGeneratingIncome();
            UnregisterFromCurrencyManager();
        }

        Debug.Log($"Estado de contratación cambiado para {businessData.businessName}: {isHired}");
    }

    private void UpdateVisuals()
    {
        if (isHired)
        {
            if (idleFX != null) idleFX.Stop();
            if (hiredFX != null) hiredFX.Play();
            if (animator != null) animator.SetBool("IsHired", true);
        }
        else
        {
            if (idleFX != null) idleFX.Play();
            if (hiredFX != null) hiredFX.Stop();
            if (animator != null) animator.SetBool("IsHired", false);
        }
    }

    private void RegisterWithCurrencyManager()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.RegisterBusiness(this);
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance es null al intentar registrar el negocio");
        }
    }

    private void UnregisterFromCurrencyManager()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.UnregisterBusiness(this);
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance es null al intentar desregistrar el negocio");
        }
    }

    public void AutomateWithManager(Manager manager)
    {
        if (!isHired)
        {
            Debug.LogWarning("Intento de automatizar un negocio no contratado");
            return;
        }

        isAutomated = true;
        StartGeneratingIncome();
        OnBusinessAutomationChanged?.Invoke(this, true);
        Debug.Log($"Negocio {businessData.businessName} automatizado por manager");
    }

    private void StartGeneratingIncome()
    {
        if (incomeGenerationCoroutine != null)
        {
            StopCoroutine(incomeGenerationCoroutine);
        }
        incomeGenerationCoroutine = StartCoroutine(GenerateIncomeRoutine());
        Debug.Log($"Iniciando generación de ingresos para {businessData.businessName}");
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
        while (isAutomated && isHired)
        {
            yield return new WaitForSeconds(businessData.baseIncomeInterval);
            GenerateIncome();
        }
    }

    private void GenerateIncome()
    {
        if (!isAutomated || !isHired) return;
        
        currentIncome = businessData.baseIncome;
        OnIncomeGenerated?.Invoke(currentIncome, businessData.businessName);
        Debug.Log($"{businessData.businessName} generó {currentIncome} de ingresos");
    }

    public BusinessData GetBusinessData() => businessData;
    public bool IsHired() => isHired;
    public bool IsAutomated() => isAutomated;

    private void OnDisable()
    {
        StopGeneratingIncome();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (businessData != null)
        {
            Gizmos.color = isHired ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
    }
#endif
}