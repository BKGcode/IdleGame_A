using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/savefile.json";

    // Método para guardar los datos en JSON
    public static void SaveGame(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved: " + savePath);
    }

    // Método para cargar los datos desde JSON
    public static SaveData LoadGame(SaveData saveData)
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, saveData);
            Debug.Log("Game Loaded: " + savePath);
            return saveData;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return null;
        }
    }
}
