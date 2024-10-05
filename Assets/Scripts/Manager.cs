using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Manager Settings")]
    [SerializeField] private string managerName; // Nombre del manager
    [SerializeField] private int cost; // Costo para contratar al manager
    [SerializeField] private Business assignedBusiness; // Negocio que este manager va a automatizar

    [Header("UI Components")]
    [SerializeField] private GameObject costContainer; // Contenedor del costo en la UI
    [SerializeField] private TMPro.TextMeshProUGUI costText; // Texto para mostrar el costo
    [SerializeField] private UIManager uiManager; // Referencia a UIManager para actualizar la UI
    private bool isHired = false;

    private void Start()
    {
        UpdateCostText(); // Actualizar el texto del costo al iniciar
    }

    // Detectar la colisión con el jugador para contratar al manager
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHired)
        {
            TryHireManager(); // Intentar contratar al manager si aún no está contratado
        }
    }

    private void TryHireManager()
    {
        if (GameManager.Instance.CanAfford(cost))
        {
            GameManager.Instance.SpendMoney(cost); // Restar el costo al dinero del jugador
            isHired = true;
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
}
