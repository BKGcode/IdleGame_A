using UnityEngine;
using System;

public class LifeSystem : MonoBehaviour
{
    public int maxLives = 3; // Vidas máximas
    public int currentLives; // Vidas actuales

    public event Action OnLifeLost; // Evento cuando se pierde una vida
    public event Action OnLifeGained; // Evento cuando se gana una vida
    public event Action OnGameOver; // Evento cuando el jugador pierde todas las vidas

    public UIManager uiManager; // Referencia al sistema de UI para actualizar corazones

    private void Start()
    {
        currentLives = maxLives; // Inicializar las vidas
        uiManager.UpdateLivesUI(currentLives); // Actualizar la UI
    }

    // Método para perder vidas
    public void LoseLife(int damage)
    {
        currentLives -= damage;
        OnLifeLost?.Invoke(); // Disparar el evento de pérdida de vida
        uiManager.UpdateLivesUI(currentLives); // Actualizar UI

        if (currentLives <= 0)
        {
            OnGameOver?.Invoke(); // Disparar el evento de Game Over
        }
    }

    // Método para ganar vidas
    public void GainLife(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives); // Limitar el número de vidas al máximo
        OnLifeGained?.Invoke(); // Disparar el evento de ganancia de vida
        uiManager.UpdateLivesUI(currentLives); // Actualizar UI
    }
}
