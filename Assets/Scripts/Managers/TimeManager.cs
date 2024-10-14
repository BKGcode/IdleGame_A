// Assets/Scripts/UI/TimeManager.cs
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TimeData timeData;

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        timeData.IncrementSessionTime(deltaTime);
    }

    private void OnApplicationQuit()
    {
        timeData.EndSession();
    }
}
