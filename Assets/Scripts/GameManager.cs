using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Cinemachine")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Lives Settings")]
    public int maxLives = 3; // N�mero m�ximo de vidas
    private int currentLives;

    [Header("UI Settings")]
    public Image[] heartImages; // Array de im�genes de corazones
    public Sprite fullHeart;    // Sprite del coraz�n lleno
    public Sprite emptyHeart;   // Sprite del coraz�n vac�o
    public GameObject gameOverPanel; // Panel de Game Over

    // Evento para cuando el jugador muere
    public delegate void OnPlayerDeath();
    public event OnPlayerDeath PlayerDeathEvent;

    private void Awake()
    {
        // Implementar el Patr�n Singleton
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

    // M�todo para asignar el objetivo de la c�mara
    public void SetCameraTarget(Transform target)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = target;
            virtualCamera.LookAt = target;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera no est� asignada en el GameManager.");
        }
    }

    // M�todo para disminuir una vida
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

    // M�todo para aumentar una vida (opcional)
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

    // M�todo a llamar cuando el jugador muere
    private void Die()
    {
        Debug.Log("Jugador ha muerto");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Invocar el evento de muerte del jugador
        PlayerDeathEvent?.Invoke();

        // Aqu� puedes agregar l�gica adicional como pausar el juego, reiniciar la escena, etc.
    }

    // M�todo para resetear las vidas (opcional)
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
