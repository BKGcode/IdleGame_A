using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LifeData", menuName = "Game Systems/Life Data")]
public class LifeData : ScriptableObject
{
    public int maxLives = 3;  // Máximo de vidas posibles
    public int currentLives;  // Vidas actuales

    // Eventos que se disparan cuando se gana o pierde una vida
    public UnityEvent onLifeLost;
    public UnityEvent onLifeGained;

    private void OnEnable()
    {
        // Inicializamos las vidas actuales con el máximo al iniciar
        currentLives = maxLives;
    }

    // Método para perder una vida (siempre pierde solo una vida)
    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            onLifeLost.Invoke();  // Disparar el evento de pérdida de vida
        }
    }

    // Método para ganar una vida (cuando sea posible aumentar las vidas)
    public void GainLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            onLifeGained.Invoke();  // Disparar el evento de ganancia de vida
        }
    }

    // Método para reiniciar las vidas (al comenzar una nueva partida)
    public void ResetLives()
    {
        currentLives = maxLives;
    }
}
