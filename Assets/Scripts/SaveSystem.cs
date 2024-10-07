using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath;

    public static void SaveGame(SaveData saveData, int slotIndex)
    {
        string json = JsonUtility.ToJson(saveData);
        string slotSavePath = GetSlotSavePath(slotIndex);
        File.WriteAllText(slotSavePath, json);
        Debug.Log("Game Saved: " + slotSavePath);
    }

    public static SaveData LoadGame(SaveData saveData, int slotIndex)
    {
        string slotSavePath = GetSlotSavePath(slotIndex);
        if (File.Exists(slotSavePath))
        {
            string json = File.ReadAllText(slotSavePath);
            JsonUtility.FromJsonOverwrite(json, saveData);
            Debug.Log("Game Loaded: " + slotSavePath);
            return saveData;
        }
        else
        {
            Debug.LogWarning("Save file not found for slot " + slotIndex);
            return null;
        }
    }

    private static string GetSlotSavePath(int slotIndex)
    {
        return Path.Combine(savePath, $"savefile_slot{slotIndex}.json");
    }
}
