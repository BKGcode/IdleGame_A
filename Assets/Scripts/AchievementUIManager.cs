using UnityEngine;
using System.Collections.Generic;

public class AchievementUIManager : MonoBehaviour
{
    [Header("Achievements")]
    [SerializeField] private List<Achievement> achievements; // Lista de logros disponibles

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOverAchievementsCheck += CheckAchievementsOnGameOver;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOverAchievementsCheck -= CheckAchievementsOnGameOver;
        }
    }

    private void CheckAchievementsOnGameOver()
    {
        foreach (var achievement in achievements)
        {
            achievement.CheckCondition(GameManager.Instance);
        }
    }
}
