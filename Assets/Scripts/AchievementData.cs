using UnityEngine;

[CreateAssetMenu(fileName = "AchievementData", menuName = "Game Systems/Achievement Data")]
public class AchievementData : ScriptableObject
{
    // Nombre del logro
    public string achievementName;

    // Descripción del logro
    public string achievementDescription;

    // Puntos requeridos para desbloquear el logro
    public int pointsRequired;

    // Tiempo requerido (en segundos) para desbloquear el logro
    public float timeRequired;

    // Flag para indicar si el logro ya fue desbloqueado
    [HideInInspector]
    public bool isUnlocked = false;

    // Método para comprobar si el logro se desbloquea por puntos
    public bool CheckPointsCondition(int currentPoints)
    {
        return !isUnlocked && currentPoints >= pointsRequired;
    }

    // Método para comprobar si el logro se desbloquea por tiempo
    public bool CheckTimeCondition(float currentTime)
    {
        return !isUnlocked && currentTime >= timeRequired;
    }

    // Método para desbloquear el logro
    public void UnlockAchievement()
    {
        isUnlocked = true;
    }
}
