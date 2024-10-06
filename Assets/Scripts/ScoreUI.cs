using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    // Referencia al componente TextMeshPro para mostrar los puntos
    public TextMeshProUGUI scoreText;

    // Referencia al ScriptableObject de puntos
    public ScoreData scoreData;

    private void Start()
    {
        // Actualizamos la UI al inicio
        UpdateScoreUI();

        // Nos suscribimos al evento para actualizar la UI cuando cambien los puntos
        scoreData.onScoreChanged.AddListener(UpdateScoreUI);
    }

    private void UpdateScoreUI()
    {
        // Mostramos los puntos actuales sin añadir texto en el código
        scoreText.text = scoreData.currentScore.ToString();
    }

    private void OnDestroy()
    {
        // Eliminamos los listeners al destruir el objeto
        scoreData.onScoreChanged.RemoveListener(UpdateScoreUI);
    }
}
