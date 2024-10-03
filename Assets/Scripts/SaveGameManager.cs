using UnityEngine;
using System;
using System.IO;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;

    private void Awake()
    {
        // Implementar el patrón Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Clase para almacenar los datos de la partida
    [Serializable]
    public class SaveData
    {
        public string dateTime;  // Fecha y hora de guardado
        // Añade aquí otros datos que quieras guardar
    }

    // Método para verificar si hay una partida guardada en una ranura
    public bool IsSlotSaved(int slot)
    {
        string path = GetSaveFilePath(slot);
        return File.Exists(path);
    }

    // Método para guardar la partida en una ranura
    public void SaveGame(int slot, SaveData data)
    {
        string path = GetSaveFilePath(slot);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    // Método para cargar la partida desde una ranura
    public SaveData LoadGame(int slot)
    {
        string path = GetSaveFilePath(slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else
        {
            Debug.LogError("No se encontró una partida guardada en la ranura " + slot);
            return null;
        }
    }

    // Método para obtener la información de la partida guardada en una ranura
    public string GetSaveInfo(int slot)
    {
        if (IsSlotSaved(slot))
        {
            SaveData data = LoadGame(slot);
            return "Guardado el: " + data.dateTime;
        }
        else
        {
            return null;
        }
    }

    // Método para obtener la ruta del archivo de guardado
    private string GetSaveFilePath(int slot)
    {
        return Application.persistentDataPath + "/savegame_slot" + slot + ".json";
    }
}
