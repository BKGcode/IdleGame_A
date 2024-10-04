using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SaveGameData(GameData gameData)
    {
        // Llamar a SaveSystem para guardar los datos
        SaveSystem.SaveGameData(gameData);
    }
}
