using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText; // Texto que muestra el dinero
    [SerializeField] private TextMeshProUGUI pointsText; // Texto que muestra los puntos
    [SerializeField] private TextMeshProUGUI timePlayedText; // Texto que muestra el tiempo jugado
    [SerializeField] private Image[] lifeIcons; // Im�genes que representan las vidas

    private void OnEnable()
    {
        GameManager.Instance.OnMoneyChanged += UpdateMoneyText;
        GameManager.Instance.OnPointsChanged += UpdatePointsText;
        // Suscribirse a otros eventos si es necesario
    }

    private void OnDisable()
    {
        GameManager.Instance.OnMoneyChanged -= UpdateMoneyText;
        GameManager.Instance.OnPointsChanged -= UpdatePointsText;
        // Cancelar suscripci�n a otros eventos si es necesario
    }

    private void Start()
    {
        UpdateMoneyText();
        UpdatePointsText();
        UpdateTimePlayedText();
    }

    // M�todo para actualizar el texto del dinero
    private void UpdateMoneyText()
    {
        moneyText.text = $"Money: {GameManager.Instance.GetMoney()}";
    }

    // M�todo para actualizar el texto de los puntos
    private void UpdatePointsText()
    {
        pointsText.text = $"Points: {GameManager.Instance.GetPoints()}";
    }

    // M�todo para actualizar el texto del tiempo jugado
    private void UpdateTimePlayedText()
    {
        timePlayedText.text = $"Time Played: {GameManager.Instance.GetTimePlayed()}s";
    }

    // M�todo para actualizar las vidas en la UI
    public void UpdateLivesUI(int currentLives)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < currentLives;
        }
    }
}
