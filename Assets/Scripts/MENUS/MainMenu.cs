using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameplaySceneName = "GameplayScene"; // Nombre de la escena de juego

    // Función simplificada para iniciar una nueva partida
    public void StartNewGame()
    {
        SceneManager.LoadScene(gameplaySceneName); // Cambia a la escena del juego
    }

    // Función para salir del juego con confirmación
    public void ExitGame()
    {
        Application.Quit(); // Cierra la aplicación
    }
}
