using UnityEngine;
using System.Collections;

public class WeaponSystem : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil
    public Transform firePoint; // Punto desde donde se disparan los proyectiles
    public AudioClip fireSound; // Sonido al disparar
    public AudioClip reloadSound; // Sonido de recarga
    public AudioClip explosionSound; // Sonido de explosión

    public float fireRate = 0.5f; // Cadencia de disparo
    public int bulletsPerMagazine = 10; // Balas por cargador
    public int totalMagazines = 3; // Número total de cargadores
    public float reloadTime = 2f; // Tiempo de recarga
    public float maxProjectileDistance = 50f; // Distancia máxima del proyectil
    public int bulletsPerShot = 1; // Número de proyectiles disparados a la vez
    public float dispersionAngle = 5f; // Ángulo de dispersión
    public float explosionRadius = 0f; // Radio de explosión (0 si no explota)

    private int currentBullets; // Balas actuales en el cargador
    private int currentMagazines; // Cargadores restantes
    private bool isReloading = false;
    private AudioSource audioSource;

    private void Start()
    {
        currentBullets = bulletsPerMagazine;
        currentMagazines = totalMagazines;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isReloading) return;

        if (Input.GetButtonDown("Fire1") && currentBullets > 0)
        {
            Shoot();
        }
        else if (currentBullets <= 0 && currentMagazines > 0)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (fireSound != null) audioSource.PlayOneShot(fireSound);

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 shootDirection = firePoint.forward;
            float randomAngle = Random.Range(-dispersionAngle, dispersionAngle);
            shootDirection = Quaternion.Euler(0, randomAngle, 0) * shootDirection;

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            // Pasar solo la distancia máxima y el radio de explosión, ya no se pasa el shooter
            projectileScript.SetParameters(maxProjectileDistance, explosionRadius);
        }

        currentBullets--;
        if (currentBullets <= 0 && currentMagazines == 0)
        {
            DestroyWeapon();
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        if (reloadSound != null) audioSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);

        currentBullets = bulletsPerMagazine;
        currentMagazines--;

        isReloading = false;

        if (currentMagazines <= 0 && currentBullets <= 0)
        {
            DestroyWeapon();
        }
    }

    private void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    public void SetWeaponParameters(int newBulletsPerMagazine, int newTotalMagazines, float newFireRate, float newReloadTime, float newMaxProjectileDistance, float newExplosionRadius, int newBulletsPerShot, float newDispersionAngle)
    {
        bulletsPerMagazine = newBulletsPerMagazine;
        totalMagazines = newTotalMagazines;
        fireRate = newFireRate;
        reloadTime = newReloadTime;
        maxProjectileDistance = newMaxProjectileDistance;
        explosionRadius = newExplosionRadius;
        bulletsPerShot = newBulletsPerShot;
        dispersionAngle = newDispersionAngle;
    }
}
