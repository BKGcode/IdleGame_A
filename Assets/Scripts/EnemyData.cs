using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game Systems/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float moveSpeed = 3f;  // Velocidad de movimiento del enemigo
    public int damage = 10;  // Daño que inflige al jugador
    public int maxLives = 3;  // Número máximo de vidas del enemigo
    public GameObject enemyPrefab;  // Prefab del enemigo, para definir su aspecto
}
