using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public UnityEvent OnGameResume;
    public ConfirmationPopup confirmationPopup;
    private int currentSaveSlot = 0;

    // Método para inicializar el slot de guardado
    public void Initialize(int saveSlot)
    {
        currentSaveSlot = saveSlot;
    }

    public void ResumeGame()
    {
        OnGameResume?.Invoke();
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0; // Pausa el tiempo del juego
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1; // Restaura el tiempo del juego
    }

    public void ShowExitConfirmation()
    {
        confirmationPopup.Initialize(
            "¿Estás seguro de que deseas regresar al menú principal?",
            ExitToMainMenu,
            CancelExit
        );
        confirmationPopup.Show();
    }

    private void ExitToMainMenu()
    {
        SaveManager.Instance.SaveGame(currentSaveSlot); // Guarda los datos en el slot correcto
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void CancelExit()
    {
        confirmationPopup.OnConfirm.RemoveListener(ExitToMainMenu);
        confirmationPopup.OnCancel.RemoveListener(CancelExit);
    }
}
