using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManagerSpawner : MonoBehaviour
{
    [Header("Manager Prefab and Data")]
    [SerializeField] private GameObject managerPrefab;
    [SerializeField] private ManagerData managerData;

    [Header("UI Settings")]
    private GameObject popupPrefab;
    private GameObject warningPopupPrefab;
    private Canvas uiCanvas;

    [Header("Proximity Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float popupCooldown = 2f;

    [Header("FX and Sound")]
    private GameObject hireFXPrefab;
    private AudioClip hireSoundClip;
    private AudioSource audioSource;

    private bool isHired = false;
    private GameObject spawnedManager;
    private GameObject popupInstance;
    private bool isPopupActive = false;
    private bool isInCooldown = false;
    private Coroutine cooldownCoroutine;

    private void Start()
    {
        // Obtener referencias del GameManager
        if (GameManager.Instance != null)
        {
            uiCanvas = GameManager.Instance.uiCanvas;
            popupPrefab = GameManager.Instance.popupPrefab;
            warningPopupPrefab = GameManager.Instance.warningPopupPrefab;
            hireFXPrefab = GameManager.Instance.hireFXPrefab;
            hireSoundClip = GameManager.Instance.hireSoundClip;
            audioSource = GameManager.Instance.audioSource;
        }
        else
        {
            Debug.LogError("No se encontró el GameManager en la escena.");
        }

        // Validar que las referencias están asignadas
        if (uiCanvas == null || popupPrefab == null || warningPopupPrefab == null)
        {
            Debug.LogError("Asignar las referencias necesarias en el GameManager.");
        }

        // Instanciar el manager en estado "no contratado"
        spawnedManager = Instantiate(managerPrefab, transform.position, Quaternion.identity);
        Manager managerComponent = spawnedManager.GetComponent<Manager>();
        managerComponent.Initialize(managerData, false);

        Debug.Log($"ManagerSpawner iniciado para {managerData.managerName}");

#if UNITY_EDITOR
        UnityEditor.SceneView.duringSceneGui += OnSceneGUI;
#endif
    }

    private void Update()
    {
        if (isPopupActive || isInCooldown || isHired) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        if (hits.Length > 0)
        {
            ShowPopup();
        }
    }

    private void ShowPopup()
    {
        isPopupActive = true;
        Time.timeScale = 0f;

        popupInstance = Instantiate(popupPrefab, uiCanvas.transform);
        PopupController popupController = popupInstance.GetComponent<PopupController>();

        popupController.SetPopupData(managerData.managerSprite, managerData.managerName, managerData.baseCost);
        popupController.OnHireButtonClicked += OnHireButtonClicked_Internal;
        popupController.OnCloseButtonClicked += ClosePopup;

        Debug.Log($"Mostrando popup para contratar {managerData.managerName}");
    }

    private void OnHireButtonClicked_Internal()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance es null en OnHireButtonClicked_Internal");
            return;
        }

        if (managerData == null)
        {
            Debug.LogError("managerData es null en OnHireButtonClicked_Internal");
            return;
        }

        double currentCurrency = CurrencyManager.Instance.GetCurrentCurrency();
        Debug.Log($"Intentando contratar {managerData.managerName}. Costo: {managerData.baseCost}, Currency actual: {currentCurrency}");

        if (CurrencyManager.Instance.SpendCurrency(managerData.baseCost))
        {
            Debug.Log($"Contratación exitosa de {managerData.managerName}. Nuevo saldo: {CurrencyManager.Instance.GetCurrentCurrency()}");
            ClosePopup();
            OnHireManager();
        }
        else
        {
            Debug.Log($"No hay suficiente currency para contratar {managerData.managerName}.");
            ClosePopup();
            ShowWarningPopup("No tienes suficientes Coins para contratar este manager.");
        }
    }

    private void ShowWarningPopup(string message)
    {
        isPopupActive = true;
        Time.timeScale = 0f;

        popupInstance = Instantiate(warningPopupPrefab, uiCanvas.transform);
        WarningPopupController warningPopupController = popupInstance.GetComponent<WarningPopupController>();

        warningPopupController.SetWarningMessage(message);
        warningPopupController.OnCloseButtonClicked += ClosePopup;

        Debug.Log($"Mostrando advertencia: {message}");
    }

    private void ClosePopup()
    {
        if (popupInstance != null)
        {
            Destroy(popupInstance);
            popupInstance = null;
            isPopupActive = false;

            Time.timeScale = 1f;

            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
            }
            cooldownCoroutine = StartCoroutine(PopupCooldownCoroutine());

            Debug.Log("Popup cerrado");
        }
    }

    private IEnumerator PopupCooldownCoroutine()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(popupCooldown);
        isInCooldown = false;
    }

    private void OnHireManager()
    {
        if (isHired)
        {
            Debug.LogWarning("Intento de contratar un manager ya contratado");
            return;
        }

        isHired = true;

        if (spawnedManager == null)
        {
            Debug.LogError("spawnedManager es null en OnHireManager");
            return;
        }

        Manager managerComponent = spawnedManager.GetComponent<Manager>();
        if (managerComponent == null)
        {
            Debug.LogError("No se pudo obtener el componente Manager del spawnedManager");
            return;
        }

        managerComponent.SetHired(true);

        if (BusinessManagerTracker.Instance == null)
        {
            Debug.LogError("BusinessManagerTracker.Instance es null en OnHireManager");
            return;
        }

        BusinessManagerTracker.Instance.RegisterHiredManager(managerComponent);

        // Automatizar los negocios correspondientes
        AutomateBusinesses(managerComponent);

        if (hireFXPrefab != null)
        {
            Instantiate(hireFXPrefab, transform.position, Quaternion.identity);
        }

        if (audioSource != null && hireSoundClip != null)
        {
            audioSource.PlayOneShot(hireSoundClip);
        }

        Debug.Log($"Manager {managerData.managerName} contratado exitosamente");

        Destroy(gameObject);
    }

    private void AutomateBusinesses(Manager manager)
    {
        BusinessData businessToManage = manager.GetManagerData().businessToManage;
        if (businessToManage == null)
        {
            Debug.LogWarning($"El manager {manager.GetManagerData().managerName} no tiene un negocio específico para administrar.");
            return;
        }

        Business[] allBusinesses = FindObjectsOfType<Business>();
        bool businessAutomated = false;
        foreach (Business business in allBusinesses)
        {
            if (business.GetBusinessData() == businessToManage)
            {
                business.AutomateWithManager(manager);
                Debug.Log($"Negocio {business.GetBusinessData().businessName} automatizado por {manager.GetManagerData().managerName}");
                businessAutomated = true;
            }
        }

        if (!businessAutomated)
        {
            Debug.LogWarning($"No se encontró ningún negocio '{businessToManage.businessName}' para que el manager {manager.GetManagerData().managerName} automatice.");
        }
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        UnityEditor.SceneView.duringSceneGui -= OnSceneGUI;
#endif
    }

#if UNITY_EDITOR
    private void OnSceneGUI(UnityEditor.SceneView sceneView)
    {
        if (this == null) return;

        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, detectionRadius);
    }
#endif
}