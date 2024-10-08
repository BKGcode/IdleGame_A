using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Tooltip("Nombre de la escena que se cargará al iniciar un nuevo juego.")]
    public string newGameSceneName;

    [Tooltip("Prefab del jugador para instanciar al iniciar una nueva partida.")]
    [SerializeField] private GameObject playerPrefab;

    /// <summary>
    /// Método para cargar la escena del juego, se asigna al botón de nuevo juego.
    /// </summary>
    public void StartNewGame()
    {
        if (!string.IsNullOrEmpty(newGameSceneName))
        {
            // Limpia los datos de la partida antes de cargar la nueva escena.
            ResetGameData();
            SceneManager.LoadScene(newGameSceneName);
        }
        else
        {
            Debug.LogError("El nombre de la escena de nuevo juego no está asignado.");
        }
    }

    /// <summary>
    /// Método para reiniciar los datos de la partida.
    /// </summary>
    private void ResetGameData()
    {
        PlayerPrefs.DeleteAll(); // Limpiar datos temporales de la partida.
        // Llama a otros métodos de reinicio si es necesario.
    }

    private void OnLevelWasLoaded(int level)
    {
        // Asegúrate de instanciar al Player si es necesario.
        if (playerPrefab != null && GameObject.FindWithTag("Player") == null)
        {
            Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
