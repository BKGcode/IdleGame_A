using UnityEngine;

public class SpawnSoundManager : MonoBehaviour
{
    private static SpawnSoundManager instance;

    public static SpawnSoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpawnSoundManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SpawnSoundManager");
                    instance = go.AddComponent<SpawnSoundManager>();
                }
            }
            return instance;
        }
    }

    private AudioSource audioSource;

    [Header("Audio Settings")]
    public AudioClip spawnSound;
    [Range(0f, 1f)]
    public float spawnSoundVolume = 1f;
    public float minPitchVariation = 0.9f;
    public float maxPitchVariation = 1.1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // Asegura que el sonido sea 2D
    }

    public void PlaySpawnSound()
    {
        if (spawnSound != null)
        {
            audioSource.pitch = Random.Range(minPitchVariation, maxPitchVariation);
            audioSource.PlayOneShot(spawnSound, spawnSoundVolume);
        }
    }
}