using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestiona las vidas del jugador, actualiza la interfaz de usuario (UI),
/// maneja las colisiones con enemigos y reproduce efectos al recibir daño.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    [SerializeField] private int maxLives = 3; // Número máximo de vidas del jugador
    private int currentLives; // Vidas actuales del jugador

    [Header("UI de Vidas")]
    [Tooltip("Arreglo de imágenes que representan las vidas del jugador (corazones).")]
    [SerializeField] private Image[] lifeImages; // Imágenes de los corazones en la UI
    [Tooltip("Sprite que representa un corazón lleno.")]
    [SerializeField] private Sprite fullHeartSprite; // Sprite de corazón lleno
    [Tooltip("Sprite que representa un corazón vacío.")]
    [SerializeField] private Sprite emptyHeartSprite; // Sprite de corazón vacío

    [Header("Efectos de Daño")]
    [Tooltip("Sonido que se reproduce al recibir daño.")]
    [SerializeField] private AudioClip damageSound; // Clip de sonido al recibir daño
    [Tooltip("Prefab de partículas que se instancian al recibir daño.")]
    [SerializeField] private ParticleSystem damageEffectPrefab; // Prefab de partículas al recibir daño

    [Header("Componentes")]
    [Tooltip("Referencia al componente AudioSource del jugador.")]
    [SerializeField] private AudioSource audioSource; // AudioSource para reproducir sonidos

    [Header("Game Over Manager")]
    [SerializeField] private GameOverManager gameOverManager; // Referencia al GameOverManager

    private Rigidbody rb; // Referencia al Rigidbody del Player

    private void Awake()
    {
        // Asignar el AudioSource si no está asignado en el Inspector
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No se encontró un componente AudioSource en el jugador.");
            }
        }

        // Obtener el Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No se encontró un componente Rigidbody en el jugador.");
        }

        // Inicializar las vidas del jugador
        currentLives = maxLives;
        UpdateLifeUI();
    }

    /// <summary>
    /// Maneja las colisiones con otros objetos.
    /// </summary>
    /// <param name="collision">Información de la colisión.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto colisionado tiene la etiqueta "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Reducir una vida
            TakeDamage();

            // Reproducir efectos de daño
            PlayDamageEffects();

            // Destruir al enemigo que impactó
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Reduce una vida al jugador y actualiza la UI.
    /// </summary>
    public void TakeDamage()
    {
        if (currentLives > 0)
        {
            currentLives--;
            UpdateLifeUI();

            // Manejar la muerte del jugador si las vidas llegan a cero
            if (currentLives <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// Actualiza la interfaz de usuario para reflejar las vidas actuales.
    /// </summary>
    private void UpdateLifeUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < currentLives)
            {
                lifeImages[i].sprite = fullHeartSprite; // Corazón lleno
            }
            else
            {
                lifeImages[i].sprite = emptyHeartSprite; // Corazón vacío
            }

            // Deshabilitar imágenes adicionales si hay más imágenes que vidas
            lifeImages[i].enabled = i < maxLives;
        }
    }

    /// <summary>
    /// Reproduce el sonido y las partículas de daño.
    /// </summary>
    private void PlayDamageEffects()
    {
        // Reproducir sonido de daño
        if (damageSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        // Instanciar efecto de partículas de daño
        if (damageEffectPrefab != null)
        {
            Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        }

        // Resetear la velocidad para evitar inercia
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador mostrando la pantalla de Game Over.
    /// </summary>
    private void Die()
    {
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogError("GameOverManager no está asignado en PlayerHealth.");
        }

        // Pausar el juego
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Añade una vida al jugador y actualiza la UI.
    /// </summary>
    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateLifeUI();
        }
    }
}
