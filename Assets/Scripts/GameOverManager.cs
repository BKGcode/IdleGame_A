using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Enemy Statistics")]
    [SerializeField] private Transform enemyStatsContainer;
    [SerializeField] private GameObject enemyStatPrefab;

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            DisplayEnemyStatistics();
        }
    }

    private void DisplayEnemyStatistics()
    {
        if (enemyStatsContainer == null || enemyStatPrefab == null || GameManager.Instance == null)
        {
            Debug.LogWarning("Missing references for displaying enemy statistics.");
            return;
        }

        // Clear existing stats
        foreach (Transform child in enemyStatsContainer)
        {
            Destroy(child.gameObject);
        }

        Dictionary<EnemyTypeSO, int> destroyedEnemies = GameManager.Instance.GetDestroyedEnemiesCounts();

        foreach (var kvp in destroyedEnemies)
        {
            GameObject statObject = Instantiate(enemyStatPrefab, enemyStatsContainer);
            Image enemyIcon = statObject.transform.Find("EnemyIcon").GetComponent<Image>();
            TextMeshProUGUI enemyCountText = statObject.transform.Find("EnemyCount").GetComponent<TextMeshProUGUI>();

            if (enemyIcon != null && enemyCountText != null)
            {
                enemyIcon.sprite = kvp.Key.enemySprite;
                enemyCountText.text = kvp.Value.ToString();
            }
            else
            {
                Debug.LogWarning("Enemy stat prefab is missing required components.");
            }
        }
    }

    private void ReturnToMainMenu()
    {
        CleanupAndReset();
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void RestartGame()
    {
        CleanupAndReset();
        SceneManager.LoadScene(gameSceneName);
    }

    private void CleanupAndReset()
    {
        Time.timeScale = 1f;

        // Destruir todos los objetos marcados con DontDestroyOnLoad
        GameObject[] persistentObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in persistentObjects)
        {
            if (obj.scene.name == "DontDestroyOnLoad")
            {
                Destroy(obj);
            }
        }

        // Resetear sistemas específicos
        ResetCurrencyManager();
        ResetTimeSystem();
        ResetBusinessesAndManagers();
        ResetObjectPool();
        ResetGameManager();

        // Aquí puedes añadir más resets para otros sistemas como vidas, armas, etc.
    }

    private void ResetCurrencyManager()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetCurrency();
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance no encontrado al intentar resetear la moneda.");
        }
    }

    private void ResetTimeSystem()
    {
        TimeSystem timeSystem = FindObjectOfType<TimeSystem>();
        if (timeSystem != null)
        {
            timeSystem.ResetTimer();
        }
        else
        {
            Debug.LogWarning("TimeSystem no encontrado al intentar resetear el tiempo.");
        }
    }

    private void ResetBusinessesAndManagers()
    {
        Business[] businesses = FindObjectsOfType<Business>();
        foreach (Business business in businesses)
        {
            business.SetHired(false);
        }

        Manager[] managers = FindObjectsOfType<Manager>();
        foreach (Manager manager in managers)
        {
            manager.SetHired(false);
        }
    }

    private void ResetObjectPool()
    {
        ObjectPool objectPool = FindObjectOfType<ObjectPool>();
        if (objectPool != null)
        {
            objectPool.ResetPool();
        }
        else
        {
            Debug.LogWarning("ObjectPool no encontrado al intentar resetear el pool de objetos.");
        }
    }

    private void ResetGameManager()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameManager();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance no encontrado al intentar resetear.");
        }
    }
}