using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody no está asignado en el PlayerController.");
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    // Método para recibir daño
    public void ReceiveDamage(int damageAmount)
    {
        // Aquí podrías manejar la salud del jugador si tienes un sistema de salud adicional

        // Informar al GameManager que el jugador ha recibido daño
        if (GameManager.instance != null)
        {
            GameManager.instance.DecreaseLife();
        }
        else
        {
            Debug.LogError("GameManager.instance es null. Asegúrate de que el GameManager está en la escena.");
        }
    }

    // Método para curarse (opcional)
    public void Heal(int healAmount)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.IncreaseLife();
        }
    }

    // Otros métodos relacionados con el jugador...
}
