using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Vector3 movement;
    private Rigidbody rb;

    private void Start()
    {
        // Inicializamos el Rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody no está asignado en el PlayerMovement2D.");
        }
    }

    void Update()
    {
        // Movimiento en el plano XZ
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0, moveZ).normalized;

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }

        // No hay lógica de salto, así que no es necesario verificar si está en el suelo
    }

    void FixedUpdate()
    {
        // Movemos al personaje
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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
