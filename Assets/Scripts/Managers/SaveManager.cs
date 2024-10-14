// Assets/Scripts/Managers/SaveManager.cs
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("Datos a Guardar")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private FarmData farmData;
    [SerializeField] private ShooterData shooterData;
    [SerializeField] private ProgressionData progressionData;
    [SerializeField] private TimeData timeData;

    private string saveFilePath;

    // Variables para almacenar las suscripciones
    private Action<int> onResourcesChangedHandler;
    private Action<int> onHealthChangedHandler;
    private Action<WeaponData> onWeaponChangedHandler;
    private Action<int> onAmmoChangedHandler;

    private Action<int> onFarmResourcesUpdatedHandler;
    private Action<UpgradeData> onFarmUpgradeAppliedHandler;
    private Action<int, FarmData.PlantInstance> onFarmPlantAddedHandler;

    private Action<int> onShooterLevelChangedHandler;
    private Action<WeaponData> onShooterWeaponUnlockedHandler;
    private Action<string> onShooterPowerUpActivatedHandler;
    private Action<int> onShooterResourcesCollectedHandler;

    private Action<UpgradeData> onProgressionUpdatedHandler;

    private Action<float> onTimeSessionUpdatedHandler;
    private Action<float> onTimeTotalPlayTimeUpdatedHandler;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    private void OnEnable()
    {
        // Definir los handlers
        onResourcesChangedHandler = (int _) => SaveGame();
        onHealthChangedHandler = (int _) => SaveGame();
        onWeaponChangedHandler = (WeaponData _) => SaveGame();
        onAmmoChangedHandler = (int _) => SaveGame();

        onFarmResourcesUpdatedHandler = (int _) => SaveGame();
        onFarmUpgradeAppliedHandler = (UpgradeData _) => SaveGame();
        onFarmPlantAddedHandler = (int _, FarmData.PlantInstance _) => SaveGame();

        onShooterLevelChangedHandler = (int _) => SaveGame();
        onShooterWeaponUnlockedHandler = (WeaponData _) => SaveGame();
        onShooterPowerUpActivatedHandler = (string _) => SaveGame();
        onShooterResourcesCollectedHandler = (int _) => SaveGame();

        onProgressionUpdatedHandler = (UpgradeData _) => SaveGame();

        onTimeSessionUpdatedHandler = (float _) => SaveGame();
        onTimeTotalPlayTimeUpdatedHandler = (float _) => SaveGame();

        // Suscribirse a eventos clave para guardar automáticamente
        playerData.OnResourcesChanged += onResourcesChangedHandler;
        playerData.OnHealthChanged += onHealthChangedHandler;
        playerData.OnWeaponChanged += onWeaponChangedHandler;
        playerData.OnAmmoChanged += onAmmoChangedHandler;

        farmData.OnResourcesUpdated += onFarmResourcesUpdatedHandler;
        farmData.OnUpgradeApplied += onFarmUpgradeAppliedHandler;
        farmData.OnPlantAdded += onFarmPlantAddedHandler;

        shooterData.OnLevelChanged += onShooterLevelChangedHandler;
        shooterData.OnWeaponUnlocked += onShooterWeaponUnlockedHandler;
        shooterData.OnPowerUpActivated += onShooterPowerUpActivatedHandler;
        shooterData.OnResourcesCollected += onShooterResourcesCollectedHandler;

        progressionData.OnProgressionUpdated += onProgressionUpdatedHandler;

        timeData.OnSessionTimeUpdated += onTimeSessionUpdatedHandler;
        timeData.OnTotalPlayTimeUpdated += onTimeTotalPlayTimeUpdatedHandler;
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos
        playerData.OnResourcesChanged -= onResourcesChangedHandler;
        playerData.OnHealthChanged -= onHealthChangedHandler;
        playerData.OnWeaponChanged -= onWeaponChangedHandler;
        playerData.OnAmmoChanged -= onAmmoChangedHandler;

        farmData.OnResourcesUpdated -= onFarmResourcesUpdatedHandler;
        farmData.OnUpgradeApplied -= onFarmUpgradeAppliedHandler;
        farmData.OnPlantAdded -= onFarmPlantAddedHandler;

        shooterData.OnLevelChanged -= onShooterLevelChangedHandler;
        shooterData.OnWeaponUnlocked -= onShooterWeaponUnlockedHandler;
        shooterData.OnPowerUpActivated -= onShooterPowerUpActivatedHandler;
        shooterData.OnResourcesCollected -= onShooterResourcesCollectedHandler;

        progressionData.OnProgressionUpdated -= onProgressionUpdatedHandler;

        timeData.OnSessionTimeUpdated -= onTimeSessionUpdatedHandler;
        timeData.OnTotalPlayTimeUpdated -= onTimeTotalPlayTimeUpdatedHandler;
    }

    // Método para guardar el juego
    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            // PlayerData
            playerHealth = playerData.Health,
            playerWeapon = playerData.CurrentWeapon != null ? playerData.CurrentWeapon.name : "",
            playerAmmo = playerData.Ammo,
            playerResources = playerData.Resources,

            // FarmData
            farmResources = farmData.Resources,
            farmPlantedPlants = farmData.PlantedPlants, // Asegúrate de que PlantInstance sea serializable
            farmUpgrades = farmData.AppliedUpgrades,

            // ShooterData
            shooterLevel = shooterData.CurrentLevel,
            shooterWeapons = shooterData.AvailableWeapons.ConvertAll(w => w.name),
            shooterPowerUps = shooterData.ActivePowerUps,

            // ProgressionData
            progressionFarmingEfficiency = progressionData.FarmingEfficiency,
            progressionFarmingArea = progressionData.FarmingArea,
            progressionShooterEfficiency = progressionData.ShooterEfficiency,
            progressionShooterCapacity = progressionData.ShooterCapacity,
            progressionDifficulty = progressionData.CurrentDifficulty.ToString(),

            // TimeData
            sessionTime = timeData.SessionTime,
            totalPlayTime = timeData.TotalPlayTime
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Juego Guardado en: " + saveFilePath);
    }

    // Método para cargar el juego
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // PlayerData
            WeaponData loadedWeapon = GetWeaponByName(data.playerWeapon);
            playerData.SetData(data.playerHealth, loadedWeapon, data.playerAmmo, data.playerResources);

            // FarmData
            List<FarmData.PlantInstance> loadedPlants = data.farmPlantedPlants;
            List<string> loadedUpgrades = data.farmUpgrades;
            farmData.SetData(loadedPlants, data.farmResources, loadedUpgrades);

            // ShooterData
            List<WeaponData> loadedShooterWeapons = GetWeaponsByNames(data.shooterWeapons);
            List<string> loadedPowerUps = data.shooterPowerUps;
            shooterData.SetData(data.shooterLevel, loadedShooterWeapons, loadedPowerUps);

            // ProgressionData
            ProgressionData.DifficultyLevel loadedDifficulty = GetDifficultyLevelByName(data.progressionDifficulty);
            progressionData.SetData(data.progressionFarmingEfficiency, data.progressionFarmingArea,
                                    data.progressionShooterEfficiency, data.progressionShooterCapacity,
                                    loadedDifficulty);

            // TimeData
            timeData.SetData(data.sessionTime, data.totalPlayTime);

            Debug.Log("Juego Cargado desde: " + saveFilePath);
        }
        else
        {
            Debug.LogWarning("No se encontró ningún archivo de guardado.");
        }
    }

    // Métodos auxiliares para obtener objetos por nombre
    private WeaponData GetWeaponByName(string weaponName)
    {
        WeaponData[] weapons = Resources.LoadAll<WeaponData>("ScriptableObjects/WeaponData");
        foreach (var weapon in weapons)
        {
            if (weapon.name == weaponName)
                return weapon;
        }
        return null;
    }

    private List<WeaponData> GetWeaponsByNames(List<string> weaponNames)
    {
        List<WeaponData> weapons = new List<WeaponData>();
        foreach (var name in weaponNames)
        {
            WeaponData weapon = GetWeaponByName(name);
            if (weapon != null)
                weapons.Add(weapon);
        }
        return weapons;
    }

    private List<string> GetUpgradesByNames(List<string> upgradeNames)
    {
        List<string> upgrades = new List<string>();
        UpgradeData[] allUpgrades = Resources.LoadAll<UpgradeData>("ScriptableObjects/UpgradeData");
        foreach (var name in upgradeNames)
        {
            foreach (var upgrade in allUpgrades)
            {
                if (upgrade.name == name)
                {
                    upgrades.Add(upgrade.name);
                    break;
                }
            }
        }
        return upgrades;
    }

    private ProgressionData.DifficultyLevel GetDifficultyLevelByName(string difficulty)
    {
        if (Enum.TryParse(difficulty, out ProgressionData.DifficultyLevel level))
            return level;
        return ProgressionData.DifficultyLevel.Normal;
    }

    // Clase interna para estructurar los datos de guardado
    [Serializable]
    private class SaveData
    {
        // PlayerData
        public int playerHealth;
        public string playerWeapon;
        public int playerAmmo;
        public int playerResources;

        // FarmData
        public int farmResources;
        public List<FarmData.PlantInstance> farmPlantedPlants;
        public List<string> farmUpgrades;

        // ShooterData
        public int shooterLevel;
        public List<string> shooterWeapons;
        public List<string> shooterPowerUps;

        // ProgressionData
        public float progressionFarmingEfficiency;
        public int progressionFarmingArea;
        public float progressionShooterEfficiency;
        public int progressionShooterCapacity;
        public string progressionDifficulty;

        // TimeData
        public float sessionTime;
        public float totalPlayTime;
    }
}
