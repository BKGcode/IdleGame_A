using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Type", menuName = "Enemy/Enemy Type")]
public class EnemyTypeSO : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public GameObject enemyPrefab;
    public Sprite enemySprite;

    [Header("Patrol")]
    public float patrolRadius = 10f;
    public float minPatrolWaitTime = 1f;
    public float maxPatrolWaitTime = 3f;

    [Header("Chase")]
    public float detectionRadius = 5f;
    [Range(0, 10)]
    public float chaseProbability = 5f;
    public float chaseSpeed = 7f;

    [Header("Movement")]
    public float patrolSpeed = 3f;
    public float rotationSpeed = 5f;

    [Header("Combat")]
    public float maxHealth = 3f;
    public float damage = 1f;

    [Header("Rewards")]
    public double currencyReward = 10.0;

    [Header("Effects")]
    public GameObject destructionParticlesPrefab;
    public AudioClip destructionSound;
}
