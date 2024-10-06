using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Referencia al ScriptableObject de puntos
    public ScoreData scoreData;

    // Método para añadir puntos
    public void AddPoints(int points)
    {
        scoreData.AddScore(points);
    }

    private void OnDestroy()
    {
        // Aseguramos que no queden listeners pendientes al destruir el objeto
        scoreData.onScoreChanged.RemoveAllListeners();
    }
}
