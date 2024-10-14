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

    /// <summary>
    /// Actualiza la UI del tiempo de sesión.
    /// </summary>
    /// <param name="newSessionTime">Nuevo tiempo de sesión.</param>
    public void UpdateSessionTimeUI(float newSessionTime)
    {
        sessionTimeText.text = $"Tiempo de Sesión: {FormatTime(newSessionTime)}";
    }

    /// <summary>
    /// Actualiza la UI del tiempo total de juego.
    /// </summary>
    /// <param name="newTotalPlayTime">Nuevo tiempo total de juego.</param>
    public void UpdateTotalPlayTimeUI(float newTotalPlayTime)
    {
        totalPlayTimeText.text = $"Tiempo Total: {FormatTime(newTotalPlayTime)}";
    }

    /// <summary>
    /// Formatea el tiempo en horas, minutos y segundos.
    /// </summary>
    /// <param name="time">Tiempo en segundos.</param>
    /// <returns>Cadena formateada.</returns>
    private string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }
}
