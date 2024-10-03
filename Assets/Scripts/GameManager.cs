using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject gameOverPanel;
    public Image[] heartImages; // Asigna estas imágenes en el Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Audio Settings")]
    public AudioClip gameOverSound;
    private AudioSource audioSource;

    private int maxHearts = 5;
    private int currentHearts;

    private bool isGameOver = false;

    private void Awake()
    {
        // Implementación del patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Obtener o añadir AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        // Inicializar estados
        currentHearts = maxHearts;
        UpdateHeartsUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        else
            Debug.LogError("Game Over Panel no está asignado en el Inspector.");

        if (gameOverSound == null)
            Debug.LogWarning("Game Over Sound no está asignado en el Inspector.");
    }

    #region Heart Management

    public void UpdateHearts(int newHeartCount)
    {
        currentHearts = Mathf.Clamp(newHeartCount, 0, maxHearts);
        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            TriggerGameOver();
        }
    }

    private void UpdateHeartsUI()
    {
        if (heartImages == null || fullHeart == null || emptyHeart == null)
        {
            Debug.LogError("Heart Images o Sprites no están correctamente asignados en el GameManager.");
            return;
        }

        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHearts)
                heartImages[i].sprite = fullHeart;
            else
                heartImages[i].sprite = emptyHeart;
        }
    }

    #endregion

    #region Game Over Management

    public void TriggerGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (audioSource != null && gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound);

        Time.timeScale = 0f; // Pausar el juego
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Reanudar el juego
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que el nombre coincide
    }

    #endregion

    #region Event Subscription

    private void OnEnable()
    {
        // Suscribirse a los eventos
        GameEvents.OnPlayerDamaged += HandlePlayerDamaged;
        GameEvents.OnPlayerHealed += HandlePlayerHealed;
        GameEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos
        GameEvents.OnPlayerDamaged -= HandlePlayerDamaged;
        GameEvents.OnPlayerHealed -= HandlePlayerHealed;
        GameEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    #endregion

    #region Event Handlers

    private void HandlePlayerDamaged(int damage)
    {
        UpdateHearts(currentHearts - damage);
    }

    private void HandlePlayerHealed(int amount)
    {
        UpdateHearts(currentHearts + amount);
    }

    private void HandlePlayerDeath()
    {
        TriggerGameOver();
    }

    #endregion
}
