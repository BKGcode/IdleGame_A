using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Type", menuName = "Pickups/Currency Pickup")]
public class CurrencyPickupType : ScriptableObject
{
    public string pickupName;
    public int currencyAmount;
    public bool canBeDestroyedByEnemy;
    public float attractionRadius = 5f;
    public float attractionSpeed = 5f;
    public GameObject prefab;
    public Sprite uiIcon;
}

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] private CurrencyPickupType pickupType;
    [SerializeField] private ParticleSystem collectEffect;
    [SerializeField] private AudioSource collectSound;

    private bool isAttracting = false;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isAttracting)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, pickupType.attractionSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, playerTransform.position) <= pickupType.attractionRadius)
        {
            isAttracting = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectPickup(other.GetComponent<PlayerController>());
        }
        else if (other.CompareTag("Enemy") && pickupType.canBeDestroyedByEnemy)
        {
            DestroyPickup();
        }
    }

    private void CollectPickup(PlayerController playerController)
    {
        if (playerController != null)
        {
            playerController.PlayerData.AddResources(pickupType.currencyAmount);
            PlayEffectsAndDestroy();
        }
        else
        {
            Debug.LogError("PlayerController not found on Player.");
        }
    }

    private void DestroyPickup()
    {
        Destroy(gameObject);
    }

    private void PlayEffectsAndDestroy()
    {
        if (collectEffect != null)
        {
            ParticleSystem effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        if (collectSound != null)
        {
            AudioSource sound = Instantiate(collectSound, transform.position, Quaternion.identity);
            sound.Play();
            Destroy(sound.gameObject, sound.clip.length);
        }

        Destroy(gameObject);
    }
}

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] private CurrencyPickupType[] pickupTypes;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomPickup();
            timer = 0f;
        }
    }

    private void SpawnRandomPickup()
    {
        if (pickupTypes.Length == 0) return;

        CurrencyPickupType randomType = pickupTypes[Random.Range(0, pickupTypes.Length)];
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        randomPosition.y = 0f; // Asegurarse de que spawneo en el suelo

        Instantiate(randomType.prefab, randomPosition, Quaternion.identity);
    }
}

public class UIPickupDisplay : MonoBehaviour
{
    [SerializeField] private Transform pickupIconContainer;
    [SerializeField] private GameObject pickupIconPrefab;

    public void AddPickupIcon(Sprite iconSprite)
    {
        GameObject iconObject = Instantiate(pickupIconPrefab, pickupIconContainer);
        UnityEngine.UI.Image iconImage = iconObject.GetComponent<UnityEngine.UI.Image>();
        if (iconImage != null)
        {
            iconImage.sprite = iconSprite;
        }
    }
}

// Asumiendo que ya existe una clase PlayerController y PlayerData
public class PlayerController : MonoBehaviour
{
    public PlayerData PlayerData { get; private set; }

    private void Start()
    {
        PlayerData = new PlayerData();
    }

    // Otros métodos del PlayerController...
}

public class PlayerData
{
    public int Resources { get; private set; }

    public void AddResources(int amount)
    {
        Resources += amount;
        Debug.Log($"Resources added: {amount}. Total: {Resources}");
    }
}