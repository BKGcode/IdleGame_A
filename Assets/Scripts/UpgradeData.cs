using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "Game Systems/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public int cost;  // Costo de la mejora en puntos
    public int additionalLives;  // Vidas adicionales que otorga
    public float speedBoost;  // Incremento de velocidad
    public int extraPointsPerKill;  // Puntos extra por enemigo derrotado
}
