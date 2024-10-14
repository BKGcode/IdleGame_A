using System;
using UnityEngine;

namespace ShooterGame.Data
{
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

        // Propiedades públicas
        public int Health 
        { 
            get => health;
            private set
            {
                health = value;
                OnHealthChanged?.Invoke(health);
            }
        }

        public WeaponData CurrentWeapon 
        { 
            get => currentWeapon;
            private set
            {
                currentWeapon = value;
                OnWeaponChanged?.Invoke(currentWeapon);
            }
        }

        public int Ammo 
        { 
            get => ammo;
            private set
            {
                ammo = value;
                OnAmmoChanged?.Invoke(ammo);
            }
        }

        public int Resources 
        { 
            get => resources;
            private set
            {
                resources = value;
                OnResourcesChanged?.Invoke(resources);
            }
        }

        // Métodos para gestionar la salud
        public void TakeDamage(int damage)
        {
            Health -= damage;
            Health = Mathf.Max(Health, 0);
        }

        public void Heal(int amount)
        {
            Health += amount;
            Health = Mathf.Min(Health, 100); // Asumiendo que 100 es la salud máxima
        }

        // Métodos para gestionar el arma
        public void ChangeWeapon(WeaponData newWeapon)
        {
            if (newWeapon == null)
            {
                Debug.LogWarning("Intento de cambiar a un arma nula.");
                return;
            }

            CurrentWeapon = newWeapon;
            Ammo = newWeapon.magazineSize; // Reiniciar munición al cambiar de arma
        }

        public void UseAmmo(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Intento de usar una cantidad negativa de munición.");
                return;
            }

            Ammo -= amount;
            Ammo = Mathf.Max(Ammo, 0);
        }

        public void Reload(int magazineSize)
        {
            Ammo = magazineSize;
        }

        // Métodos para gestionar recursos (Moneda)
        public void AddResources(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Intento de añadir una cantidad negativa de recursos.");
                return;
            }

            Resources += amount;
        }

        public bool SpendResources(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Intento de gastar una cantidad negativa de recursos.");
                return false;
            }

            if (Resources >= amount)
            {
                Resources -= amount;
                return true;
            }
            else
            {
                Debug.LogWarning("Recursos insuficientes para gastar.");
                return false;
            }
        }

        // Método para aplicar una mejora al arma
        public void ApplyWeaponUpgrade(UpgradeData upgrade)
        {
            if (upgrade == null)
            {
                Debug.LogWarning("UpgradeData es null en ApplyWeaponUpgrade.");
                return;
            }

            if (CurrentWeapon == null)
            {
                Debug.LogWarning("No hay arma equipada para aplicar la mejora.");
                return;
            }

            CurrentWeapon.ApplyUpgrade(upgrade);
            Ammo = CurrentWeapon.magazineSize;
        }

        // Método para establecer todos los datos del jugador
        public void SetData(int health, WeaponData currentWeapon, int ammo, int resources)
        {
            Health = health;
            CurrentWeapon = currentWeapon;
            Ammo = ammo;
            Resources = resources;
        }

        // Método para inicializar los datos del jugador
        public void Initialize(int initialHealth, WeaponData initialWeapon, int initialResources)
        {
            Health = initialHealth;
            CurrentWeapon = initialWeapon;
            Ammo = initialWeapon != null ? initialWeapon.magazineSize : 0;
            Resources = initialResources;
        }
    }
}