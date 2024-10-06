using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Game Systems/Save Data")]
public class SaveData : ScriptableObject
{
    public int currentLives;
    public float currentTime;
    public int currentScore;

    // Método para reiniciar los datos
    public void ResetData(int maxLives)
    {
        currentLives = maxLives;
        currentTime = 0;
        currentScore = 0;
    }
}
