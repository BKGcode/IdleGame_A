using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerMoney;
    public int points;
    public float timePlayed;
    private SaveManager saveManager;
    private float incomeMultiplier = 1f;
    private float cooldownReduction = 1f;

    public Action OnMoneyChanged;
    public Action OnPointsChanged;
    public Action OnGameOverAchievementsCheck; // Evento para verificar logros cuando termina el juego

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
        NewGame(); // Comenzar con un nuevo estado del juego.
    }

    private void Update()
    {
        if (timePlayed >= 0) // Solo rastrear el tiempo cuando se está jugando
        {
            timePlayed += Time.deltaTime;
        }
    }

    // Método para iniciar o reiniciar una nueva partida
    public void NewGame()
    {
        playerMoney = 0;
        points = 0;
        timePlayed = 0;

        // Reiniciar todos los sistemas relevantes, como el sistema de vidas
        LifeSystem.Instance.ResetLives();
        OnMoneyChanged?.Invoke();
        OnPointsChanged?.Invoke();
    }

    // Método para reiniciar el juego
    public void RestartGame()
    {
        saveManager.DeleteCurrentSave(); // Eliminar datos antiguos si es necesario
        NewGame();
    }

    // Métodos para obtener puntos, dinero y tiempo jugado
    public int GetPoints()
    {
        return points;
    }

    public int GetMoney()
    {
        return playerMoney;
    }

    public float GetTimePlayed()
    {
        return timePlayed;
    }

    // Métodos relacionados con los multiplicadores de ingresos y reducción de cooldowns
    public float GetIncomeMultiplier()
    {
        return incomeMultiplier;
    }

    public float GetCooldownReduction()
    {
        return cooldownReduction;
    }

    public void ApplyIncomeMultiplier(float amount)
    {
        incomeMultiplier += amount;
    }

    public void ReduceCooldowns(float amount)
    {
        cooldownReduction -= amount;
        if (cooldownReduction < 0.1f) cooldownReduction = 0.1f;
    }

    // Métodos relacionados con la gestión de dinero
    public bool CanAfford(int cost)
    {
        return playerMoney >= cost;
    }

    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            playerMoney -= amount;
            OnMoneyChanged?.Invoke();
        }
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        OnMoneyChanged?.Invoke();
    }

    // Método para restaurar las estadísticas del jugador después de cargar una partida
    public void SetPlayerStats(int money, int points, float timePlayed)
    {
        this.playerMoney = money;
        this.points = points;
        this.timePlayed = timePlayed;

        OnMoneyChanged?.Invoke();
        OnPointsChanged?.Invoke();
    }

    // Método para salir al menú principal
    public void ExitToMainMenu()
    {
        Debug.Log("Saliendo al menú principal...");
    }

    // Método para verificar logros después del Game Over
    public void CheckGameOverAchievements()
    {
        OnGameOverAchievementsCheck?.Invoke();
    }
}
