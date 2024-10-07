using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public ConfirmationPopup confirmationPopup;
    public GameObject saveSlotMenu; // Menú que muestra los 4 slots de guardado
    public GameObject loadSlotMenu; // Menú que muestra los 4 slots de carga
    public SaveManager saveManager;

    public void ShowSaveSlotMenu()
    {
        saveSlotMenu.SetActive(true);
    }

    public void ShowLoadSlotMenu()
    {
        loadSlotMenu.SetActive(true);
        saveManager.LoadGameData(); // Carga la información de los slots
    }

    public void SelectSaveSlot(int slotIndex)
    {
        if (saveManager.IsSlotUsed(slotIndex))
        {
            confirmationPopup.OnConfirm.AddListener(() => OverwriteSave(slotIndex));
            confirmationPopup.OnCancel.AddListener(CancelSave);
            confirmationPopup.Show();
        }
        else
        {
            StartNewGame(slotIndex);
        }
    }

    private void OverwriteSave(int slotIndex)
    {
        StartNewGame(slotIndex);
        confirmationPopup.OnConfirm.RemoveAllListeners();
        confirmationPopup.OnCancel.RemoveAllListeners();
    }

    private void CancelSave()
    {
        confirmationPopup.OnConfirm.RemoveAllListeners();
        confirmationPopup.OnCancel.RemoveAllListeners();
    }

    private void StartNewGame(int slotIndex)
    {
        saveManager.CreateNewGame(slotIndex);
        SceneManager.LoadScene("X"); // Cambia a la escena del juego
    }

    public void LoadGame(int slotIndex)
    {
        saveManager.LoadGameFromSlot(slotIndex);
        SceneManager.LoadScene("X");
    }

    public void ExitGame()
    {
        confirmationPopup.OnConfirm.AddListener(ConfirmExit);
        confirmationPopup.OnCancel.AddListener(CancelExit);
        confirmationPopup.Show();
    }

    private void ConfirmExit()
    {
        Application.Quit();
        confirmationPopup.OnConfirm.RemoveListener(ConfirmExit);
        confirmationPopup.OnCancel.RemoveListener(CancelExit);
    }

    private void CancelExit()
    {
        confirmationPopup.OnConfirm.RemoveListener(ConfirmExit);
        confirmationPopup.OnCancel.RemoveListener(CancelExit);
    }
}
