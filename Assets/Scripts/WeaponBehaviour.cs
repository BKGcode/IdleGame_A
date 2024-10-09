using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponData weaponData;
    
    private int currentAmmo;
    private int currentTotalAmmo;
    private float lastFireTime;
    private bool isReloading = false;

    private void Start()
    {
        if (weaponData != null)
        {
            currentAmmo = weaponData.magazineSize;
            currentTotalAmmo = weaponData.totalAmmo;
        }
    }

    public void Fire()
    {
        if (isReloading) return;

        if (Time.time - lastFireTime < 1f / weaponData.fireRate) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Aquí iría la lógica de disparo (instanciar balas, raycast, etc.)
        Debug.Log($"Disparando {weaponData.weaponName}");

        currentAmmo--;
        lastFireTime = Time.time;

        if (currentAmmo <= 0 && currentTotalAmmo <= 0)
        {
            DestroyWeapon();
        }
    }

    private IEnumerator Reload()
    {
        if (currentTotalAmmo <= 0) yield break;

        isReloading = true;
        Debug.Log($"Recargando {weaponData.weaponName}");

        yield return new WaitForSeconds(weaponData.reloadTime);

        int ammoToReload = Mathf.Min(weaponData.magazineSize - currentAmmo, currentTotalAmmo);
        currentAmmo += ammoToReload;
        currentTotalAmmo -= ammoToReload;

        isReloading = false;
    }

    private void DestroyWeapon()
    {
        Debug.Log($"Destruyendo {weaponData.weaponName}");
        Destroy(gameObject);
    }
}