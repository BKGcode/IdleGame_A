using UnityEngine;
using TMPro;
using UnityEngine.UI; // A�adir esta directiva para usar el tipo Button

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI gameOverPointsText;
    [SerializeField] private TextMeshProUGUI gameOverMoneyText;
    [SerializeField] private TextMeshProUGUI gameOverTimeText;
    [SerializeField] private Button restartButton; // Bot�n de Reiniciar

    private GameManager gameManager;
    private SaveManager saveManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        saveManager = FindObjectOfType<SaveManager>();
        gameOverPopup.SetActive(false); // Asegurarse de que el popup est� oculto al inicio

        // A�adir el listener para el bot�n de reinicio
        restartButton.onClick.AddListener(RestartGame);
    }

    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);

        // Actualizar los textos con la informaci�n del GameManager
        gameOverPointsText.text = gameManager.GetPoints().ToString();
        gameOverMoneyText.text = gameManager.GetMoney().ToString();

        // Formatear el tiempo en minutos:segundos (MM:SS)
        float timePlayed = gameManager.GetTimePlayed();
        int minutes = Mathf.FloorToInt(timePlayed / 60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);
        gameOverTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Mostrar tiempo formateado
    }

    // M�todo para guardar los datos de la partida y salir al men� principal
    public void ExitToMainMenu()
    {
        // Guardar la partida en los rankings
        SaveGameData();

        // Ocultar popup y salir al men� principal
        gameOverPopup.SetActive(false);
        gameManager.ExitToMainMenu();
    }

    private void SaveGameData()
    {
        if (saveManager != null)
        {
            // Crear una nueva entrada de GameData
            GameData gameData = new GameData(gameManager.GetPoints(), gameManager.GetTimePlayed(), gameManager.GetMoney());

            // Guardar los datos utilizando el SaveManager
            saveManager.SaveGameData(gameData);
        }
    }

    // M�todo para reiniciar el juego
    private void RestartGame()
    {
        // Llamar al m�todo de reiniciar partida en GameManager
        gameManager.RestartGame();
    }
}
