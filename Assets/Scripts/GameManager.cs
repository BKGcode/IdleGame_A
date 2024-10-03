using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Lives Settings")]
    public int maxLives = 3; // Número máximo de vidas
    private int currentLives;

    [Header("UI Settings")]
    public Image[] heartImages; // Array de imágenes de corazones
    public Sprite fullHeart;    // Sprite del corazón lleno
    public Sprite emptyHeart;   // Sprite del corazón vacío
    public GameObject gameOverPanel; // Panel de Game Over

    // Evento para cuando el jugador muere
    public delegate void OnPlayerDeath();
    public event OnPlayerDeath PlayerDeathEvent;

    private void Awake()
    {
        // Implementar el Patrón Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            InitializeLives();
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
            return;
        }
    }

    private void InitializeLives()
    {
        currentLives = maxLives;
        UpdateHeartsUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Método para asignar el objetivo de la cámara
    public void SetCameraTarget(Transform target)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera no está asignada en el GameManager.");
        }
    }

    // Método para disminuir una vida
    public void DecreaseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateHeartsUI();

            if (currentLives <= 0)
            {
                Die();
            }
        }
    }

    // Método para aumentar una vida (opcional)
    public void IncreaseLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateHeartsUI();
        }
    }

    // Actualizar la UI de los corazones con sprites
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }

    // Método a llamar cuando el jugador muere
    private void Die()
    {
        Debug.Log("Jugador ha muerto");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Invocar el evento de muerte del jugador
        PlayerDeathEvent?.Invoke();

        // Aquí puedes agregar lógica adicional como pausar el juego, reiniciar la escena, etc.
    }

    // Método para resetear las vidas (opcional)
    public void ResetLives()
    {
        currentLives = maxLives;
        UpdateHeartsUI();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
}
