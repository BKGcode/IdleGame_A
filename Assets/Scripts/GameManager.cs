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
        currentState = GameState.Running;
    }

    private void Update()
    {
        if (currentState == GameState.Running)
        {
            timePlayed += Time.deltaTime; // Solo actualizar el tiempo si el juego está en estado Running
        }
    }

    // Método para restaurar estadísticas del jugador al cargar una partida
    public void SetPlayerStats(int money, int points, float timePlayed)
    {
        this.playerMoney = money;
        this.points = points;
        this.timePlayed = timePlayed;

        OnMoneyChanged?.Invoke(); // Notificar que el dinero ha cambiado
        OnPointsChanged?.Invoke(); // Notificar que los puntos han cambiado
    }

    // Métodos para el manejo del multiplicador de ingresos y reducción de cooldowns
    public float GetIncomeMultiplier()
    {
        return incomeMultiplier;
    }

    public float GetCooldownReduction()
    {
        return cooldownReduction;
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        OnMoneyChanged?.Invoke(); // Notificar cuando cambie el dinero
    }

    public bool CanAfford(int cost)
    {
        return playerMoney >= cost;
    }

    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            playerMoney -= amount;
            OnMoneyChanged?.Invoke(); // Notificar cuando cambie el dinero
        }
    }

    public void ApplyIncomeMultiplier(float amount)
    {
        incomeMultiplier += amount; // Aplicar un multiplicador al ingreso
    }

    public void ReduceCooldowns(float amount)
    {
        cooldownReduction -= amount; // Reducir los cooldowns
        if (cooldownReduction < 0.1f) cooldownReduction = 0.1f; // Asegurarse de que no sea negativo
    }

    public int GetPoints()
    {
        return points;
    }

    public float GetTimePlayed()
    {
        return timePlayed;
    }

    public int GetMoney()
    {
        return playerMoney;
    }

    // Método que activa el Game Over y detiene el tiempo del juego
    public void SetGameOver()
    {
        currentState = GameState.GameOver;
        OnGameStateChanged?.Invoke(currentState);
        Time.timeScale = 0f; // Detener el tiempo del juego
    }

    // Reiniciar el juego
    public void RestartGame()
    {
        Time.timeScale = 1f; // Restablecer el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }

    // Volver al menú principal
    public void ExitToMainMenu()
    {
        Time.timeScale = 1f; // Restablecer el tiempo
        SceneManager.LoadScene("MainMenu"); // Volver al menú principal
    }
}
