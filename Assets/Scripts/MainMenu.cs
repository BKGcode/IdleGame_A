using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    // Referencias a los botones del men� principal
    public Button newGameButton;
    public Button rankingButton;
    public Button optionsButton;
    public Button quitButton;

    // Referencias a los popups
    public GameObject newGamePopup;
    public GameObject rankingPopup;
    public GameObject optionsPopup;
    public GameObject quitPopup;

    // Referencias a los botones dentro de los popups
    public Button confirmNewGameButton;
    public Button cancelNewGameButton;
    public Button closeRankingButton;
    public Button closeOptionsButton;
    public Button confirmQuitButton;
    public Button cancelQuitButton;

    // Referencias para el ranking
    public GameObject rankingEntryPrefab;  // Prefab de la entrada del ranking
    public Transform rankingContent;       // Contenedor donde se a�aden las entradas

    private void Start()
    {
        // Asignar listeners a los botones del men� principal
        newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        rankingButton.onClick.AddListener(OpenRanking);
        optionsButton.onClick.AddListener(OpenOptions);
        quitButton.onClick.AddListener(ShowQuitPopup);

        // Asignar listeners a los botones del popup de nueva partida
        confirmNewGameButton.onClick.AddListener(ConfirmNewGame);
        cancelNewGameButton.onClick.AddListener(CancelNewGame);

        // Asignar listener al bot�n de cerrar del popup de ranking
        closeRankingButton.onClick.AddListener(CloseRanking);

        // Asignar listeners a los botones del popup de opciones
        closeOptionsButton.onClick.AddListener(CloseOptions);

        // Asignar listeners a los botones del popup de salir
        confirmQuitButton.onClick.AddListener(ConfirmQuit);
        cancelQuitButton.onClick.AddListener(CancelQuit);

        // Asegurarse de que los popups est�n desactivados al inicio
        if (newGamePopup != null)
            newGamePopup.SetActive(false);

        if (rankingPopup != null)
            rankingPopup.SetActive(false);

        if (optionsPopup != null)
            optionsPopup.SetActive(false);

        if (quitPopup != null)
            quitPopup.SetActive(false);
    }

    // M�todo llamado al hacer clic en el bot�n "Nuevo Juego"
    public void OnNewGameButtonClicked()
    {
        // Mostrar el popup de confirmaci�n para iniciar una nueva partida
        if (newGamePopup != null)
            newGamePopup.SetActive(true);
    }

    // M�todo llamado al confirmar iniciar una nueva partida
    public void ConfirmNewGame()
    {
        // Cerrar el popup de nueva partida
        if (newGamePopup != null)
            newGamePopup.SetActive(false);

        // Iniciar una nueva partida
        StartNewGame();
    }

    // M�todo para cancelar la creaci�n de una nueva partida
    public void CancelNewGame()
    {
        if (newGamePopup != null)
            newGamePopup.SetActive(false);
    }

    // M�todo para iniciar una nueva partida
    private void StartNewGame()
    {
        // Reiniciar los datos del ScoreManager
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ResetData();
        }

        // Cargar la escena SCENE_A
        SceneManager.LoadScene("SCENE_A");
    }

    // M�todo para abrir el popup de ranking
    public void OpenRanking()
    {
        if (rankingPopup != null)
            rankingPopup.SetActive(true);

        UpdateRankingUI();
    }

    // M�todo para cerrar el popup de ranking
    public void CloseRanking()
    {
        if (rankingPopup != null)
            rankingPopup.SetActive(false);
    }

    // M�todo para actualizar la UI del ranking
    public void UpdateRankingUI()
    {
        // Limpiar las entradas existentes
        foreach (Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        // Verificar que el ScoreManager existe
        if (ScoreManager.instance != null)
        {
            // Obtener las mejores partidas del ScoreManager
            List<ScoreManager.GameData> bestGames = ScoreManager.instance.bestGames;

            // Generar las entradas del ranking
            for (int i = 0; i < bestGames.Count; i++)
            {
                ScoreManager.GameData gameData = bestGames[i];

                // Instanciar el prefab
                GameObject entryObj = Instantiate(rankingEntryPrefab, rankingContent);

                // Obtener el componente de texto y actualizarlo
                TextMeshProUGUI entryText = entryObj.GetComponent<TextMeshProUGUI>();
                int position = i + 1;
                int score = gameData.score;
                float time = gameData.time;
                int minutes = Mathf.FloorToInt(time / 60F);
                int seconds = Mathf.FloorToInt(time % 60F);
                int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);

                entryText.text = string.Format("{0}. Puntos: {1} - Tiempo: {2:00}:{3:00}:{4:00}", position, score, minutes, seconds, milliseconds);
            }
        }
    }

    // Funci�n para abrir el men� de opciones
    public void OpenOptions()
    {
        if (optionsPopup != null)
            optionsPopup.SetActive(true);
    }

    // Funci�n para cerrar el men� de opciones
    public void CloseOptions()
    {
        if (optionsPopup != null)
            optionsPopup.SetActive(false);
    }

    // Funci�n para mostrar el popup de salir
    public void ShowQuitPopup()
    {
        if (quitPopup != null)
            quitPopup.SetActive(true);
    }

    // Funci�n para confirmar la salida del juego
    public void ConfirmQuit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // Funci�n para cancelar la salida
    public void CancelQuit()
    {
        if (quitPopup != null)
            quitPopup.SetActive(false);
    }
}
