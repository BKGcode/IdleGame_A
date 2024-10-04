using UnityEngine;
using TMPro;
using UnityEngine.UI; // Añadir esta directiva para usar el tipo Button

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI gameOverPointsText;
    [SerializeField] private TextMeshProUGUI gameOverMoneyText;
    [SerializeField] private TextMeshProUGUI gameOverTimeText;
    [SerializeField] private Button restartButton; // Botón de Reiniciar

    private GameManager gameManager;
    private SaveManager saveManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        saveManager = FindObjectOfType<SaveManager>();
        gameOverPopup.SetActive(false); // Asegurarse de que el popup esté oculto al inicio

        // Añadir el listener para el botón de reinicio
        restartButton.onClick.AddListener(RestartGame);
    }

    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);

        // Actualizar los textos con la información del GameManager
        gameOverPointsText.text = gameManager.GetPoints().ToString();
        gameOverMoneyText.text = gameManager.GetMoney().ToString();

        // Formatear el tiempo en minutos:segundos (MM:SS)
        float timePlayed = gameManager.GetTimePlayed();
        int minutes = Mathf.FloorToInt(timePlayed / 60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);
        gameOverTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Mostrar tiempo formateado
    }

    // Método para guardar los datos de la partida y salir al menú principal
    public void ExitToMainMenu()
    {
        // Guardar la partida en los rankings
        SaveGameData();

        // Ocultar popup y salir al menú principal
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

    // Método para reiniciar el juego
    private void RestartGame()
    {
        // Llamar al método de reiniciar partida en GameManager
        gameManager.RestartGame();
    }
}
