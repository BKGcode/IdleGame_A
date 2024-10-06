using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SaveData saveData;  // Referencia al ScriptableObject de datos de guardado
    public LifeData lifeData;  // Referencia al ScriptableObject de vidas
    public TimeData timeData;  // Referencia al ScriptableObject de tiempo
    public ScoreData scoreData;  // Referencia al ScriptableObject de puntos

    private void Start()
    {
        // Comprobamos si el archivo de guardado existe
        if (SaveSystem.LoadGame(saveData) == null)
        {
            // Si no existe, inicializamos los datos
            ResetGame(lifeData.maxLives);
        }
        else
        {
            ApplyLoadedData();
        }
    }

    // Método para guardar los datos actuales
    public void SaveGame()
    {
        saveData.currentLives = lifeData.currentLives;
        saveData.currentTime = timeData.currentTime;
        saveData.currentScore = scoreData.currentScore;

        SaveSystem.SaveGame(saveData);
    }

    // Método para cargar los datos en los sistemas
    public void ApplyLoadedData()
    {
        lifeData.currentLives = saveData.currentLives;
        timeData.currentTime = saveData.currentTime;
        scoreData.currentScore = saveData.currentScore;

        lifeData.onLifeLost.Invoke();  // Actualizamos la UI de vidas
        timeData.onTimeChanged.Invoke();  // Actualizamos la UI de tiempo
        scoreData.onScoreChanged.Invoke();  // Actualizamos la UI de puntos
    }

    // Método para reiniciar los datos de guardado (cuando se empieza una nueva partida)
    public void ResetGame(int maxLives)
    {
        saveData.ResetData(maxLives);
        ApplyLoadedData();
    }
}
