// Assets/Scripts/ScriptableObjects/CollectableResourceData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectableResource", menuName = "ScriptableObjects/CollectableResourceData")]
public class CollectableResourceData : ScriptableObject
{
    [Header("Información del Recurso")]
    public string resourceName;
    public Sprite resourceIcon;

    [Header("Características del Recurso")]
    public int amount; // Cantidad de recursos que proporciona
}
