using UnityEngine;
using UnityEngine.Events;
using TMPro; // Importante si usas TextMeshPro para los textos

public class ConfirmationPopup : MonoBehaviour
{
    public UnityEvent OnConfirm;
    public UnityEvent OnCancel;
    public TextMeshProUGUI messageText; // Referencia opcional para mostrar un mensaje en el popup

    // Método para inicializar el mensaje y las acciones
    public void Initialize(string message, UnityAction confirmAction, UnityAction cancelAction)
    {
        if (messageText != null)
        {
            messageText.text = message; // Configura el mensaje si se proporciona un TextMeshProUGUI
        }

        // Configurar las acciones de confirmación y cancelación
        OnConfirm.RemoveAllListeners(); // Limpia los listeners anteriores
        OnCancel.RemoveAllListeners();

        OnConfirm.AddListener(confirmAction);
        OnCancel.AddListener(cancelAction);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Confirm()
    {
        OnConfirm?.Invoke();
        Hide();
    }

    public void Cancel()
    {
        OnCancel?.Invoke();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
