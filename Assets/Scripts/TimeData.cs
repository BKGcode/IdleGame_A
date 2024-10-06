using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TimeData", menuName = "Game Systems/Time Data")]
public class TimeData : ScriptableObject
{
    // Tiempo actual de la partida
    public float currentTime = 0;

    // Evento que se dispara cuando el tiempo cambia
    public UnityEvent onTimeChanged;

    // Método para reiniciar el tiempo
    public void ResetTime()
    {
        currentTime = 0;
        onTimeChanged.Invoke();
    }

    // Método para actualizar el tiempo
    public void UpdateTime(float deltaTime)
    {
        currentTime += deltaTime;
        onTimeChanged.Invoke();
    }
}
