using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Game Systems/Score Data")]
public class ScoreData : ScriptableObject
{
    public int currentScore = 0;  // Puntos actuales del jugador

    // Evento que se dispara cuando los puntos cambian
    public UnityEvent onScoreChanged;

    // Método para añadir puntos
    public void AddScore(int amount)
    {
        currentScore += amount;
        onScoreChanged.Invoke();
    }

    // Método para reiniciar los puntos
    public void ResetScore()
    {
        currentScore = 0;
        onScoreChanged.Invoke();
    }
}
