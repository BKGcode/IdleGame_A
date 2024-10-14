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
        if (playerData != null)
        {
            playerData.OnResourcesChanged += UpdatePlayerResourcesUI;
        }
        else
        {
            Debug.LogError("PlayerData no está asignado en ResourcesUI.");
        }

        if (farmData != null)
        {
            farmData.OnResourcesUpdated += UpdateFarmResourcesUI;
        }
        else
        {
            Debug.LogError("FarmData no está asignado en ResourcesUI.");
        }
    }

    private void OnDisable()
    {
        if (playerData != null)
        {
            playerData.OnResourcesChanged -= UpdatePlayerResourcesUI;
        }

        if (farmData != null)
        {
            farmData.OnResourcesUpdated -= UpdateFarmResourcesUI;
        }
    }

    /// <summary>
    /// Actualiza la UI de recursos del jugador con la nueva cantidad de recursos.
    /// </summary>
    /// <param name="newResources">Nueva cantidad de recursos del jugador.</param>
    public void UpdatePlayerResourcesUI(int newResources)
    {
        playerResourcesText.text = $"Recursos Jugador: {newResources}";
    }

    /// <summary>
    /// Actualiza la UI de recursos de la granja con la nueva cantidad de recursos.
    /// </summary>
    /// <param name="newResources">Nueva cantidad de recursos de la granja.</param>
    public void UpdateFarmResourcesUI(int newResources)
    {
        farmResourcesText.text = $"Recursos Granja: {newResources}";
    }
}
