using UnityEngine;
using TMPro; // Para TextMeshPro

public class PopupManager : MonoBehaviour
{
    public GameObject gameOverPopup; // Panel del popup de Game Over
    public TextMeshProUGUI pointsText; // Texto para mostrar los puntos
    public TextMeshProUGUI timeText; // Texto para mostrar la duraci�n de la partida

    public GameManager gameManager; // Referencia al GameManager para obtener puntos y tiempo

    // M�todo para mostrar el popup de Game Over
    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true); // Activar el popup
        pointsText.text = "Points: " + gameManager.GetPoints(); // Mostrar puntos
        timeText.text = "Time: " + gameManager.GetTimePlayed() + " seconds"; // Mostrar tiempo
    }

    // M�todo para ocultar el popup y reiniciar el juego
    public void RestartGame()
    {
        gameOverPopup.SetActive(false); // Ocultar popup
        gameManager.RestartGame(); // Reiniciar la partida
    }

    // M�todo para salir al men� principal
    public void ExitToMainMenu()
    {
        gameOverPopup.SetActive(false); // Ocultar popup
        gameManager.ExitToMainMenu(); // Volver al men� principal
    }
}
