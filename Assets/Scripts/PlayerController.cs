using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad normal del jugador
    public float boostSpeed = 10f; // Velocidad de impulso al presionar Shift
    private Vector3 moveDirection; // Dirección de movimiento

    public LifeSystem lifeSystem; // Referencia al sistema de vidas

    private void Update()
    {
        HandleMovement(); // Manejar el movimiento del jugador
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ).normalized; // Direccion de movimiento

        if (moveDirection.magnitude >= 0.1f)
        {
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : moveSpeed; // Cambiar velocidad si se presiona Shift
            transform.Translate(moveDirection * currentSpeed * Time.deltaTime, Space.World); // Movimiento del jugador
        }
    }

    // Método para detectar colisiones con objetos
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si el objeto tiene el tag "Enemy", el jugador pierde vida
            lifeSystem.LoseLife(1); // Perder 1 vida al colisionar con un enemigo
        }
    }

    // Método para simular recibir daño
    public void TakeDamage(int damageAmount)
    {
        lifeSystem.LoseLife(damageAmount); // Perder vidas a través del sistema de vidas
    }

    // Método para ganar vidas
    public void GainLife(int amount)
    {
        lifeSystem.GainLife(amount); // Ganar vidas a través del sistema de vidas
    }
}
