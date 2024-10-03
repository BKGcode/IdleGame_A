using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHearts = 5;
    private int currentHearts;

    private void Start()
    {
        currentHearts = maxHearts;
        // Emitir evento inicial para actualizar la UI de corazones
        GameEvents.PlayerHealed(0); // Pasa 0 para solo actualizar la UI
    }

    private void Update()
    {
        // Aquí puedes agregar la lógica de movimiento u otras interacciones del jugador
    }

    public void TakeDamage(int damage)
    {
        currentHearts -= damage;
        if (currentHearts <= 0)
        {
            currentHearts = 0;
            Die();
        }

        // Emitir evento de daño para que otros sistemas lo escuchen
        GameEvents.PlayerDamaged(damage);
    }

    public void Heal(int amount)
    {
        currentHearts += amount;
        if (currentHearts > maxHearts)
            currentHearts = maxHearts;

        // Emitir evento de curación para que otros sistemas lo escuchen
        GameEvents.PlayerHealed(amount);
    }

    private void Die()
    {
        // Emitir evento de muerte
        GameEvents.PlayerDied();

        // Opcional: Desactivar al jugador, reproducir animación de muerte, etc.
        gameObject.SetActive(false);
    }

    // Método para obtener el estado actual de corazones (opcional)
    public int GetCurrentHearts()
    {
        return currentHearts;
    }
}
