using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public TimeData timeData; // Datos del tiempo

    private void Update()
    {
        // Solo incrementa el tiempo si el juego no está en pausa ni en Game Over
        if (timeData != null && !timeData.isPaused)
        {
            timeData.currentTime += Time.deltaTime;
            timeData.onTimeChanged.Invoke(); // Invoca el evento para actualizar la UI
        }
    }

    // Método para reiniciar el tiempo a cero
    public void ResetTime()
    {
        timeData.currentTime = 0f;
        timeData.onTimeChanged.Invoke(); // Actualiza la UI del tiempo
    }

    // Pausar el tiempo
    public void PauseTime()
    {
        timeData.isPaused = true;
    }

    // Reanudar el tiempo
    public void ResumeTime()
    {
        timeData.isPaused = false;
    }
}
