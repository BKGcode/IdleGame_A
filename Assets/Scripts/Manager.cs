using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Manager Settings")]
    [SerializeField] private string managerName; // Nombre del manager
    [SerializeField] private int cost; // Costo para contratar al manager
    [SerializeField] private Business assignedBusiness; // Negocio que este manager va a automatizar

    [Header("UI Components")]
    [SerializeField] private GameObject costContainer; // Objeto que contiene el texto del coste (padre del texto)
    [SerializeField] private TMPro.TextMeshProUGUI costText; // Texto del costo del manager

    private bool isHired = false; // Indica si el manager está contratado

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no está inicializado.");
            return;
        }

        UpdateCostText();
        costContainer.SetActive(true); // Mostrar el contenedor del coste al inicio
        GameManager.Instance.OnMoneyChanged += UpdateInteractableState; // Suscribirse al evento de cambio de dinero
        UpdateInteractableState();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMoneyChanged -= UpdateInteractableState; // Cancelar la suscripción al evento de cambio de dinero
        }
    }

    // Detectar la colisión con un trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHired && GameManager.Instance.CanAfford(cost))
        {
            HireManager();
        }
    }

    public void HireManager()
    {
        if (GameManager.Instance != null && !isHired)
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
            costContainer.SetActive(false); // Ocultar el objeto contenedor del texto de coste
            Debug.Log(managerName + " ha sido contratado para automatizar " + assignedBusiness.name);
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

    private void UpdateInteractableState()
    {
        // No es necesario para botones, pero puedes agregar lógica aquí si es necesario.
    }
}
