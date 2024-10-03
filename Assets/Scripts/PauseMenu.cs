using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Referencias a los paneles
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject confirmExitPanel;
    public GameObject confirmRestartPanel;

    // Referencias a los botones del PausePanel
    [Header("Pause Panel Buttons")]
    public Button salirButton;
    public Button reiniciarButton;
    public Button cerrarButton; // Nueva referencia para el bot�n Cerrar

    // Referencias a los botones del ConfirmExitPanel
    [Header("Confirm Exit Panel Buttons")]
    public Button confirmarExitButton;
    public Button cerrarConfirmExitButton;

    // Referencias a los botones del ConfirmRestartPanel
    [Header("Confirm Restart Panel Buttons")]
    public Button confirmarRestartButton;
    public Button cerrarConfirmRestartButton;

    // Referencia al SpawnManager
    [Header("Spawn Manager")]
    public SpawnManager spawnManager;

    private bool isPaused = false;

    private void Start()
    {
        // Verificar que todas las referencias est�n asignadas
        if (pausePanel == null || confirmExitPanel == null || confirmRestartPanel == null ||
            salirButton == null || reiniciarButton == null ||
            confirmarExitButton == null || cerrarConfirmExitButton == null ||
            confirmarRestartButton == null || cerrarConfirmRestartButton == null ||
            cerrarButton == null ||
            spawnManager == null) // Verificar tambi�n el SpawnManager
        {
            Debug.LogError("Faltan referencias en el script PauseMenu.");
            return;
        }

        // Asegurarse de que todos los paneles est�n desactivados al inicio
        pausePanel.SetActive(false);
        confirmExitPanel.SetActive(false);
        confirmRestartPanel.SetActive(false);

        // Asignar listeners a los botones del PausePanel
        salirButton.onClick.AddListener(OnSalirClicked);
        reiniciarButton.onClick.AddListener(OnReiniciarClicked);
        cerrarButton.onClick.AddListener(OnCerrarClicked); // Asignar listener al bot�n Cerrar

        // Asignar listeners a los botones del ConfirmExitPanel
        confirmarExitButton.onClick.AddListener(OnConfirmarExitClicked);
        cerrarConfirmExitButton.onClick.AddListener(OnCerrarConfirmExitClicked);

        // Asignar listeners a los botones del ConfirmRestartPanel
        confirmarRestartButton.onClick.AddListener(OnConfirmarRestartClicked);
        cerrarConfirmRestartButton.onClick.AddListener(OnCerrarConfirmRestartClicked);
    }

    private void Update()
    {
        // Detectar si se presiona la tecla ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si alguno de los paneles de confirmaci�n est� activo, no hacer nada
            if (confirmExitPanel.activeSelf || confirmRestartPanel.activeSelf)
            {
                // Ignorar la tecla ESC para evitar conflictos
                return;
            }

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

    // M�todo para pausar el juego
    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;  // Detener el tiempo del juego
        AudioListener.pause = true;  // Pausar el audio
    }

    // M�todo para reanudar el juego
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;  // Reanudar el tiempo del juego
        AudioListener.pause = false;  // Reanudar el audio
    }

    // Acci�n al hacer clic en "Salir" en el PausePanel
    private void OnSalirClicked()
    {
        Debug.Log("SalirButton Clicked");
        // Cerrar el panel de pausa y abrir el panel de confirmaci�n de salir
        pausePanel.SetActive(false);
        confirmExitPanel.SetActive(true);
    }

    // Acci�n al hacer clic en "Reiniciar" en el PausePanel
    private void OnReiniciarClicked()
    {
        Debug.Log("ReiniciarButton Clicked");
        // Cerrar el panel de pausa y abrir el panel de confirmaci�n de reiniciar
        pausePanel.SetActive(false);
        confirmRestartPanel.SetActive(true);
    }

    // Acci�n al hacer clic en "Cerrar" en el PausePanel
    private void OnCerrarClicked()
    {
        Debug.Log("CerrarButton Clicked");
        // Reanudar el juego
        ResumeGame();
    }

    // Acci�n al hacer clic en "Confirmar" en el ConfirmExitPanel
    private void OnConfirmarExitClicked()
    {
        Debug.Log("ConfirmarExitButton Clicked");
        // Reiniciar el ScoreManager si existe
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetData();
        }

        // Reanudar el tiempo y el audio antes de cambiar de escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Cargar la escena del men� principal
        SceneManager.LoadScene("MainMenu");  // Aseg�rate de que el nombre coincide exactamente
    }

    // Acci�n al hacer clic en "Cerrar" en el ConfirmExitPanel
    private void OnCerrarConfirmExitClicked()
    {
        Debug.Log("CerrarConfirmExitButton Clicked");
        // Cerrar el panel de confirmaci�n de salir y volver al panel de pausa
        confirmExitPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // Acci�n al hacer clic en "Confirmar" en el ConfirmRestartPanel
    private void OnConfirmarRestartClicked()
    {
        Debug.Log("ConfirmarRestartButton Clicked");
        // Reiniciar el ScoreManager si existe
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetData();
        }

        // Reiniciar el SpawnManager si existe
        if (spawnManager != null)
        {
            spawnManager.ResetSpawn();
        }

        // Reanudar el tiempo y el audio antes de recargar la escena
        Time.timeScale = 1f;
        AudioListener.pause = false;

        // Recargar la escena actual para reiniciar la partida
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Acci�n al hacer clic en "Cerrar" en el ConfirmRestartPanel
    private void OnCerrarConfirmRestartClicked()
    {
        Debug.Log("CerrarConfirmRestartButton Clicked");
        // Cerrar el panel de confirmaci�n de reiniciar y volver al panel de pausa
        confirmRestartPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    private void OnDestroy()
    {
        // Remover listeners para evitar posibles errores
        salirButton.onClick.RemoveListener(OnSalirClicked);
        reiniciarButton.onClick.RemoveListener(OnReiniciarClicked);
        cerrarButton.onClick.RemoveListener(OnCerrarClicked); // Remover listener del bot�n Cerrar

        confirmarExitButton.onClick.RemoveListener(OnConfirmarExitClicked);
        cerrarConfirmExitButton.onClick.RemoveListener(OnCerrarConfirmExitClicked);

        confirmarRestartButton.onClick.RemoveListener(OnConfirmarRestartClicked);
        cerrarConfirmRestartButton.onClick.RemoveListener(OnCerrarConfirmRestartClicked);
    }
}
