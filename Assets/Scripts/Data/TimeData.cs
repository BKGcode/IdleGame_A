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
    public float SessionTime { get; private set; }
    public float TotalPlayTime { get; private set; }

    // Método para actualizar el tiempo de sesión
    public void IncrementSessionTime(float deltaTime)
    {
        SessionTime += deltaTime;
        OnSessionTimeUpdated?.Invoke(SessionTime);
    }

    // Método para finalizar la sesión
    public void EndSession()
    {
        // Lógica para finalizar la sesión
        // Por ejemplo, añadir SessionTime a TotalPlayTime
        TotalPlayTime += SessionTime;
        SessionTime = 0f;
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
