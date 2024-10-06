using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    // Array de logros a gestionar
    public AchievementData[] achievements;

    // Referencias a los sistemas de puntos y tiempo
    public ScoreData scoreData;
    public TimeData timeData;

    private void Start()
    {
        // Nos suscribimos a los eventos de puntos y tiempo
        scoreData.onScoreChanged.AddListener(CheckAchievementsByPoints);
        timeData.onTimeChanged.AddListener(CheckAchievementsByTime);
    }

    // Método para comprobar los logros basados en puntos
    private void CheckAchievementsByPoints()
    {
        foreach (var achievement in achievements)
        {
            if (achievement.CheckPointsCondition(scoreData.currentScore))
            {
                UnlockAchievement(achievement);
            }
        }
    }

    // Método para comprobar los logros basados en tiempo
    private void CheckAchievementsByTime()
    {
        foreach (var achievement in achievements)
        {
            if (achievement.CheckTimeCondition(timeData.currentTime))
            {
                UnlockAchievement(achievement);
            }
        }
    }

    // Método para desbloquear un logro
    private void UnlockAchievement(AchievementData achievement)
    {
        achievement.UnlockAchievement();
        Debug.Log("Achievement Unlocked: " + achievement.achievementName);
        // Aquí podrías disparar un evento para mostrar un popup en la UI, sonidos, etc.
    }

    private void OnDestroy()
    {
        // Remover listeners al destruir este objeto
        scoreData.onScoreChanged.RemoveListener(CheckAchievementsByPoints);
        timeData.onTimeChanged.RemoveListener(CheckAchievementsByTime);
    }
}
