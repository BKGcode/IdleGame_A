// Assets/Scripts/UI/HealthUI.cs
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        if (playerData != null)
        {
            playerData.OnHealthChanged += UpdateHealthUI;
        }
        else
        {
            Debug.LogError("PlayerData no está asignado en HealthUI.");
        }
    }

    private void OnDisable()
    {
        if (playerData != null)
        {
            playerData.OnHealthChanged -= UpdateHealthUI;
        }
    }

    /// <summary>
    /// Actualiza la UI de salud con el nuevo valor de salud.
    /// </summary>
    /// <param name="newHealth">Nueva salud del jugador.</param>
    public void UpdateHealthUI(int newHealth)
    {
        healthText.text = $"Salud: {newHealth}";
    }
}
