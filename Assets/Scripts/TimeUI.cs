using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    // Referencia al componente TextMeshPro para mostrar el tiempo
    public TextMeshProUGUI timeText;

    // Referencia al ScriptableObject de tiempo
    public TimeData timeData;

    private void Start()
    {
        // Actualizamos la UI al inicio
        UpdateTimeUI();

        // Nos suscribimos al evento para actualizar la UI cuando cambie el tiempo
        timeData.onTimeChanged.AddListener(UpdateTimeUI);
    }

    private void UpdateTimeUI()
    {
        // Convertimos el tiempo actual a minutos y segundos
        int minutes = Mathf.FloorToInt(timeData.currentTime / 60);
        int seconds = Mathf.FloorToInt(timeData.currentTime % 60);

        // Mostramos el tiempo en formato MM:SS
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnDestroy()
    {
        // Removemos los listeners para evitar errores al destruir el objeto
        timeData.onTimeChanged.RemoveListener(UpdateTimeUI);
    }
}
