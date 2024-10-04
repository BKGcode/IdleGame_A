using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Upgrade Settings")]
    [SerializeField] private List<UpgradeData> availableUpgrades; // Lista de mejoras disponibles en el juego
    [SerializeField] private Transform upgradeButtonParent; // El contenedor donde se instanciarán los botones
    [SerializeField] private GameObject upgradeButtonPrefab; // Prefab del botón de mejora

    private Dictionary<UpgradeData, int> upgradeUses = new Dictionary<UpgradeData, int>();
    private Dictionary<UpgradeData, float> cooldownTimers = new Dictionary<UpgradeData, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializar el número de usos y cooldowns para cada mejora
        foreach (var upgrade in availableUpgrades)
        {
            upgradeUses[upgrade] = 0; // Inicialmente no ha sido usada
            cooldownTimers[upgrade] = 0f; // Sin tiempo de espera
        }

        CreateUpgradeButtons(); // Instanciar los botones al inicio
    }

    private void Update()
    {
        UpdateCooldownTimers();
    }

    // Método para actualizar los timers de cooldown
    private void UpdateCooldownTimers()
    {
        foreach (var upgrade in availableUpgrades)
        {
            if (cooldownTimers[upgrade] > 0)
            {
                cooldownTimers[upgrade] -= Time.deltaTime;
            }
        }
    }

    // Método para instanciar los botones de mejora
    private void CreateUpgradeButtons()
    {
        foreach (var upgrade in availableUpgrades)
        {
            // Instanciar un nuevo botón a partir del prefab
            GameObject newButton = Instantiate(upgradeButtonPrefab, upgradeButtonParent);
            
            // Configurar el botón con la información de la mejora
            UpgradeButtonUI buttonUI = newButton.GetComponent<UpgradeButtonUI>();
            buttonUI.Initialize(upgrade);
        }
    }

    public bool TryPurchaseUpgrade(UpgradeData upgrade)
    {
        if (GameManager.Instance.CanAfford(upgrade.baseCost) && CanUseUpgrade(upgrade))
        {
            GameManager.Instance.SpendMoney(upgrade.baseCost);
            ApplyUpgrade(upgrade);
            return true;
        }
        return false;
    }

    private bool CanUseUpgrade(UpgradeData upgrade)
    {
        return upgradeUses[upgrade] < upgrade.maxUses && cooldownTimers[upgrade] <= 0;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        upgradeUses[upgrade]++;
        cooldownTimers[upgrade] = upgrade.cooldownTime;

        switch (upgrade.upgradeType)
        {
            case UpgradeType.IncomeMultiplier:
                GameManager.Instance.ApplyIncomeMultiplier(upgrade.effectAmount);
                break;
            case UpgradeType.TimeReduction:
                GameManager.Instance.ReduceCooldowns(upgrade.effectAmount);
                break;
        }

        Debug.Log($"{upgrade.upgradeName} aplicada. Usos restantes: {upgrade.maxUses - upgradeUses[upgrade]}");
    }

    public bool IsUpgradeAvailable(UpgradeData upgrade)
    {
        return CanUseUpgrade(upgrade);
    }

    public int GetRemainingUses(UpgradeData upgrade)
    {
        return upgrade.maxUses - upgradeUses[upgrade];
    }

    public float GetCooldown(UpgradeData upgrade)
    {
        return cooldownTimers[upgrade];
    }
}
