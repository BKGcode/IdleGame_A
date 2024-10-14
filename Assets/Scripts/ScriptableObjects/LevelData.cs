// Assets/Scripts/ScriptableObjects/LevelData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;

    [Header("Configuración de Enemigos")]
    public GameObject[] enemyPrefabs;
    public int enemyCount;

    [Header("Loot Disponible")]
    public GameObject[] lootPrefabs;

    [Header("Configuración del Nivel")]
    public float levelDuration; // Duración del nivel en segundos
    public Vector3 spawnAreaSize; // Tamaño del área de spawn
}
