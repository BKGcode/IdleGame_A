using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameObject newWeaponPrefab; // Prefab del arma que se recogerá

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponSystem currentWeapon = other.GetComponentInChildren<WeaponSystem>();

            // Destruir el arma actual si el jugador ya tiene una
            if (currentWeapon != null)
            {
                Destroy(currentWeapon.gameObject);
            }

            // Instanciar la nueva arma y asignarla al jugador
            GameObject newWeapon = Instantiate(newWeaponPrefab, other.transform.position, Quaternion.identity, other.transform);

            // Ajustar la rotación del arma a (0,0,0)
            newWeapon.transform.localRotation = Quaternion.Euler(0, 180, 0);

            // Destruir el objeto del pickup después de ser recogido
            Destroy(gameObject);
        }
    }
}
