using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/savefile.json";

    public static void SaveGame(int money, int points, float timePlayed)
    {
        // Aquí no forzamos la conversión si GameData espera un float
        GameData data = new GameData(money, points, timePlayed);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return null;
    }

    public static void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
    }

    // Método para cargar datos de ranking desde un archivo utilizando GameData
    public static GameData[] LoadRankings()
    {
        string rankingFilePath = Application.persistentDataPath + "/rankings.json";
        if (File.Exists(rankingFilePath))
        {
            string json = File.ReadAllText(rankingFilePath);
            return JsonUtility.FromJson<GameData[]>(json); // Usamos GameData[]
        }
        return new GameData[0]; // Retorna un array vacío si no hay rankings
    }
}
