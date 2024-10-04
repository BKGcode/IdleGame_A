using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI moneyText; // Elemento de texto para el dinero
    [SerializeField] private TextMeshProUGUI pointsText; // Elemento de texto para los puntos
    [SerializeField] private TextMeshProUGUI timePlayedText; // Elemento de texto para el tiempo jugado

    public int score; // Puntos del jugador
    public float timePlayed; // Tiempo jugado
    public int money; // Dinero del jugador

    private void Start()
    {
        // Actualizar los valores al inicio
        UpdateMoneyText();
        UpdatePointsText();
        UpdateTimePlayedText();

        // Suscribirse a los eventos de cambio en el GameManager
        GameManager.Instance.OnMoneyChanged += UpdateMoneyText;
        GameManager.Instance.OnPointsChanged += UpdatePointsText;
    }

    private void Update()
    {
        // Actualizar el tiempo en cada frame
        UpdateTimePlayedText();
    }

    private void OnDestroy()
    {
        // Cancelar suscripción cuando el objeto es destruido
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnMoneyChanged -= UpdateMoneyText;
            GameManager.Instance.OnPointsChanged -= UpdatePointsText;
        }
    }

    private void UpdateMoneyText()
    {
        if (GameManager.Instance != null)
        {
            moneyText.text = GameManager.Instance.GetMoney().ToString();
        }
    }

    private void UpdatePointsText()
    {
        if (GameManager.Instance != null)
        {
            pointsText.text = GameManager.Instance.GetPoints().ToString();
        }
    }

    private void UpdateTimePlayedText()
    {
        if (GameManager.Instance != null)
        {
            float timePlayed = GameManager.Instance.GetTimePlayed();
            int minutes = Mathf.FloorToInt(timePlayed / 60F);
            int seconds = Mathf.FloorToInt(timePlayed % 60F);
            timePlayedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Método para resetear los contadores y estadísticas de la UI al reiniciar el juego
    public void ResetUI()
    {
        score = 0; // Reiniciar los puntos
        timePlayed = 0; // Reiniciar el tiempo de juego
        money = 100; // Restablecer el dinero inicial

        // Actualizar los textos en la interfaz
        UpdateMoneyText();
        UpdatePointsText();
        UpdateTimePlayedText();
    }
}
