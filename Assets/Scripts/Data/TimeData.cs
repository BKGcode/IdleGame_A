// Assets/Scripts/Data/TimeData.cs
using System;
using UnityEngine;

[Serializable]
public class TimeData
{
    // Eventos para notificar cambios en el tiempo
    public event Action<float> OnSessionTimeUpdated;
    public event Action<float> OnTotalPlayTimeUpdated;

    // Propiedades de tiempo
    public float SessionTime { get; private set; } // Tiempo de la partida actual en segundos
    public float TotalPlayTime { get; private set; } // Tiempo total jugado en segundos

    // Constructor inicial
    public TimeData()
    {
        SessionTime = 0f;
        TotalPlayTime = 0f;
    }

    // Métodos para gestionar el tiempo
    public void IncrementSessionTime(float deltaTime)
    {
        SessionTime += deltaTime;
        OnSessionTimeUpdated?.Invoke(SessionTime);
    }

    public void EndSession()
    {
        TotalPlayTime += SessionTime;
        OnTotalPlayTimeUpdated?.Invoke(TotalPlayTime);
        SessionTime = 0f;
        OnSessionTimeUpdated?.Invoke(SessionTime);
    }

    public void AddPlayTime(float time)
    {
        TotalPlayTime += time;
        OnTotalPlayTimeUpdated?.Invoke(TotalPlayTime);
    }

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(float sessionTime, float totalPlayTime)
    {
        SessionTime = sessionTime;
        TotalPlayTime = totalPlayTime;

        OnSessionTimeUpdated?.Invoke(SessionTime);
        OnTotalPlayTimeUpdated?.Invoke(TotalPlayTime);
    }
}
