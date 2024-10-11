using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BusinessSpawner : MonoBehaviour
{
    [Header("Business Prefab and Data")]
    [SerializeField] private GameObject businessPrefab;
    [SerializeField] private BusinessData businessData;

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
    private GameObject spawnedBusiness;
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

        // Instanciar el negocio en estado "no contratado"
        spawnedBusiness = Instantiate(businessPrefab, transform.position, Quaternion.identity);
        Business businessComponent = spawnedBusiness.GetComponent<Business>();
        businessComponent.Initialize(businessData, false);

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

        popupController.SetPopupData(businessData.icon, businessData.businessName, businessData.hiringCost);
        popupController.OnHireButtonClicked += OnHireButtonClicked_Internal;
        popupController.OnCloseButtonClicked += ClosePopup;
    }

    private void OnHireButtonClicked_Internal()
    {
        if (SimpleCurrency.Instance == null)
        {
            Debug.LogError("SimpleCurrency.Instance es null en OnHireButtonClicked_Internal");
            return;
        }

        if (businessData == null)
        {
            Debug.LogError("businessData es null en OnHireButtonClicked_Internal");
            return;
        }

        if (SimpleCurrency.Instance.SpendCurrency(businessData.hiringCost))
        {
            ClosePopup();
            OnHireBusiness();
        }
        else
        {
            ClosePopup();
            ShowWarningPopup("No tienes suficientes Coins para contratar este negocio.");
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
        }
    }

    private IEnumerator PopupCooldownCoroutine()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(popupCooldown);
        isInCooldown = false;
    }

    private void OnHireBusiness()
    {
        if (isHired)
        {
            Debug.LogWarning("Intento de contratar un negocio ya contratado");
            return;
        }

        isHired = true;

        if (spawnedBusiness == null)
        {
            Debug.LogError("spawnedBusiness es null en OnHireBusiness");
            return;
        }

        Business businessComponent = spawnedBusiness.GetComponent<Business>();
        if (businessComponent == null)
        {
            Debug.LogError("No se pudo obtener el componente Business del spawnedBusiness");
            return;
        }

        businessComponent.SetHired(true);

        if (BusinessManagerTracker.Instance == null)
        {
            Debug.LogError("BusinessManagerTracker.Instance es null en OnHireBusiness");
            return;
        }

        BusinessManagerTracker.Instance.RegisterHiredBusiness(businessComponent);

        if (hireFXPrefab != null)
        {
            Instantiate(hireFXPrefab, transform.position, Quaternion.identity);
        }

        if (audioSource != null && hireSoundClip != null)
        {
            audioSource.PlayOneShot(hireSoundClip);
        }

        Destroy(gameObject);
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