// Assets/Scripts/UI/CollectableResourceUI.cs
using UnityEngine;
using TMPro;

public class CollectableResourceUI : MonoBehaviour
{
    [SerializeField] private ShooterData shooterData;
    [SerializeField] private TextMeshProUGUI collectedResourcesText;

    private void OnEnable()
    {
        shooterData.OnResourcesCollected += UpdateCollectedResourcesUI;
    }

    private void OnDisable()
    {
        shooterData.OnResourcesCollected -= UpdateCollectedResourcesUI;
    }

    private void UpdateCollectedResourcesUI(int amount)
    {
        collectedResourcesText.text = $"Recursos Recogidos: {amount}";
    }
}
