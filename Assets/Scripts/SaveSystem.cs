using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    private static string filePath = Application.persistentDataPath + "/saveData.json"; // Ruta del archivo de guardado

    // Guarda los datos de partida en un archivo JSON
    public static void SaveGame(GameData data)
    {
        List<GameData> savedGames = LoadGameData(); // Cargar los datos anteriores
        if (savedGames == null)
            savedGames = new List<GameData>();

        savedGames.Add(data); // Añadir la nueva partida al ranking
        string json = JsonUtility.ToJson(new GameDataList(savedGames), true); // Convertir a JSON
        File.WriteAllText(filePath, json); // Escribir en el archivo
    }

    // Cargar todas las partidas guardadas desde el archivo JSON
    public static List<GameData> LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameDataList dataList = JsonUtility.FromJson<GameDataList>(json);
            return dataList.games;
        }
        return new List<GameData>();
    }

    // Borrar los datos de guardado (reiniciar ranking)
    public static void ClearSaveData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

[System.Serializable]
public class GameDataList
{
    public List<GameData> games;

    public GameDataList(List<GameData> games)
    {
        this.games = games;
    }
}
