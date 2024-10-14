// Assets/Scripts/ScriptableObjects/PlantData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlant", menuName = "ScriptableObjects/PlantData")]
public class PlantData : ScriptableObject
{
    [Header("Información de la Planta")]
    public string plantName;
    public Sprite plantIcon;

    [Header("Características de Crecimiento")]
    public float growthTime; // Tiempo en segundos para crecer
    public int resourcesProduced; // Recursos generados al cosechar

    [Header("Requisitos de Siembra")]
    public int seedsRequired;
    public Color soilColor; // Color del tile cuando está plantado
}
