using UnityEngine;
using TMPro;
using System;

public class TimeSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    private float elapsedTime;
    private bool isRunning = true;

    private void Start()
    {
        if (timeText == null)
        {
            Debug.LogError("TimeSystem: TMP_Text component is not assigned!");
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    private void UpdateTimeDisplay()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimeDisplay();
    }

    public TimeSpan GetElapsedTime()
    {
        return TimeSpan.FromSeconds(elapsedTime);
    }
}