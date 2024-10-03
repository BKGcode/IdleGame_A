using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public Button exitButton;  // Referencia al bot�n "Salir"

    private void Start()
    {
        // Asignar el listener al bot�n
        exitButton.onClick.AddListener(ExitToMainMenu);
    }

    // M�todo para regresar al men� principal
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // Cambia "MainMenu" por el nombre de tu escena de men� principal
    }
}
