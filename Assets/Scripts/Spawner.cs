using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Spawner : MonoBehaviour
{
    [Header("Prefab and Data")]
    [SerializeField] protected GameObject spawnablePrefab;
    [SerializeField] protected ScriptableObject spawnableData;

    [Header("UI Settings")]
    protected GameObject popupPrefab;
    protected GameObject warningPopupPrefab;
    protected Canvas uiCanvas;

    [Header("Proximity Settings")]
    [SerializeField] protected float detectionRadius = 5f;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float popupCooldown = 2f;

    [Header("FX and Sound")]
    protected GameObject hireFXPrefab;
    protected AudioClip hireSoundClip;
    protected AudioSource audioSource;

    protected bool isHired = false;
    protected GameObject spawnedObject;
    protected GameObject popupInstance;
    protected bool isPopupActive = false;
    protected bool isInCooldown = false;
    protected Coroutine cooldownCoroutine;

    protected virtual void Start()
    {
        InitializeReferences();
        SpawnObject();
    }

    protected virtual void Update()
    {
        if (isPopupActive || isInCooldown || isHired) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (hits.Length > 0)
        {
            ShowPopup();
        }
    }

    protected abstract void InitializeReferences();
    protected abstract void SpawnObject();
    protected abstract void SetPopupData(PopupController popupController);
    protected abstract double GetHiringCost();
    protected abstract void OnHireObject();

    protected virtual void ShowPopup()
    {
        isPopupActive = true;
        GameManager.Instance.SetTimeScale(0f);

        popupInstance = Instantiate(popupPrefab, uiCanvas.transform);
        PopupController popupController = popupInstance.GetComponent<PopupController>();

        SetPopupData(popupController);
        popupController.OnHireButtonClicked += OnHireButtonClicked;
        popupController.OnCloseButtonClicked += ClosePopup;

        Debug.Log($"Mostrando popup para contratar {spawnableData}");
    }

    protected virtual void OnHireButtonClicked()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance es null en OnHireButtonClicked");
            return;
        }

        double hiringCost = GetHiringCost();
        double currentCurrency = CurrencyManager.Instance.GetCurrentCurrency();
        
        if (CurrencyManager.Instance.SpendCurrency(hiringCost))
        {
            Debug.Log($"Contratación exitosa. Nuevo saldo: {CurrencyManager.Instance.GetCurrentCurrency()}");
            ClosePopup();
            OnHireObject();
        }
        else
        {
            Debug.Log($"No hay suficiente currency para contratar.");
            ClosePopup();
            ShowWarningPopup("No tienes suficientes Coins para contratar.");
        }
    }

    protected virtual void ShowWarningPopup(string message)
    {
        isPopupActive = true;
        GameManager.Instance.SetTimeScale(0f);

        popupInstance = Instantiate(warningPopupPrefab, uiCanvas.transform);
        WarningPopupController warningPopupController = popupInstance.GetComponent<WarningPopupController>();

        warningPopupController.SetWarningMessage(message);
        warningPopupController.OnCloseButtonClicked += ClosePopup;

        Debug.Log($"Mostrando advertencia: {message}");
    }

    protected virtual void ClosePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
            isPopupActive = false;

            GameManager.Instance.SetTimeScale(1f);

            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
            }
            cooldownCoroutine = StartCoroutine(PopupCooldownCoroutine());

            Debug.Log("Popup cerrado");
        }
    }

    protected virtual IEnumerator PopupCooldownCoroutine()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(popupCooldown);
        isInCooldown = false;
    }

    protected virtual void PlayHireFXAndSound()
    {
        GameManager.Instance.SpawnHireFX(transform.position);
        GameManager.Instance.PlayHireSound();
    }

    protected virtual void OnDestroy()
    {
        // Implementar lógica de limpieza si es necesario
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}