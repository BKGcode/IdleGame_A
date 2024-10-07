using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public UnityEvent OnRestart;
    private int currentSaveSlot = 0;

    // Método para inicializar con el slot actual
    public void Initialize(int saveSlot)
    {
        currentSaveSlot = saveSlot;
    }

    public void RestartGame()
    {
        OnRestart?.Invoke();
        SceneManager.LoadScene("Gameplay"); // Reinicia la escena de juego
    }

    public void ReturnToMainMenu()
    {
        SaveManager.Instance.SaveGame(currentSaveSlot); // Guarda los datos antes de salir al MainMenu
        SceneManager.LoadScene("MainMenu");
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
