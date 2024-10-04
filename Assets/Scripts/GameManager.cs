using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int points; // Puntos del jugador en la partida actual
    private float timePlayed; // Tiempo jugado en la partida actual
    public LifeSystem lifeSystem; // Referencia al sistema de vidas
    public PopupManager popupManager; // Referencia al popup manager

    private void Start()
    {
        // Suscribirse al evento de Game Over del sistema de vidas
        lifeSystem.OnGameOver += HandleGameOver;
    }

    private void Update()
    {
        timePlayed += Time.deltaTime; // Actualizar el tiempo jugado
    }

    // Llamar este m�todo cuando el jugador muera o la partida termine
    public void EndGame()
    {
        GameData data = new GameData(points, timePlayed); // Crear los datos de la partida
        SaveSystem.SaveGame(data); // Guardar la partida

        popupManager.ShowGameOverPopup(); // Mostrar el popup de "Game Over"
    }

    // M�todo para reiniciar el juego
    public void RestartGame()
    {
        // Reiniciar puntos y tiempo jugado
        points = 0;
        timePlayed = 0;

        // Aqu� puedes agregar l�gica adicional para reiniciar la escena
        // Ejemplo: UnityEngine.SceneManagement.SceneManager.LoadScene("YourGameScene");
    }

    // M�todo para manejar el Game Over
    private void HandleGameOver()
    {
        EndGame(); // Llamar al final de la partida cuando el jugador pierda todas las vidas
    }

    // Obtener los puntos actuales (para el popup)
    public int GetPoints()
    {
        return points;
    }

    // Obtener el tiempo jugado (para el popup)
    public float GetTimePlayed()
    {
        return timePlayed;
    }

    // Salir al men� principal
    public void ExitToMainMenu()
    {
        // Aqu� puedes agregar la l�gica para volver al men� principal
        // Ejemplo: UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento para evitar errores
        lifeSystem.OnGameOver -= HandleGameOver;
    }
}
