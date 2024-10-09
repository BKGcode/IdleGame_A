using UnityEngine;
using System.Collections;

public class WeaponBehaviour : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform firePoint;
    
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

        if (weaponData.projectilePrefab != null && firePoint != null)
        {
            GameObject projectileObject = Instantiate(weaponData.projectilePrefab, firePoint.position, firePoint.rotation);
            projectileObject.tag = "Projectile";
            
            ProjectileBehaviour projectile = projectileObject.GetComponent<ProjectileBehaviour>();
            if (projectile != null)
            {
                projectile.SetSpeed(weaponData.projectileSpeed);
            }
        }

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