using UnityEngine;

[CreateAssetMenu(fileName = "NewSaveData", menuName = "Save System/Save Data")]
public class SaveData : ScriptableObject
{
    public int currentLives;
    public float currentTime;
    public int currentScore;

    // Método para resetear los datos de guardado
    public void ResetData(int maxLives)
    {
        currentLives = maxLives;
        currentTime = 0f;
        currentScore = 0;
    }
}
