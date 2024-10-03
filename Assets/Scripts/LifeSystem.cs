using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    public int maxLives = 3;           // Número máximo de vidas
    public int currentLives;           // Vidas actuales del jugador

    public Image[] hearts;             // Array de imágenes de corazones
    public Sprite fullHeart;           // Sprite del corazón lleno
    public Sprite emptyHeart;          // Sprite del corazón vacío

    private void Start()
    {
        // Inicializar las vidas actuales al máximo
        currentLives = maxLives;

        // Actualizar la UI al iniciar el juego
        UpdateHeartsUI();
    }

    // Método para reducir vidas
    public void LoseLife(int amount)
    {
        currentLives -= amount;

        // Asegurarse de que las vidas no sean negativas
        if (currentLives < 0)
            currentLives = 0;

        // Actualizar la UI
        UpdateHeartsUI();

        // Comprobar si el jugador ha perdido todas las vidas
        if (currentLives == 0)
        {
            // Lógica de juego cuando el jugador pierde todas las vidas
            Debug.Log("Jugador sin vidas");
            // Puedes llamar a un método para terminar el juego, mostrar una pantalla de Game Over, etc.
        }
    }

    // Método para ganar vidas
    public void GainLife(int amount)
    {
        currentLives += amount;

        // Asegurarse de que las vidas no excedan el máximo
        if (currentLives > maxLives)
            currentLives = maxLives;

        // Actualizar la UI
        UpdateHeartsUI();
    }

    // Método para actualizar la UI de los corazones
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;  // Asegurarse de que el corazón esté visible
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                hearts[i].enabled = true;  // Mantener el corazón visible pero vacío
            }
        }
    }
}
