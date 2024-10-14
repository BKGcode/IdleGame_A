// Assets/Scripts/Data/PlayerData.cs
using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    // Eventos para notificar cambios en los datos del jugador
    public event Action<int> OnHealthChanged;
    public event Action<WeaponData> OnWeaponChanged;
    public event Action<int> OnAmmoChanged;
    public event Action<int> OnResourcesChanged;

    // Propiedades del jugador
    public int Health { get; private set; }
    public WeaponData CurrentWeapon { get; private set; }
    public int Ammo { get; private set; }
    public int Resources { get; private set; }

    // Constructor inicial
    public PlayerData(int initialHealth)
    {
        Health = initialHealth;
        Ammo = 0;
        Resources = 0;
    }

    // Métodos para modificar los datos del jugador
    public void TakeDamage(int damage)
    {
        Health -= damage;
        Health = Mathf.Max(Health, 0);
        OnHealthChanged?.Invoke(Health);
    }

    public void Heal(int amount)
    {
        Health += amount;
        OnHealthChanged?.Invoke(Health);
    }

    public void ChangeWeapon(WeaponData newWeapon)
    {
        CurrentWeapon = newWeapon;
        Ammo = newWeapon.magazineSize;
        OnWeaponChanged?.Invoke(newWeapon);
        OnAmmoChanged?.Invoke(Ammo);
    }

    public void Reload(int ammoAmount)
    {
        Ammo += ammoAmount;
        Ammo = Mathf.Min(Ammo, CurrentWeapon.magazineSize);
        OnAmmoChanged?.Invoke(Ammo);
    }

    public void UseAmmo(int amount)
    {
        Ammo -= amount;
        Ammo = Mathf.Max(Ammo, 0);
        OnAmmoChanged?.Invoke(Ammo);
    }

    public void AddResources(int amount)
    {
        Resources += amount;
        OnResourcesChanged?.Invoke(Resources);
    }

    public void SpendResources(int amount)
    {
        Resources -= amount;
        Resources = Mathf.Max(Resources, 0);
        OnResourcesChanged?.Invoke(Resources);
    }

    // Método para establecer todos los datos (usado en la carga)
    public void SetData(int health, WeaponData weapon, int ammo, int resources)
    {
        Health = health;
        CurrentWeapon = weapon;
        Ammo = ammo;
        Resources = resources;

        OnHealthChanged?.Invoke(Health);
        OnWeaponChanged?.Invoke(weapon);
        OnAmmoChanged?.Invoke(Ammo);
        OnResourcesChanged?.Invoke(Resources);
    }
}
