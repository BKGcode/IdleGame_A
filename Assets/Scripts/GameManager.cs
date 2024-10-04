using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    [SerializeField] private int points = 0; // Puntos actuales del jugador
    [SerializeField] private float timePlayed = 0f; // Tiempo jugado
    [SerializeField] private int playerMoney = 100; // Dinero inicial del jugador
    private float incomeMultiplier = 1f; // Multiplicador de ingresos
    private float cooldownReduction = 1f; // Reducción de cooldown (1 significa sin reducción)

    private SaveManager saveManager;

    public event Action OnMoneyChanged; // Evento que se dispara al cambiar el dinero
    public event Action OnPointsChanged; // Evento que se dispara al cambiar los puntos
    public event Action<GameState> OnGameStateChanged;
    public event Action OnGameOverAchievementsCheck;

    public enum GameState
    {
        Running,
        Paused,
        GameOver
    }

    private GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        if (saveManager == null)
        {
            Debug.LogError("No se encontró SaveManager en la escena.");
        }

        InvokeRepeating(nameof(UpdateTimePlayed), 0f, 1f); // Actualiza el tiempo jugado cada segundo
    }

    private void UpdateTimePlayed()
    {
        if (currentState == GameState.Running)
        {
            timePlayed += 1f;
        }
    }

    // Método para reiniciar el juego
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResetPlayerStats();
        ChangeGameState(GameState.Running);
    }

    // Método para agregar dinero
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        OnMoneyChanged?.Invoke(); // Disparar el evento cuando cambie el dinero
        Debug.Log("Dinero actualizado: " + playerMoney);
    }

    // Método para gastar dinero
    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            playerMoney -= amount;
            OnMoneyChanged?.Invoke(); // Disparar el evento cuando cambie el dinero
        }
    }

    public bool CanAfford(int amount)
    {
        return playerMoney >= amount;
    }

    public int GetMoney()
    {
        return playerMoney;
    }

    // Métodos para manejar los puntos
    public void AddPoints(int amount)
    {
        points += amount;
        OnPointsChanged?.Invoke(); // Disparar evento al cambiar puntos
        Debug.Log("Puntos actualizados: " + points);
    }

    public int GetPoints()
    {
        return points;
    }

    // Método para obtener el tiempo jugado
    public float GetTimePlayed()
    {
        return timePlayed;
    }

    // Método para salir al menú principal
    public void ExitToMainMenu()
    {
        if (saveManager != null)
        {
            saveManager.SaveGame(this);
        }
        SceneManager.LoadScene("MainMenu");
    }

    // Método para aplicar el multiplicador de ingresos
    public void ApplyIncomeMultiplier(float multiplier)
    {
        incomeMultiplier += multiplier;
        Debug.Log("Nuevo multiplicador de ingresos aplicado: " + incomeMultiplier);
    }

    // Método para reducir los cooldowns globales
    public void ReduceCooldowns(float reductionAmount)
    {
        cooldownReduction -= reductionAmount;
        if (cooldownReduction < 0.1f) // Evitar que el cooldown sea menor que 0.1
        {
            cooldownReduction = 0.1f;
        }
        Debug.Log("Reducción de cooldown aplicada: " + cooldownReduction);
    }

    // Métodos para obtener los multiplicadores y la reducción de cooldowns
    public float GetIncomeMultiplier()
    {
        return incomeMultiplier;
    }

    public float GetCooldownReduction()
    {
        return cooldownReduction;
    }

    private void ResetPlayerStats()
    {
        points = 0;
        timePlayed = 0f;
        playerMoney = 100;
        incomeMultiplier = 1f;
        cooldownReduction = 1f;
    }

    private void ChangeGameState(GameState newState)
    {
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void SetPlayerStats(int money, int points, float timePlayed)
    {
        this.playerMoney = money;
        this.points = points;
        this.timePlayed = timePlayed;
    }

    public void LoadGame()
    {
        if (saveManager != null)
        {
            saveManager.LoadGame(this);
        }
    }

    // Método para manejar el Game Over
    public void GameOver()
    {
        Debug.Log("El juego ha terminado.");
        ChangeGameState(GameState.GameOver);
        OnGameOverAchievementsCheck?.Invoke(); // Llamar evento para verificar logros de Game Over
    }
}
