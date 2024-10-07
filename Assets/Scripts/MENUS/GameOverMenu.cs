using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverUI; // Referencia al panel de la UI del Game Over
    public string mainMenuSceneName = "MainMenu"; // Nombre de la escena del menú principal
    public string gameplaySceneName = "GameplayScene"; // Nombre de la escena de juego para reiniciar

    public SaveManager saveManager; // Referencia al SaveManager para reiniciar los datos
    public TimeSystem timeSystem; // Referencia al TimeSystem para reiniciar el tiempo
    private int currentSaveSlot = 0; // Slot de guardado actual (para reiniciar la partida)

    // Muestra el menú de Game Over
    public void ShowGameOver()
    {
        gameOverUI.SetActive(true); // Activa la UI del Game Over
    }

    // Función para confirmar y volver al menú principal
    public void ConfirmGameOver()
    {
        SceneManager.LoadScene(mainMenuSceneName); // Carga la escena del menú principal
    }

    // Método para reiniciar la partida
    public void RestartGame()
    {
        // Reinicia todos los datos de la partida en el slot actual
        saveManager.CreateNewGame(currentSaveSlot);

        // Asegúrate de que el tiempo se restablezca antes de reiniciar la escena
        if (timeSystem != null)
        {
            timeSystem.ResetTime();
        }

        SceneManager.LoadScene(gameplaySceneName); // Recarga la escena de juego para reiniciar
    }

    // Método para inicializar el Game Over (compatibilidad con LifeSystem)
    public void Initialize(int saveSlot)
    {
        currentSaveSlot = saveSlot; // Guarda el slot actual para usarlo en el reinicio
    }

    // Método para mostrar el menú de Game Over (llamado desde LifeSystem)
    public void Show()
    {
        ShowGameOver(); // Llama al método existente para activar la UI.
    }
}
