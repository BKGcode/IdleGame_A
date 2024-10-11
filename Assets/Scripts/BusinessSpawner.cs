using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BusinessSpawner : Spawner
{
    [Header("Business Specific Settings")]
    [SerializeField] private BusinessData businessData;

    protected override void InitializeReferences()
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

        spawnableData = businessData;
    }

    protected override void SpawnObject()
    {
        // Instanciar el negocio en estado "no contratado"
        spawnedObject = Instantiate(spawnablePrefab, transform.position, Quaternion.identity);
        Business businessComponent = spawnedObject.GetComponent<Business>();
        businessComponent.Initialize(businessData, false);

        Debug.Log($"BusinessSpawner iniciado para {businessData.businessName}");
    }

    protected override void SetPopupData(PopupController popupController)
    {
        popupController.SetPopupData(businessData.icon, businessData.businessName, businessData.hiringCost);
    }

    protected override double GetHiringCost()
    {
        return businessData.hiringCost;
    }

    protected override void OnHireButtonClicked()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance es null en OnHireButtonClicked");
            return;
        }

        double currentCurrency = CurrencyManager.Instance.GetCurrentCurrency();
        Debug.Log($"Intentando contratar {businessData.businessName}. Costo: {businessData.hiringCost}, Currency actual: {currentCurrency}");

        if (CurrencyManager.Instance.SpendCurrency(businessData.hiringCost))
        {
            Debug.Log($"Contratación exitosa de {businessData.businessName}. Nuevo saldo: {CurrencyManager.Instance.GetCurrentCurrency()}");
            ClosePopup();
            OnHireObject();
        }
        else
        {
            Debug.Log($"No hay suficiente currency para contratar {businessData.businessName}.");
            ClosePopup();
            ShowWarningPopup("No tienes suficientes Coins para contratar este negocio.");
        }
    }

    protected override void OnHireObject()
    {
        if (isHired)
        {
            Debug.LogWarning("Intento de contratar un negocio ya contratado");
            return;
        }

        isHired = true;

        if (spawnedObject == null)
        {
            Debug.LogError("spawnedObject es null en OnHireObject");
            return;
        }

        Business businessComponent = spawnedObject.GetComponent<Business>();
        if (businessComponent == null)
        {
            Debug.LogError("No se pudo obtener el componente Business del spawnedObject");
            return;
        }

        businessComponent.SetHired(true);

        if (BusinessManagerTracker.Instance == null)
        {
            Debug.LogError("BusinessManagerTracker.Instance es null en OnHireObject");
            return;
        }

        BusinessManagerTracker.Instance.RegisterHiredBusiness(businessComponent);

        PlayHireFXAndSound();

        Debug.Log($"Negocio {businessData.businessName} contratado exitosamente");

        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        // Aquí puedes añadir lógica adicional específica de BusinessSpawner si es necesario
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
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