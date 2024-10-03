using UnityEngine;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour
{
    public int maxLives = 3;           // N�mero m�ximo de vidas
    public int currentLives;           // Vidas actuales del jugador

    public Image[] hearts;             // Array de im�genes de corazones
    public Sprite fullHeart;           // Sprite del coraz�n lleno
    public Sprite emptyHeart;          // Sprite del coraz�n vac�o

    private void Start()
    {
        // Inicializar las vidas actuales al m�ximo
        currentLives = maxLives;

        // Actualizar la UI al iniciar el juego
        UpdateHeartsUI();
    }

    // M�todo para reducir vidas
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
            // L�gica de juego cuando el jugador pierde todas las vidas
            Debug.Log("Jugador sin vidas");
            // Puedes llamar a un m�todo para terminar el juego, mostrar una pantalla de Game Over, etc.
        }
    }

    // M�todo para ganar vidas
    public void GainLife(int amount)
    {
        currentLives += amount;

        // Asegurarse de que las vidas no excedan el m�ximo
        if (currentLives > maxLives)
            currentLives = maxLives;

        // Actualizar la UI
        UpdateHeartsUI();
    }

    // M�todo para actualizar la UI de los corazones
    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;  // Asegurarse de que el coraz�n est� visible
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                hearts[i].enabled = true;  // Mantener el coraz�n visible pero vac�o
            }
        }
    }
}
