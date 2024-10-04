using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [Header("Manager Settings")]
    [SerializeField] private string managerName; // Nombre del manager
    [SerializeField] private int cost; // Costo para contratar al manager
    [SerializeField] private Business assignedBusiness; // Negocio que este manager va a automatizar

    [Header("UI Components")]
    [SerializeField] private GameObject costContainer; // Objeto que contiene el texto del coste (padre del texto)
    [SerializeField] private TMPro.TextMeshProUGUI costText; // Texto del costo del manager
    [SerializeField] private Button hireButton; // Botón para contratar al manager

    private bool isHired = false; // Indica si el manager está contratado

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no está inicializado.");
            return;
        }

        UpdateCostText();
        hireButton.onClick.AddListener(HireManager);
        UpdateButtonState(); // Actualizar el estado del botón al inicio
        
        GameManager.Instance.OnMoneyChanged += UpdateButtonInteractable; // Suscribirse al evento de cambio de dinero
        UpdateButtonInteractable(); // Actualizar el estado del botón al inicio
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMoneyChanged -= UpdateButtonInteractable; // Cancelar la suscripción al evento de cambio de dinero
        }
    }

    public void HireManager()
    {
        if (GameManager.Instance != null && GameManager.Instance.CanAfford(cost) && !isHired)
        {
            GameManager.Instance.SpendMoney(cost); // Gasta el dinero
            isHired = true; // Marcar como contratado

            // Verificar si el negocio asignado no es nulo
            if (assignedBusiness == null)
            {
                Debug.LogError("assignedBusiness no está asignado al manager.");
                return;
            }

            AutomateBusiness(); // Automatizar el negocio
            UpdateButtonState(); // Actualizar el estado del botón después de contratar
            hireButton.gameObject.SetActive(false); // Ocultar el botón de contratar
            costContainer.SetActive(false); // Ocultar el objeto contenedor del texto de coste
            Debug.Log(managerName + " ha sido contratado para automatizar " + assignedBusiness.name);
        }
        else
        {
            Debug.LogWarning("No se puede contratar el manager: Dinero insuficiente o ya está contratado.");
        }
    }

    private void AutomateBusiness()
    {
        if (assignedBusiness != null)
        {
            assignedBusiness.AutomateBusiness();
            Debug.Log("El negocio " + assignedBusiness.name + " ahora está automatizado por " + managerName);
        }
    }

    private void UpdateCostText()
    {
        if (costText != null)
        {
            costText.text = cost.ToString(); // Actualiza el texto del costo
        }
    }

    private void UpdateButtonState()
    {
        // El botón solo será interactivo si el manager no está contratado
        hireButton.interactable = !isHired;
    }

    private void UpdateButtonInteractable()
    {
        if (!isHired && GameManager.Instance != null)
        {
            hireButton.interactable = GameManager.Instance.CanAfford(cost); // Solo habilita el botón si el jugador tiene suficiente dinero
        }
    }
}
