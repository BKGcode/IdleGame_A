using UnityEngine;
using TMPro;

public class LifeUI : MonoBehaviour
{
    // Referencia al TextMeshPro para mostrar las vidas
    public TextMeshProUGUI lifeText;

    // Referencia al ScriptableObject de vidas
    public LifeData lifeData;

    private void Start()
    {
        // Actualizamos la UI inicialmente
        UpdateLifeUI();

        // Nos suscribimos al evento para actualizar la UI cuando cambien las vidas
        lifeData.onLifeLost.AddListener(UpdateLifeUI);
        lifeData.onLifeGained.AddListener(UpdateLifeUI);
    }

    private void UpdateLifeUI()
    {
        // Mostramos las vidas actuales en la UI
        lifeText.text = lifeData.currentLives.ToString();
    }

    private void OnDestroy()
    {
        // Eliminamos los listeners al destruir el objeto
        lifeData.onLifeLost.RemoveListener(UpdateLifeUI);
        lifeData.onLifeGained.RemoveListener(UpdateLifeUI);
    }
}
