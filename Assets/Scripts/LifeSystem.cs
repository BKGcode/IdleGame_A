using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    // Referencia al ScriptableObject de vidas
    public LifeData lifeData;

    private void Start()
    {
        // Asignamos las funciones para cuando se ganen o pierdan vidas
        lifeData.onLifeLost.AddListener(OnLifeLost);
        lifeData.onLifeGained.AddListener(OnLifeGained);
    }

    // Función para que el jugador o enemigo pierda una vida
    public void TakeDamage()
    {
        lifeData.LoseLife();  // El personaje pierde una vida

        if (lifeData.currentLives <= 0)
        {
            Die();  // Si las vidas llegan a 0, el personaje muere
        }
    }

    // Función para manejar la muerte
    private void Die()
    {
        Debug.Log("Character has died");  // Aquí puedes manejar el Game Over o la muerte del enemigo
        // Puedes añadir más lógica aquí para la muerte del personaje o enemigo
    }

    // Función para llamar cuando el personaje pierde una vida
    private void OnLifeLost()
    {
        // Aquí puedes actualizar la UI o añadir efectos visuales/sonoros
    }

    // Función para llamar cuando el personaje gana una vida
    private void OnLifeGained()
    {
        // Aquí puedes actualizar la UI o añadir efectos visuales/sonoros
    }

    private void OnDestroy()
    {
        // Removemos los listeners al destruir este objeto
        lifeData.onLifeLost.RemoveListener(OnLifeLost);
        lifeData.onLifeGained.RemoveListener(OnLifeGained);
    }
}
