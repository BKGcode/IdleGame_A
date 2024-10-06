using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    // Referencia al ScriptableObject de tiempo
    public TimeData timeData;

    private void Update()
    {
        // Actualizamos el tiempo transcurrido en cada frame
        timeData.UpdateTime(Time.deltaTime);
    }

    private void OnDestroy()
    {
        // Aseguramos que no queden listeners pendientes al destruir el objeto
        timeData.onTimeChanged.RemoveAllListeners();
    }
}
