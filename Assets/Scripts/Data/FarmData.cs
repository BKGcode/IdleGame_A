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

    // Estructura para instancias de plantas en los tiles
    [Serializable]
    public class PlantInstance
    {
        public PlantData Plant;
        public float TimePlanted; // Tiempo en que se plantó
        public bool IsHarvested;

        public PlantInstance(PlantData plant, float timePlanted)
        {
            Plant = plant;
            TimePlanted = timePlanted;
            IsHarvested = false;
        }
    }

    // Propiedades de la granja
    public List<PlantInstance> PlantedPlants { get; private set; }
    public int Resources { get; private set; }
    public List<string> AppliedUpgrades { get; private set; } // Almacena nombres de las mejoras aplicadas

    // Constructor inicial
    public FarmData()
    {
        PlantedPlants = new List<PlantInstance>();
        AppliedUpgrades = new List<string>();
        Resources = 0;
    }

    // Métodos para gestionar la granja
    public void PlantSeed(PlantData plant, float currentTime)
    {
        PlantInstance newPlant = new PlantInstance(plant, currentTime);
        PlantedPlants.Add(newPlant);
        OnPlantAdded?.Invoke(PlantedPlants.Count - 1, newPlant);
    }

    public void HarvestPlant(int index)
    {
        if (index < 0 || index >= PlantedPlants.Count) return;

        PlantInstance plant = PlantedPlants[index];
        if (!plant.IsHarvested)
        {
            Resources += plant.Plant.resourcesProduced;
            plant.IsHarvested = true;
            OnResourcesUpdated?.Invoke(Resources);
        }
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        if (!AppliedUpgrades.Contains(upgrade.upgradeName))
        {
            AppliedUpgrades.Add(upgrade.upgradeName);
            Resources -= upgrade.cost;
            OnUpgradeApplied?.Invoke(upgrade);
            OnResourcesUpdated?.Invoke(Resources);
        }
    }

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

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(List<PlantInstance> plants, int resources, List<string> upgrades)
    {
        PlantedPlants = plants;
        Resources = resources;
        AppliedUpgrades = upgrades;

        OnResourcesUpdated?.Invoke(Resources);
    }
}
