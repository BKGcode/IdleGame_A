using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponData weaponData;
    public float bobSpeed = 1f;
    public float bobHeight = 0.5f;
    public float rotationSpeed = 50f;
    public AudioClip pickupSound;
    public ParticleSystem pickupParticlesPrefab;

    private Vector3 startPosition;
    private float bobTime;
    private GameObject weaponInstance;

    private void Start()
    {
        startPosition = transform.position;
        
        if (weaponData != null && weaponData.weaponPrefab != null)
        {
            weaponInstance = Instantiate(weaponData.weaponPrefab, transform);
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponInstance.transform.localRotation = Quaternion.identity;
            
            // Desactivar cualquier collider en el arma instanciada para evitar interferencias
            Collider weaponCollider = weaponInstance.GetComponent<Collider>();
            if (weaponCollider != null)
            {
                weaponCollider.enabled = false;
            }
        }
    }

    private void Update()
    {
        // Movimiento de arriba a abajo
        bobTime += Time.deltaTime * bobSpeed;
        float yOffset = Mathf.Sin(bobTime) * bobHeight;
        transform.position = startPosition + new Vector3(0f, yOffset, 0f);

        // Rotación
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponManager playerWeaponManager = other.GetComponent<PlayerWeaponManager>();
            if (playerWeaponManager != null)
            {
                // Equipar el arma
                playerWeaponManager.EquipWeapon(weaponData);

                // Reproducir sonido
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                // Crear y reproducir partículas
                if (pickupParticlesPrefab != null)
                {
                    ParticleSystem particles = Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);
                    particles.Play();

                    // Destruir el sistema de partículas después de que termine
                    float particleDuration = particles.main.duration + particles.main.startLifetime.constantMax;
                    Destroy(particles.gameObject, particleDuration);
                }

                // Destruir el objeto de recogida
                Destroy(gameObject);
            }
        }
    }
}