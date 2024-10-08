using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Gestiona la pantalla de Game Over, incluyendo la visualización y los botones para reiniciar o salir.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Panel de Game Over
    [SerializeField] private Button mainMenuButton; // Botón para volver al Main Menu
    [SerializeField] private Button exitButton; // Botón para salir del juego
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Nombre de la escena del Main Menu

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
    }

    /// <summary>
    /// Muestra el panel de Game Over.
    /// </summary>
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego
        }
    }

    /// <summary>
    /// Vuelve al Main Menu.
    /// </summary>
    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene(mainMenuSceneName); // Cargar la escena especificada
    }

    /// <summary>
    /// Sale del juego.
    /// </summary>
    private void ExitGame()
    {
        Application.Quit();

        // En el Editor de Unity, para probar el funcionamiento
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
