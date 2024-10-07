using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public LifeData lifeData; // Datos de vidas
    public GameOverMenu gameOverMenu; // Referencia al GameOverMenu
    private int currentSaveSlot = 0; // Slot de guardado actual

    // Método para inicializar con el slot actual
    public void Initialize(int saveSlot)
    {
        currentSaveSlot = saveSlot; // Establece el slot de guardado actual
    }

    private void Update()
    {
        // Comprueba si las vidas han llegado a cero y activa el GameOverMenu
        if (lifeData.currentLives <= 0 && !gameOverMenu.gameObject.activeSelf)
        {
            TriggerGameOver();
        }
    }

    // Método para reducir vidas
    public void ReduceLife(int amount)
    {
        lifeData.currentLives -= amount;
        lifeData.onLifeLost.Invoke(); // Actualiza la UI de vidas

        CheckGameOver(); // Verifica si las vidas son 0 después de reducir
    }

    // Método para restaurar vidas
    public void RestoreLife(int amount)
    {
        lifeData.currentLives = Mathf.Min(lifeData.currentLives + amount, lifeData.maxLives);
        lifeData.onLifeGained.Invoke(); // Actualiza la UI de vidas
    }

    // Método para verificar si se ha alcanzado el estado de Game Over
    private void CheckGameOver()
    {
        if (lifeData.currentLives <= 0)
        {
            TriggerGameOver();
        }
    }

    // Método que se encarga de gestionar el Game Over
    private void TriggerGameOver()
    {
        gameOverMenu.Initialize(currentSaveSlot); // Inicializa el menú de Game Over con el slot actual
        gameOverMenu.Show(); // Muestra el menú de Game Over
    }
}
