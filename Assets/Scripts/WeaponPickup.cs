using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponManager playerWeaponManager = other.GetComponent<PlayerWeaponManager>();
            if (playerWeaponManager != null)
            {
                playerWeaponManager.EquipWeapon(weaponData);
                Destroy(gameObject);
            }
        }
    }
}