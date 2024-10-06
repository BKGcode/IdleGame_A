using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public UpgradeData[] availableUpgrades;  // Mejores disponibles
    public ScoreData scoreData;  // Datos de los puntos
    public LifeData lifeData;  // Datos de las vidas

    // Método para comprar una mejora
    public bool PurchaseUpgrade(UpgradeData upgrade)
    {
        if (scoreData.currentScore >= upgrade.cost)
        {
            ApplyUpgrade(upgrade);
            scoreData.currentScore -= upgrade.cost;
            scoreData.onScoreChanged.Invoke();  // Actualizamos la UI de los puntos
            return true;
        }
        return false;
    }

    // Método para aplicar una mejora
    private void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade.additionalLives > 0)
        {
            lifeData.currentLives += upgrade.additionalLives;
            lifeData.onLifeGained.Invoke();  // Actualizamos la UI de las vidas
        }
        // Aplica otros efectos como aumento de velocidad, puntos extra, etc.
    }
}
