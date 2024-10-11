using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManagerSpawner : Spawner
{
    [Header("Manager Specific Settings")]
    [SerializeField] private ManagerData managerData;

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

        spawnableData = managerData;
    }

    protected override void SpawnObject()
    {
        // Instanciar el manager en estado "no contratado"
        spawnedObject = Instantiate(spawnablePrefab, transform.position, Quaternion.identity);
        Manager managerComponent = spawnedObject.GetComponent<Manager>();
        managerComponent.Initialize(managerData, false);

        Debug.Log($"ManagerSpawner iniciado para {managerData.managerName}");
    }

    protected override void SetPopupData(PopupController popupController)
    {
        popupController.SetPopupData(managerData.managerIcon, managerData.managerName, managerData.hiringCost);
    }

    protected override double GetHiringCost()
    {
        return managerData.hiringCost;
    }

    protected override void OnHireButtonClicked()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance es null en OnHireButtonClicked");
            return;
        }

        double currentCurrency = CurrencyManager.Instance.GetCurrentCurrency();
        Debug.Log($"Intentando contratar {managerData.managerName}. Costo: {managerData.hiringCost}, Currency actual: {currentCurrency}");

        if (CurrencyManager.Instance.SpendCurrency(managerData.hiringCost))
        {
            Debug.Log($"Contratación exitosa de {managerData.managerName}. Nuevo saldo: {CurrencyManager.Instance.GetCurrentCurrency()}");
            ClosePopup();
            OnHireObject();
        }
        else
        {
            Debug.Log($"No hay suficiente currency para contratar {managerData.managerName}.");
            ClosePopup();
            ShowWarningPopup("No tienes suficientes Coins para contratar este manager.");
        }
    }

    protected override void OnHireObject()
    {
        if (isHired)
        {
            Debug.LogWarning("Intento de contratar un manager ya contratado");
            return;
        }

        isHired = true;

        if (spawnedObject == null)
        {
            Debug.LogError("spawnedObject es null en OnHireObject");
            return;
        }

        Manager managerComponent = spawnedObject.GetComponent<Manager>();
        if (managerComponent == null)
        {
            Debug.LogError("No se pudo obtener el componente Manager del spawnedObject");
            return;
        }

        managerComponent.SetHired(true);

        // Si existe un tracker para managers, podrías registrarlo aquí
        // if (ManagerTracker.Instance != null)
        // {
        //     ManagerTracker.Instance.RegisterHiredManager(managerComponent);
        // }
        // else
        // {
        //     Debug.LogWarning("ManagerTracker.Instance es null. No se pudo registrar el manager contratado.");
        // }

        PlayHireFXAndSound();

        Debug.Log($"Manager {managerData.managerName} contratado exitosamente");

        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update();
        // Aquí puedes añadir lógica adicional específica de ManagerSpawner si es necesario
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // Aquí puedes añadir lógica de limpieza específica de ManagerSpawner si es necesario
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
#endif
}