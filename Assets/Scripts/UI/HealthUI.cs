// Assets/Scripts/UI/HealthUI.cs
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI healthText;

    private void OnEnable()
    {
        playerData.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        playerData.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int newHealth)
    {
        healthText.text = $"Salud: {newHealth}";
    }
}
