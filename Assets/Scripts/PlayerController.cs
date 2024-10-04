using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad normal del jugador
    public float boostSpeed = 10f; // Velocidad de impulso al presionar Shift
    private Vector3 moveDirection; // Dirección de movimiento

    public LifeSystem lifeSystem; // Referencia al sistema de vidas
    public Camera mainCamera; // Cámara principal para proyectar el punto de clic del ratón
    public LayerMask groundLayer; // Capa para detectar el suelo

    private void Update()
    {
        HandleRotation(); // Manejar la rotación del jugador
        HandleMovement(); // Manejar el movimiento del jugador
    }

    private void HandleRotation()
    {
        // Ray desde la cámara hacia el punto donde está el cursor del ratón
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Si el ray colisiona con el plano del suelo (groundLayer), rotar hacia ese punto
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            Vector3 directionToLook = (targetPosition - transform.position).normalized;
            directionToLook.y = 0; // Evitar que el jugador se incline hacia arriba o abajo

            // Rotar el jugador hacia la dirección del puntero del ratón
            if (directionToLook != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Suavizar la rotación
            }
        }
    }

    private void HandleMovement()
    {
        float moveZ = Input.GetAxis("Vertical"); // Avanzar (W) o retroceder (S)

        if (Mathf.Abs(moveZ) > 0.1f) // Si se presiona W o S
        {
            // Velocidad actual, cambiada si se presiona Shift
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : moveSpeed;

            // Mover hacia adelante o hacia atrás basado en la dirección local del jugador
            transform.Translate(Vector3.forward * moveZ * currentSpeed * Time.deltaTime, Space.Self);
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
