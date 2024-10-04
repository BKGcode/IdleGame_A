using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] hearts; // Array de imágenes de los corazones
    public Sprite fullHeart; // Sprite de corazón lleno
    public Sprite emptyHeart; // Sprite de corazón vacío

    // Método para actualizar la UI de las vidas
    public void UpdateLivesUI(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].sprite = fullHeart; // Mostrar corazón lleno
            }
            else
            {
                hearts[i].sprite = emptyHeart; // Mostrar corazón vacío
            }
        }
    }
}
