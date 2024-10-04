using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    [Header("Popup UI Components")]
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI timeText;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Mostrar el popup de Game Over
    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true); // Mostrar popup
        pointsText.text = "Points: " + gameManager.GetPoints(); // Mostrar puntos
        timeText.text = "Time: " + gameManager.GetTimePlayed() + " seconds"; // Mostrar tiempo
    }

    // Método para ocultar el popup y reiniciar el juego
    public void RestartGame()
    {
        gameOverPopup.SetActive(false); // Ocultar popup
        gameManager.RestartGame(); // Reiniciar la partida
    }

    // Método para salir al menú principal
    public void ExitToMainMenu()
    {
        gameOverPopup.SetActive(false); // Ocultar popup
        gameManager.ExitToMainMenu(); // Volver al menú principal
    }
}
