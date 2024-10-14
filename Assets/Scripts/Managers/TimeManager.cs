// Assets/Scripts/Managers/TimeManager.cs
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TimeData timeData;

    private void Update()
    {
        // Incrementa el tiempo de sesión cada frame
        if (timeData != null)
        {
            timeData.IncrementSessionTime(Time.deltaTime);
        }
        else
        {
            Debug.LogError("TimeData no está asignado en TimeManager.");
        }
    }

    private void OnApplicationQuit()
    {
        // Finaliza la sesión cuando la aplicación se cierra
        if (timeData != null)
        {
            timeData.EndSession();
        }
        else
        {
            Debug.LogError("TimeData no está asignado en TimeManager.");
        }
    }
}
