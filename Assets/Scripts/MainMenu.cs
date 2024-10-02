using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // Asignaciones manuales desde el Inspector
    public GameObject newGameButton;
    public GameObject optionsButton;
    public GameObject loadGameButton;
    public GameObject quitButton;

    // Referencia al OptionsMenu
    public OptionsMenu optionsMenu;  // Referencia al script OptionsMenu

    // Pop-up para confirmar salida
    public GameObject quitPopup;  // El panel del pop-up de salida
    private TextMeshProUGUI popupMessage;  // Texto del pop-up usando TextMeshProUGUI

    private void Start()
    {
        // Aseguramos que el pop-up de salida esté inicialmente oculto
        quitPopup.SetActive(false);

        // Obtenemos el componente TextMeshProUGUI directamente del objeto asociado
        popupMessage = quitPopup.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Función para el botón "Nuevo Juego"
    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");  // Cambia "GameScene" por el nombre de tu escena de juego
    }

    // Función para el botón "Opciones"
    public void OpenOptions()
    {
        // Llamamos a la función que abre el menú de opciones
        optionsMenu.ShowOptions();
    }

    // Función para el botón "Cargar"
    public void LoadGame()
    {
        Debug.Log("Load game (to be implemented)");
    }

    // Función para el botón "Salir" (que muestra el pop-up)
    public void ShowQuitPopup()
    {
        quitPopup.SetActive(true);  // Mostrar el pop-up
    }

    // Función para confirmar la salida del juego
    public void ConfirmQuit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    // Función para cancelar la salida
    public void CancelQuit()
    {
        quitPopup.SetActive(false);  // Ocultar el pop-up
    }
}
