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
        // Aseguramos que el pop-up de salida est� inicialmente oculto
        quitPopup.SetActive(false);

        // Obtenemos el componente TextMeshProUGUI directamente del objeto asociado
        popupMessage = quitPopup.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Funci�n para el bot�n "Nuevo Juego"
    public void StartNewGame()
    {
        SceneManager.LoadScene("GameScene");  // Cambia "GameScene" por el nombre de tu escena de juego
    }

    // Funci�n para el bot�n "Opciones"
    public void OpenOptions()
    {
        // Llamamos a la funci�n que abre el men� de opciones
        optionsMenu.ShowOptions();
    }

    // Funci�n para el bot�n "Cargar"
    public void LoadGame()
    {
        Debug.Log("Load game (to be implemented)");
    }

    // Funci�n para el bot�n "Salir" (que muestra el pop-up)
    public void ShowQuitPopup()
    {
        quitPopup.SetActive(true);  // Mostrar el pop-up
    }

    // Funci�n para confirmar la salida del juego
    public void ConfirmQuit()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }

    // Funci�n para cancelar la salida
    public void CancelQuit()
    {
        quitPopup.SetActive(false);  // Ocultar el pop-up
    }
}
