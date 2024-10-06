using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidad de movimiento normal
    public float boostMultiplier = 2f;  // Multiplicador de velocidad para el boost
    public Camera mainCamera;  // Cámara principal para detectar la posición del ratón
    public LifeData lifeData;  // Sistema de vidas (ScriptableObject)

    private Vector3 moveDirection;  // Dirección de movimiento
    private bool isAlive = true;  // Control para evitar más daño si ya está muerto

    private void Update()
    {
        if (isAlive)
        {
            RotateTowardsMouse();  // Maneja la rotación del jugador hacia el puntero del ratón
            ProcessInput();  // Control del movimiento del jugador
        }
    }

    // Método para hacer que el jugador apunte hacia el puntero del ratón
    private void RotateTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);  // Lanza un rayo desde la cámara hacia el ratón
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);  // Creamos un plano que representa el suelo

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);  // Obtenemos el punto donde el rayo golpea el plano
            Vector3 lookDirection = point - transform.position;  // Calculamos la dirección hacia ese punto
            lookDirection.y = 0;  // Ignoramos la rotación en el eje Y para evitar que el jugador gire verticalmente

            transform.LookAt(transform.position + lookDirection);  // Ajustamos la rotación del jugador
        }
    }

    // Método para procesar la entrada del teclado
    private void ProcessInput()
    {
        // Obtenemos las entradas de teclado
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // Calculamos la dirección de movimiento en el espacio local
        moveDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // Movemos al jugador si hay una entrada válida
        if (moveDirection.magnitude >= 0.1f)
        {
            MovePlayer();
        }
    }

    // Método para mover al jugador sin Rigidbody
    private void MovePlayer()
    {
        // Comprobamos si se está presionando la tecla Shift para aplicar el boost
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * boostMultiplier : moveSpeed;

        // Convertimos la dirección de movimiento a espacio local
        Vector3 movement = transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime;

        // Movemos al jugador usando transform.Translate
        transform.Translate(movement, Space.World);
    }

    // Método para recibir daño y perder vidas
    public void TakeDamage(int damageAmount)
    {
        if (!isAlive) return;  // Evitar recibir daño si ya ha muerto

        // En lugar de reducir las vidas directamente, usamos el método LoseLife() en LifeData
        lifeData.LoseLife();

        if (lifeData.currentLives <= 0)
        {
            Die();  // Si las vidas llegan a 0, el jugador muere
        }
    }

    // Método para manejar la muerte del jugador
    private void Die()
    {
        isAlive = false;
        Debug.Log("Player Died");
        // Aquí puedes manejar la lógica de Game Over, reiniciar el nivel, etc.
    }
}
