using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 1; // Cantidad de da�o que inflige

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ReceiveDamage(damageAmount);
            }

            // Opcional: Destruir el enemigo despu�s de infligir da�o
            // Destroy(gameObject);
        }
    }
}
