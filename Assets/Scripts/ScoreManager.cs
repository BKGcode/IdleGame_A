using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;  // Singleton para acceder desde otros scripts

    public int currentScore = 0;          // Puntaje actual del jugador
    public TextMeshProUGUI scoreText;     // Referencia al texto de puntaje en la UI

    private float startTime;              // Tiempo de inicio del juego
    private float elapsedTime;            // Tiempo transcurrido
    public TextMeshProUGUI timeText;      // Referencia al texto de tiempo en la UI

    private bool isGameOver = false;      // Variable para controlar si el juego ha terminado

    // Lista de las mejores partidas
    public List<GameData> bestGames = new List<GameData>();

    // Ruta para guardar el archivo de ranking
    private string rankingFilePath;

    private void Awake()
    {
        // Implementar el patrón Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Persistir entre escenas si es necesario
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicializar la ruta del archivo de ranking
        rankingFilePath = Application.persistentDataPath + "/ranking.json";

        // Cargar el ranking al iniciar
        LoadRanking();
    }

    private void Start()
    {
        // Inicializar el puntaje y la UI
        UpdateScoreUI();

        // Inicializar el tiempo de inicio
        startTime = Time.time;
    }

    private void Update()
    {
        if (!isGameOver)
        {
            // Actualizar el tiempo transcurrido
            elapsedTime = Time.time - startTime;
            UpdateTimeUI();
        }
    }

    // Método para añadir puntos
    public void AddPoints(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    // Método para restar puntos
    public void SubtractPoints(int amount)
    {
        currentScore -= amount;
        if (currentScore < 0)
            currentScore = 0;
        UpdateScoreUI();
    }

    // Método para actualizar la UI del puntaje
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Puntos: " + currentScore;
    }

    // Método para actualizar la UI del tiempo
    private void UpdateTimeUI()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime % 60F);
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100F) % 100F);
            timeText.text = string.Format("Tiempo: {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    // Método para obtener el tiempo transcurrido
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Método para indicar que el juego ha terminado
    public void GameOver()
    {
        isGameOver = true;
        // Almacenar el tiempo final
        elapsedTime = Time.time - startTime;
        // Actualizar la UI por última vez
        UpdateTimeUI();
    }

    // Método para guardar la partida actual en el ranking
    public void SaveGameData()
    {
        // Crear un objeto GameData con los datos actuales
        GameData currentGame = new GameData();
        currentGame.score = currentScore;
        currentGame.time = elapsedTime;

        // Añadir la partida a la lista
        bestGames.Add(currentGame);

        // Ordenar la lista según el puntaje (descendente)
        bestGames.Sort((a, b) => b.score.CompareTo(a.score));

        // Mantener solo las 10 mejores partidas
        if (bestGames.Count > 10)
        {
            bestGames.RemoveRange(10, bestGames.Count - 10);
        }

        // Guardar el ranking en un archivo
        SaveRanking();
    }

    // Método para guardar el ranking en un archivo
    private void SaveRanking()
    {
        string json = JsonUtility.ToJson(new GameDataList(bestGames), true);
        File.WriteAllText(rankingFilePath, json);
    }

    // Método para cargar el ranking desde un archivo
    private void LoadRanking()
    {
        if (File.Exists(rankingFilePath))
        {
            string json = File.ReadAllText(rankingFilePath);
            GameDataList dataList = JsonUtility.FromJson<GameDataList>(json);
            bestGames = dataList.list;
        }
    }

    // Método para reiniciar los datos al iniciar una nueva partida
    public void ResetData()
    {
        currentScore = 0;
        elapsedTime = 0f;
        isGameOver = false;
        startTime = Time.time;
        UpdateScoreUI();
        UpdateTimeUI();
    }

    // Clase para representar una partida
    [System.Serializable]
    public class GameData
    {
        public int score;
        public float time;
    }

    // Clase auxiliar para serializar la lista
    [System.Serializable]
    private class GameDataList
    {
        public List<GameData> list;

        public GameDataList(List<GameData> list)
        {
            this.list = list;
        }
    }
}
