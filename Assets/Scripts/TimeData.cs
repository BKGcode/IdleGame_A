using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "TimeData", menuName = "Game/TimeData")]
public class TimeData : ScriptableObject
{
    public float currentTime; // Tiempo actual de la partida
    public UnityEvent onTimeChanged; // Evento para actualizar la UI del tiempo
    public bool isPaused; // Indica si el tiempo está pausado
}
