// Assets/Scripts/UI/TimeUI.cs
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TimeData timeData;
    [SerializeField] private TextMeshProUGUI sessionTimeText;
    [SerializeField] private TextMeshProUGUI totalPlayTimeText;

    private void OnEnable()
    {
        timeData.OnSessionTimeUpdated += UpdateSessionTimeUI;
        timeData.OnTotalPlayTimeUpdated += UpdateTotalPlayTimeUI;
    }

    private void OnDisable()
    {
        timeData.OnSessionTimeUpdated -= UpdateSessionTimeUI;
        timeData.OnTotalPlayTimeUpdated -= UpdateTotalPlayTimeUI;
    }

    private void UpdateSessionTimeUI(float newSessionTime)
    {
        sessionTimeText.text = $"Tiempo de Sesión: {FormatTime(newSessionTime)}";
    }

    private void UpdateTotalPlayTimeUI(float newTotalPlayTime)
    {
        totalPlayTimeText.text = $"Tiempo Total: {FormatTime(newTotalPlayTime)}";
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds - minutes * 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
