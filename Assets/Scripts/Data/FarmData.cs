// Assets/Scripts/Data/FarmData.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FarmData
{
    // Eventos para notificar cambios en los datos de la granja
    public event Action<int> OnResourcesUpdated;
    public event Action<UpgradeData> OnUpgradeApplied;
    public event Action<int, PlantInstance> OnPlantAdded;

    // Propiedades de la granja
    public int Resources { get; private set; }
    public List<PlantInstance> PlantedPlants { get; private set; }
    public List<string> AppliedUpgrades { get; private set; }

    [Header("Datos de Plantas")]
    public List<PlantData> PlantDataList;

    // Constructor inicial
    public FarmData()
    {
        Resources = 0;
        PlantedPlants = new List<PlantInstance>();
        AppliedUpgrades = new List<string>();
    }

    // Métodos para gestionar la granja
    public void AddResources(int amount)
    {
        Resources += amount;
        OnResourcesUpdated?.Invoke(Resources);
    }

    public void SpendResources(int amount)
    {
        Resources -= amount;
        Resources = Mathf.Max(Resources, 0);
        OnResourcesUpdated?.Invoke(Resources);
    }

    public void PlantSeed(PlantData plantData, float plantedTime)
    {
        PlantInstance newPlant = new PlantInstance
        {
            plantData = plantData,
            plantedTime = plantedTime
        };
        PlantedPlants.Add(newPlant);
        OnPlantAdded?.Invoke(PlantedPlants.Count - 1, newPlant);
    }

    public void HarvestPlant(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= PlantedPlants.Count)
            return;

        PlantInstance plant = PlantedPlants[tileIndex];
        if (plant.IsMature())
        {
            AddResources(plant.plantData.resourcesProduced);
            PlantedPlants.RemoveAt(tileIndex);
            // Opcional: Notificar la cosecha
        }
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (!AppliedUpgrades.Contains(upgrade.upgradeName))
        {
            AppliedUpgrades.Add(upgrade.upgradeName);
            OnUpgradeApplied?.Invoke(upgrade);
        }
    }

    // Clase interna para representar una instancia de planta
    [Serializable]
    public class PlantInstance
    {
        public PlantData plantData;
        public float plantedTime;

        public bool IsMature()
        {
            return Time.time - plantedTime >= plantData.growthTime;
        }
    }

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(List<PlantInstance> plants, int resources, List<string> upgrades)
    {
        PlantedPlants = plants;
        Resources = resources;
        AppliedUpgrades = upgrades;

        OnResourcesUpdated?.Invoke(Resources);
        // Puedes notificar también sobre las plantas plantadas y las mejoras aplicadas
    }
}
