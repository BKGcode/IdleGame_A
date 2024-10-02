using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    public float jumpCooldown = 0.5f; // Tiempo en segundos para permitir saltos consecutivos
    private float jumpCooldownTimer;  // Temporizador para rastrear el tiempo desde el último salto

    private Vector3 movement;
    private Rigidbody rb;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private int jumpCount;  // Contador de saltos

    private void Start()
    {
        // Inicializamos el rigidbody
        rb = GetComponent<Rigidbody>();
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

        // Verificamos si el personaje está tocando el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            // Reiniciamos el temporizador y contador de saltos cuando toca el suelo
            jumpCooldownTimer = jumpCooldown;
            jumpCount = 0;  // Restablecer el contador de saltos al tocar el suelo
        }

        // Detectamos el salto
        if (Input.GetButtonDown("Jump") && (isGrounded || (jumpCount < 2 && jumpCooldownTimer > 0)))
        {
            Jump();
        }

        // Reducimos el temporizador si no está en el suelo
        if (!isGrounded && jumpCooldownTimer > 0)
        {
            jumpCooldownTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Movemos al personaje
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Jump()
    {
        // Restablecemos la velocidad vertical antes de aplicar el salto
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Aumentamos el contador de saltos
        jumpCount++;

        // Reiniciamos el temporizador de salto tras un salto exitoso
        jumpCooldownTimer = jumpCooldown;
    }
}
