// Assets/Scripts/Data/PlayerData.cs
using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    // Eventos para notificar cambios
    public event Action<int> OnHealthChanged;
    public event Action<WeaponData> OnWeaponChanged;
    public event Action<int> OnAmmoChanged;
    public event Action<int> OnResourcesChanged;

    // Propiedades del jugador
    [SerializeField] private int health;
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private int ammo;
    [SerializeField] private int resources; // Moneda del jugador

    public int Health => health;
    public WeaponData CurrentWeapon => currentWeapon;
    public int Ammo => ammo;
    public int Resources => resources; // Exposición de la moneda

    // Métodos para gestionar la salud
    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
        OnHealthChanged?.Invoke(health);
    }

    public void Heal(int amount)
    {
        health += amount;
        OnHealthChanged?.Invoke(health);
    }

    // Métodos para gestionar el arma
    public void ChangeWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        ammo = newWeapon.magazineSize; // Reiniciar munición al cambiar de arma
        OnWeaponChanged?.Invoke(newWeapon);
        OnAmmoChanged?.Invoke(ammo);
    }

    public void UseAmmo(int amount)
    {
        ammo -= amount;
        ammo = Mathf.Max(ammo, 0);
        OnAmmoChanged?.Invoke(ammo);
    }

    public void Reload(int magazineSize)
    {
        ammo = magazineSize;
        OnAmmoChanged?.Invoke(ammo);
    }

    // Métodos para gestionar recursos (Moneda)
    public void AddResources(int amount)
    {
        resources += amount;
        OnResourcesChanged?.Invoke(resources);
    }

    public bool SpendResources(int amount)
    {
        if (resources >= amount)
        {
            resources -= amount;
            OnResourcesChanged?.Invoke(resources);
            return true;
        }
        else
        {
            Debug.LogWarning("Moneda insuficiente para gastar.");
            return false;
        }
    }

    /// <summary>
    /// Método para aplicar una mejora al arma
    /// </summary>
    /// <param name="upgrade">La mejora a aplicar.</param>
    public void ApplyWeaponUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("UpgradeData es null en ApplyWeaponUpgrade.");
            return;
        }

        if (currentWeapon == null)
        {
            Debug.LogWarning("No hay arma equipada para aplicar la mejora.");
            return;
        }

        // Aplica la mejora al arma
        currentWeapon.ApplyUpgrade(upgrade);

        // Opcional: actualizar munición si se cambia el tamaño del cargador
        ammo = currentWeapon.magazineSize;
        OnAmmoChanged?.Invoke(ammo);

        // Notificar que el arma ha sido actualizada
        OnWeaponChanged?.Invoke(currentWeapon);
    }

    /// <summary>
    /// Método para establecer todos los datos del jugador.
    /// Utilizado principalmente para cargar datos guardados.
    /// </summary>
    /// <param name="health">Salud del jugador.</param>
    /// <param name="currentWeapon">Arma actual del jugador.</param>
    /// <param name="ammo">Munición actual del jugador.</param>
    /// <param name="resources">Recursos (moneda) del jugador.</param>
    public void SetData(int health, WeaponData currentWeapon, int ammo, int resources)
    {
        this.health = health;
        this.currentWeapon = currentWeapon;
        this.ammo = ammo;
        this.resources = resources;

        // Notificar a los suscriptores sobre los cambios
        OnHealthChanged?.Invoke(this.health);
        OnWeaponChanged?.Invoke(this.currentWeapon);
        OnAmmoChanged?.Invoke(this.ammo);
        OnResourcesChanged?.Invoke(this.resources);
    }
}
