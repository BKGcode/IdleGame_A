using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LifeSystem : MonoBehaviour
{
    [Header("Life Settings")]
    [SerializeField] private int maxLives = 3; // Define el número máximo de vidas
    [SerializeField] private float cooldownTime = 2f; // Tiempo de cooldown entre la pérdida de vidas
    private int currentLives;
    private bool canTakeDamage = true; // Variable para rastrear si se puede recibir daño

    [Header("UI Settings")]
    [SerializeField] private GameObject heartPrefab; // Prefab del corazón que se muestra en la UI
    [SerializeField] private Transform heartsContainer; // Contenedor para los corazones en la UI
    [SerializeField] private Sprite fullHeartSprite; // Sprite del corazón lleno
    [SerializeField] private Sprite emptyHeartSprite; // Sprite del corazón vacío

    private List<Image> heartImages = new List<Image>();

    private void Start()
    {
        currentLives = maxLives;
        GenerateHearts(); // Generar corazones en la UI al inicio
        UpdateHeartsUI(); // Actualizar la UI para reflejar el número de vidas actuales
    }

    private void GenerateHearts()
    {
        // Limpiar corazones existentes
        foreach (var heart in heartImages)
        {
            Destroy(heart.gameObject);
        }
        heartImages.Clear();

        // Generar corazones basados en maxLives
        for (int i = 0; i < maxLives; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
            Image heartImage = newHeart.GetComponent<Image>();
            heartImages.Add(heartImage);
        }
    }

    private void UpdateHeartsUI()
    {
        // Actualizar los corazones en la UI basado en currentLives
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentLives)
            {
                heartImages[i].sprite = fullHeartSprite; // Corazón lleno
            }
            else
            {
                heartImages[i].sprite = emptyHeartSprite; // Corazón vacío
            }
        }
    }

    public void LoseLife(int damageAmount)
    {
        if (canTakeDamage)
        {
            currentLives -= damageAmount;
            if (currentLives < 0)
            {
                currentLives = 0;
            }
            UpdateHeartsUI(); // Actualizar la UI cuando cambien las vidas

            if (currentLives <= 0)
            {
                ShowGameOver();
            }

            StartCoroutine(DamageCooldown()); // Iniciar cooldown después de recibir daño
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(cooldownTime);
        canTakeDamage = true;
    }

    public void GainLife(int healAmount)
    {
        currentLives += healAmount;
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }
        UpdateHeartsUI(); // Actualizar la UI cuando cambien las vidas
    }

    private void ShowGameOver()
    {
        // Lógica para desencadenar el game over
        Debug.Log("Game Over!");
    }
}
