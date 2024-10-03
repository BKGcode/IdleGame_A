using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Button exitButton;  // Referencia al botón "Salir"

    private void Start()
    {
        // Asignar el listener al botón
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    // Método para regresar al menú principal
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // Cambia "MainMenu" por el nombre de tu escena de menú principal
    }
}
