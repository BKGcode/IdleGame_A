using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/rankings.dat";

    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Append);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData[] LoadRankings()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            GameData[] data = (GameData[])formatter.Deserialize(stream);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + savePath);
            return null;
        }
    }
}
