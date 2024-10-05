using System;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Save the current game state
    public void SaveGame()
    {
        SaveSystem.SaveGame(GameManager.Instance.playerMoney, GameManager.Instance.points, GameManager.Instance.timePlayed);
    }

    // Load a saved game state
    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            GameManager.Instance.SetPlayerStats(data.money, data.points, data.timePlayed);
        }
    }

    // Save game data specifically for ranking, etc.
    public void SaveGameData()
    {
        SaveGame();
    }

    // Optional: Delete current save file
    public void DeleteCurrentSave()
    {
        SaveSystem.DeleteSaveFile(); // Optional: clears save data for a fresh start
    }
}
