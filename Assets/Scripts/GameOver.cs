using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject enemyStatPrefab;
    [SerializeField] private Transform enemyStatsContainer;

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateScoreAndTime();
        UpdateEnemyStatistics();
    }

    private void UpdateScoreAndTime()
    {
        if (scoreText != null && GameManager.Instance != null)
        {
            // Asumimos que GameManager tiene una propiedad Score o un método para obtenerlo
            int score = GetScoreFromGameManager();
            scoreText.text = $"Score: {score}";
        }
        else
        {
            Debug.LogWarning("Score text or GameManager not found.");
        }

        if (timeText != null && GameManager.Instance != null)
        {
            // Asumimos que GameManager tiene una forma de obtener el tiempo de juego
            string time = GetTimeFromGameManager();
            timeText.text = $"Time: {time}";
        }
        else
        {
            Debug.LogWarning("Time text or GameManager not found.");
        }
    }

    private void UpdateEnemyStatistics()
    {
        if (enemyStatsContainer == null)
        {
            Debug.LogError("Enemy stats container is not assigned.");
            return;
        }

        // Limpiar el contenedor de estadísticas de enemigos
        foreach (Transform child in enemyStatsContainer)
        {
            Destroy(child.gameObject);
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found.");
            return;
        }

        Dictionary<EnemyTypeSO, int> destroyedEnemies = GameManager.Instance.GetDestroyedEnemiesCounts();

        foreach (var kvp in destroyedEnemies)
        {
            if (enemyStatPrefab == null)
            {
                Debug.LogError("Enemy stat prefab is not assigned.");
                return;
            }

            GameObject statObject = Instantiate(enemyStatPrefab, enemyStatsContainer);
            Image enemyIcon = statObject.transform.Find("EnemyIcon")?.GetComponent<Image>();
            TextMeshProUGUI enemyCountText = statObject.transform.Find("EnemyCount")?.GetComponent<TextMeshProUGUI>();

            if (enemyIcon != null && enemyCountText != null)
            {
                if (kvp.Key != null && kvp.Key.enemySprite != null)
                {
                    enemyIcon.sprite = kvp.Key.enemySprite;
                }
                else
                {
                    Debug.LogWarning($"Enemy sprite is missing for {kvp.Key?.name ?? "unknown enemy"}.");
                    enemyIcon.color = Color.clear; // Make the icon invisible if sprite is missing
                }

                enemyCountText.text = kvp.Value.ToString();
            }
            else
            {
                Debug.LogWarning("Enemy stat prefab is missing required components.");
            }
        }
    }

    private int GetScoreFromGameManager()
    {
        // Implementa esto de acuerdo a cómo GameManager maneja el score
        // Por ejemplo:
        // return GameManager.Instance.Score;
        // o
        // return GameManager.Instance.GetScore();
        return 0; // Placeholder
    }

    private string GetTimeFromGameManager()
    {
        // Implementa esto de acuerdo a cómo GameManager maneja el tiempo
        // Por ejemplo:
        // return GameManager.Instance.GetPlayTime().ToString("mm:ss");
        // o
        // return GameManager.Instance.FormattedPlayTime;
        return "00:00"; // Placeholder
    }
}