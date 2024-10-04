using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] hearts; // Array de im�genes de los corazones
    public Sprite fullHeart; // Sprite de coraz�n lleno
    public Sprite emptyHeart; // Sprite de coraz�n vac�o

    // M�todo para actualizar la UI de las vidas
    public void UpdateLivesUI(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lives)
            {
                hearts[i].sprite = fullHeart; // Mostrar coraz�n lleno
            }
            else
            {
                hearts[i].sprite = emptyHeart; // Mostrar coraz�n vac�o
            }
        }
    }
}
