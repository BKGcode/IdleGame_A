using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private UIManager uiManager;

    private void Start()
    {
        currentLives = maxLives;
        uiManager = FindObjectOfType<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("No se encontró UIManager en la escena.");
        }
        else
        {
            uiManager.UpdateLivesUI(currentLives);
        }
    }

    // Método para perder una vida
    public void LoseLife(int damageAmount)
    {
        currentLives -= damageAmount;
        if (currentLives < 0)
        {
            currentLives = 0;
        }

        if (uiManager != null)
        {
            uiManager.UpdateLivesUI(currentLives);
        }

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    // Método para ganar una vida
    public void GainLife(int amount)
    {
        currentLives += amount;
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        if (uiManager != null)
        {
            uiManager.UpdateLivesUI(currentLives);
        }
    }

    private void GameOver()
    {
        // Lógica para el fin del juego
        GameManager.Instance.GameOver();
    }
}
