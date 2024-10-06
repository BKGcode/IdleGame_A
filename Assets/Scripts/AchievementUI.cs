using UnityEngine;
using TMPro;

public class AchievementUI : MonoBehaviour
{
    // Referencia al TextMeshPro para mostrar el logro desbloqueado
    public TextMeshProUGUI achievementText;

    // Método para mostrar un logro desbloqueado
    public void ShowAchievement(string achievementName)
    {
        achievementText.text = "Achievement Unlocked: " + achievementName;
        // Aquí puedes añadir animaciones o efectos de sonido
        // Desaparece el mensaje tras unos segundos
        Invoke("HideAchievement", 3f);
    }

    // Método para ocultar el texto
    private void HideAchievement()
    {
        achievementText.text = "";
    }
}
