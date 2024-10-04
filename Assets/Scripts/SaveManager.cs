using UnityEngine;
using System;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    // Guardar juego en JSON
    public void SaveGame(GameManager gameManager)
    {
        SaveData data = new SaveData(gameManager.GetMoney(), gameManager.GetPoints(), gameManager.GetTimePlayed());
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Juego guardado en: " + saveFilePath);
    }

    // Cargar juego desde JSON
    public void LoadGame(GameManager gameManager)
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            gameManager.SetPlayerStats(data.money, data.points, data.timePlayed);
            Debug.Log("Juego cargado desde: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No se encontró archivo de guardado.");
        }
    }
}

[Serializable]
public class SaveData
{
    public int money;
    public int points;
    public float timePlayed;

    public SaveData(int money, int points, float timePlayed)
    {
        this.money = money;
        this.points = points;
        this.timePlayed = timePlayed;
    }
}
