using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI moneyText; // Elemento de texto para el dinero
    [SerializeField] private TextMeshProUGUI pointsText; // Elemento de texto para los puntos
    [SerializeField] private TextMeshProUGUI timePlayedText; // Elemento de texto para el tiempo jugado
    [SerializeField] private TextMeshProUGUI livesText; // Elemento de texto para las vidas

    public int score; // Puntos del jugador
    public float timePlayed; // Tiempo jugado
    public int money; // Dinero del jugador
    public int lives; // Vidas del jugador

    private void Start()
    {
        UpdateMoneyUI();
        UpdatePointsUI();
        UpdateTimePlayedUI();
        UpdateLivesUI(lives); // Inicializar las vidas en la UI
    }

    // M�todo para actualizar el dinero en la UI
    public void UpdateMoneyUI()
    {
        moneyText.text = money.ToString();
    }

    // M�todo para actualizar los puntos en la UI
    public void UpdatePointsUI()
    {
        pointsText.text = score.ToString();
    }

    // M�todo para actualizar el tiempo jugado en la UI
    public void UpdateTimePlayedUI()
    {
        int minutes = Mathf.FloorToInt(timePlayed / 60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);
        timePlayedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // M�todo para actualizar las vidas en la UI
    public void UpdateLivesUI(int currentLives)
    {
        livesText.text = currentLives.ToString(); // Mostrar el n�mero de vidas restantes
    }

    // M�todos para agregar dinero y actualizar la UI
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    // M�todos para agregar puntos y actualizar la UI
    public void AddPoints(int amount)
    {
        score += amount;
        UpdatePointsUI();
    }

    // M�todo para actualizar el tiempo jugado cada frame
    private void Update()
    {
        timePlayed += Time.deltaTime;
        UpdateTimePlayedUI();
    }
}
