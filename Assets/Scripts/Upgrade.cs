using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public string Name; // Nombre de la mejora
    public Sprite UpgradeSprite; // Imagen específica de la mejora
    public int Cost; // Costo de la mejora
    public int CurrentLevel = 0; // Nivel actual de la mejora
    public int MaxLevel = 5; // Nivel máximo de la mejora
    public bool IsAvailable => CurrentLevel < MaxLevel; // Está disponible si no ha alcanzado el nivel máximo

    // Método para aumentar el costo de la mejora después de una compra
    public void IncreaseCost(float multiplier)
    {
        Cost = Mathf.CeilToInt(Cost * multiplier);
    }
}
