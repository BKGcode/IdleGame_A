using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // --- Botones del Menú Principal ---
    [Header("Main Menu Buttons")]
    public Button newGameButton;
    public Button optionsButton;
    public Button rankingButton;
    public Button exitButton;

    // --- Panel de Confirmación ---
    [Header("Confirmation Popup")]
    public GameObject confirmationPrefab;
    public TextMeshProUGUI confirmationTitle;
    public TextMeshProUGUI confirmationMessage;
    public Image confirmationImage;
    public Button confirmButton;
    public Button cancelButton;

    // --- Texto e Imagen del Popup (Editable en el Inspector) ---
    [Header("New Game Confirmation")]
    public string newGameTitle;
    public string newGameMessage;
    public Sprite newGameImage;

    [Header("Exit Game Confirmation")]
    public string exitTitle;
    public string exitMessage;
    public Sprite exitImage;

    // --- Panel de Opciones ---
    [Header("Options Panel")]
    public GameObject optionsPanel;
    public Button closeOptionsButton;

    // --- Panel de Ranking ---
    [Header("Ranking Panel")]
    public GameObject rankingPanel;
    public Button closeRankingButton;

    // Delegados para las acciones de confirmación
    private Action confirmAction;

    void Start()
    {
        // Asignar funciones a los botones del menú principal
        newGameButton.onClick.AddListener(OnNewGame);
        optionsButton.onClick.AddListener(OpenOptions);
        rankingButton.onClick.AddListener(OpenRanking);
        exitButton.onClick.AddListener(OnExit);

        // Asignar funciones a los botones de cierre de paneles
        closeOptionsButton.onClick.AddListener(CloseOptionsPanel);
        closeRankingButton.onClick.AddListener(CloseRankingPanel);

        // Asegurarse de que los paneles estén cerrados al iniciar
        CloseAllPanels();
    }

    // --- Confirmación para Nuevo Juego ---
    void OnNewGame()
    {
        ShowConfirmation(newGameTitle,
                         newGameMessage,
                         newGameImage,
                         () =>
                         {
                             // Cargar la escena del juego si se confirma, sin cerrar el panel
                             SceneManager.LoadScene("SCENE_A");
                         });
    }

    // --- Confirmación para Salir del Juego ---
    void OnExit()
    {
        ShowConfirmation(exitTitle,
                         exitMessage,
                         exitImage,
                         () =>
                         {
                             // Cerrar el juego si se confirma, sin cerrar el panel
                             Application.Quit();
                         });
    }

    // --- Abrir Panel de Opciones ---
    void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    // --- Abrir Panel de Ranking ---
    void OpenRanking()
    {
        rankingPanel.SetActive(true);
    }

    // --- Cerrar el Panel de Opciones ---
    void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }

    // --- Cerrar el Panel de Ranking ---
    void CloseRankingPanel()
    {
        rankingPanel.SetActive(false);
    }

    // --- Mostrar Popup de Confirmación ---
    void ShowConfirmation(string title, string message, Sprite image, Action onConfirm)
    {
        if (confirmationTitle == null || confirmationMessage == null || confirmButton == null || cancelButton == null)
        {
            Debug.LogError("Please assign all popup components in the Inspector.");
            return;
        }

        confirmationTitle.text = title;
        confirmationMessage.text = message;

        if (image != null)
        {
            confirmationImage.sprite = image;
            confirmationImage.gameObject.SetActive(true);
        }
        else
        {
            confirmationImage.gameObject.SetActive(false);
        }

        confirmAction = onConfirm;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() =>
        {
            // Ejecutar la acción de confirmación, sin cerrar el panel
            confirmAction.Invoke();
        });

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() =>
        {
            // Cerrar el panel de confirmación al cancelar
            confirmationPrefab.SetActive(false);
        });

        confirmationPrefab.SetActive(true);
    }

    // --- Cerrar Todos los Paneles ---
    void CloseAllPanels()
    {
        optionsPanel.SetActive(false);
        rankingPanel.SetActive(false);
        confirmationPrefab.SetActive(false);
    }
}
