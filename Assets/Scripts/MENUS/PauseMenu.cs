using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Referencia al panel de la UI del menú de pausa
    public string mainMenuSceneName = "MainMenu"; // Nombre de la escena del menú principal

    private bool isPaused = false; // Estado de pausa

    private void Update()
    {
        // Detecta si se presiona la tecla ESC para pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Pausa el juego y muestra el menú de pausa
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Muestra la UI del menú de pausa
        Time.timeScale = 0f; // Detiene el tiempo del juego
        isPaused = true;
    }

    // Reanuda el juego y oculta el menú de pausa
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Oculta la UI del menú de pausa
        Time.timeScale = 1f; // Restaura el tiempo del juego
        isPaused = false;
    }

    // Sale al menú principal
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Asegura que el tiempo se reanude al salir
        SceneManager.LoadScene(mainMenuSceneName); // Carga la escena del menú principal
    }
}
