using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 1; // Cantidad de daño que inflige

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ReceiveDamage(damageAmount);
            }

            // Opcional: Destruir el enemigo después de infligir daño
            // Destroy(gameObject);
        }
    }
}
