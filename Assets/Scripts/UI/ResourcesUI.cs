// Assets/Scripts/UI/ResourcesUI.cs
using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private FarmData farmData;
    [SerializeField] private TextMeshProUGUI playerResourcesText;
    [SerializeField] private TextMeshProUGUI farmResourcesText;

    private void OnEnable()
    {
        playerData.OnResourcesChanged += UpdatePlayerResourcesUI;
        farmData.OnResourcesUpdated += UpdateFarmResourcesUI;
    }

    private void OnDisable()
    {
        playerData.OnResourcesChanged -= UpdatePlayerResourcesUI;
        farmData.OnResourcesUpdated -= UpdateFarmResourcesUI;
    }

    private void UpdatePlayerResourcesUI(int newResources)
    {
        playerResourcesText.text = $"Recursos Jugador: {newResources}";
    }

    private void UpdateFarmResourcesUI(int newResources)
    {
        farmResourcesText.text = $"Recursos Granja: {newResources}";
    }
}
