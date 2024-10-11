using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public Canvas uiCanvas;
    public GameObject popupPrefab;
    public GameObject warningPopupPrefab;

    [Header("FX and Sound")]
    public GameObject hireFXPrefab;
    public AudioClip hireSoundClip;
    public AudioSource audioSource;

    private void Awake()
    {
        InitializeSingleton();
        ValidateReferences();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created and set to DontDestroyOnLoad.");
        }
        else
        {
            Debug.LogWarning("Multiple instances of GameManager detected. Destroying the new instance.");
            Destroy(gameObject);
        }
    }

    private void ValidateReferences()
    {
        ValidateCanvas();
        ValidatePopupPrefabs();
        ValidateAudioComponents();
        ValidateHireFX();
    }

    private void ValidateCanvas()
    {
        if (uiCanvas == null)
        {
            uiCanvas = FindObjectOfType<Canvas>();
            if (uiCanvas == null)
            {
                Debug.LogError("No Canvas found in the scene. Make sure an active Canvas exists.");
            }
            else
            {
                Debug.Log("Canvas found and automatically assigned.");
            }
        }
        else
        {
            Debug.Log("Canvas reference is valid.");
        }
    }

    private void ValidatePopupPrefabs()
    {
        if (popupPrefab == null)
        {
            Debug.LogError("Assign the popup prefab in the GameManager.");
        }
        else
        {
            Debug.Log("Popup prefab reference is valid.");
        }

        if (warningPopupPrefab == null)
        {
            Debug.LogError("Assign the warning popup prefab in the GameManager.");
        }
        else
        {
            Debug.Log("Warning popup prefab reference is valid.");
        }
    }

    private void ValidateAudioComponents()
    {
        if (hireSoundClip == null)
        {
            Debug.LogWarning("The hire sound clip is not assigned in the GameManager.");
        }
        else
        {
            Debug.Log("Hire sound clip reference is valid.");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning("No AudioSource found on the GameManager. Adding one automatically.");
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                Debug.Log("AudioSource found and automatically assigned.");
            }
        }
        else
        {
            Debug.Log("AudioSource reference is valid.");
        }
    }

    private void ValidateHireFX()
    {
        if (hireFXPrefab == null)
        {
            Debug.LogWarning("The hire FX prefab is not assigned in the GameManager.");
        }
        else
        {
            Debug.Log("Hire FX prefab reference is valid.");
        }
    }

    public void PlayHireSound()
    {
        if (audioSource != null && hireSoundClip != null)
        {
            audioSource.PlayOneShot(hireSoundClip);
            Debug.Log("Hire sound played successfully.");
        }
        else
        {
            Debug.LogWarning("Could not play hire sound. Check the references in the GameManager.");
        }
    }

    public void SpawnHireFX(Vector3 position)
    {
        if (hireFXPrefab != null)
        {
            GameObject fxInstance = Instantiate(hireFXPrefab, position, Quaternion.identity);
            Debug.Log($"Hire FX spawned at position: {position}");
            
            // Opcional: Destruir el FX después de un tiempo si no se autodestruye
            ParticleSystem particleSystem = fxInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                float lifetime = particleSystem.main.duration;
                Destroy(fxInstance, lifetime);
            }
            else
            {
                Destroy(fxInstance, 5f); // Tiempo por defecto si no es un sistema de partículas
            }
        }
        else
        {
            Debug.LogWarning("Could not spawn hire FX. Check the prefab reference in the GameManager.");
        }
    }

    // Método auxiliar para pausar/reanudar el tiempo del juego
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Debug.Log($"Game time scale set to: {scale}");
    }

    // Método para reiniciar el GameManager si es necesario
    public void ResetGameManager()
    {
        // Aquí puedes añadir lógica para reiniciar variables o estados si es necesario
        Debug.Log("GameManager has been reset.");
    }
}