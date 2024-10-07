using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [Header("Fog Settings")]
    public Transform player; // Referencia al objeto del jugador
    public float visionRadius = 5f; // Radio de visión del jugador

    [Header("Fog of War Components")]
    public Texture2D fogTexture; // Textura de la niebla, asignable desde el Inspector
    public Color fogColor = Color.black; // Color de la niebla
    public Color clearedColor = Color.clear; // Color para área despejada

    private Texture2D dynamicFogTexture;
    private Renderer fogRenderer;
    private int textureWidth;
    private int textureHeight;
    private bool[,] visibilityMap; // Mapa de visibilidad persistente

    private void Start()
    {
        // Validar si la textura de niebla es legible
        if (!fogTexture.isReadable)
        {
            Debug.LogError("La textura de niebla no es legible. Asegúrate de que 'Read/Write Enabled' esté activado en las configuraciones de importación de la textura.");
            return;
        }

        // Inicializamos la textura de niebla dinámica
        textureWidth = fogTexture.width;
        textureHeight = fogTexture.height;
        dynamicFogTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        dynamicFogTexture.SetPixels(fogTexture.GetPixels());
        dynamicFogTexture.Apply();

        // Obtenemos el renderer del objeto que tiene la niebla
        fogRenderer = GetComponent<Renderer>();
        fogRenderer.material.SetTexture("_FogTex", dynamicFogTexture);
        fogRenderer.material.SetColor("_FogColor", fogColor);

        // Inicializamos el mapa de visibilidad
        visibilityMap = new bool[textureWidth, textureHeight];
    }

    private void Update()
    {
        if (fogRenderer != null && dynamicFogTexture != null)
        {
            UpdateFog();
        }
    }

    private void UpdateFog()
    {
        // Convertir la posición del jugador en coordenadas de textura
        Vector3 playerPosition = player.position;
        Vector2 textureCoord = new Vector2(playerPosition.x / transform.localScale.x, playerPosition.z / transform.localScale.z);
        int pixelX = Mathf.FloorToInt(textureCoord.x * textureWidth);
        int pixelY = Mathf.FloorToInt(textureCoord.y * textureHeight);

        // Actualizar los píxeles en el radio de visión
        int radiusInPixels = Mathf.FloorToInt((visionRadius / transform.localScale.x) * textureWidth);
        for (int y = -radiusInPixels; y <= radiusInPixels; y++)
        {
            for (int x = -radiusInPixels; x <= radiusInPixels; x++)
            {
                if (x * x + y * y <= radiusInPixels * radiusInPixels)
                {
                    int fogX = Mathf.Clamp(pixelX + x, 0, textureWidth - 1);
                    int fogY = Mathf.Clamp(pixelY + y, 0, textureHeight - 1);
                    visibilityMap[fogX, fogY] = true; // Marcar como visible
                }
            }
        }

        // Aplicar el mapa de visibilidad a la textura de niebla
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                if (visibilityMap[x, y])
                {
                    dynamicFogTexture.SetPixel(x, y, clearedColor);
                }
            }
        }

        // Aplicar los cambios a la textura de niebla
        dynamicFogTexture.Apply();
    }
}

/*
Variables explicadas:
- player: referencia al transform del jugador para saber su posición en todo momento.
- visionRadius: radio de visión que despeja la niebla en torno al jugador.
- fogTexture: textura base de la niebla que se asigna desde el Inspector.
- dynamicFogTexture: textura de niebla que se modifica según el movimiento del jugador.
- fogRenderer: renderer del objeto que tiene la niebla aplicada.
- visibilityMap: mapa de visibilidad persistente que marca las áreas que ya fueron exploradas.
*/