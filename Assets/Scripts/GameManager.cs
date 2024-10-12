using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public Canvas uiCanvas; // Cambiado de private a public
    public GameObject popupPrefab; // Cambiado de private a public
    public GameObject warningPopupPrefab; // Cambiado de private a public

    [Header("FX and Sound")]
    public GameObject hireFXPrefab; // Cambiado de private a public
    public AudioClip hireSoundClip; // Cambiado de private a public
    public AudioSource audioSource; // Cambiado de private a public

    [Header("Enemy Tracking")]
    private Dictionary<EnemyTypeSO, int> destroyedEnemiesCounts = new Dictionary<EnemyTypeSO, int>();

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

            // Optional: Destroy the FX after a certain time if it doesn't self-destruct
            ParticleSystem particleSystem = fxInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                float lifetime = particleSystem.main.duration;
                Destroy(fxInstance, lifetime);
            }
            else
            {
                Destroy(fxInstance, 5f); // Default time if not a particle system
            }
        }
        else
        {
            Debug.LogWarning("Could not spawn hire FX. Check the prefab reference in the GameManager.");
        }
    }

    // Auxiliary method to pause/resume game time
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Debug.Log($"Game time scale set to: {scale}");
    }

    // Method to reset the GameManager if needed
    public void ResetGameManager()
    {
        destroyedEnemiesCounts.Clear();
        Debug.Log("GameManager has been reset.");
    }

    // Methods for tracking destroyed enemies
    public void IncrementEnemyDestroyedCount(EnemyTypeSO enemyType)
    {
        if (!destroyedEnemiesCounts.ContainsKey(enemyType))
        {
            destroyedEnemiesCounts[enemyType] = 0;
        }
        destroyedEnemiesCounts[enemyType]++;
        Debug.Log($"Enemy '{enemyType.enemyName}' destroyed. Total count: {destroyedEnemiesCounts[enemyType]}.");
    }

    public Dictionary<EnemyTypeSO, int> GetDestroyedEnemiesCounts()
    {
        return new Dictionary<EnemyTypeSO, int>(destroyedEnemiesCounts);
    }

    public int GetDestroyedEnemyCount(EnemyTypeSO enemyType)
    {
        if (destroyedEnemiesCounts.TryGetValue(enemyType, out int count))
        {
            return count;
        }
        return 0;
    }

    public void ResetEnemyCounts()
    {
        destroyedEnemiesCounts.Clear();
        Debug.Log("Enemy counts have been reset.");
    }

    // Additional methods to handle popups
    public void ShowPopup(string message)
    {
        if (popupPrefab != null && uiCanvas != null)
        {
            GameObject popupInstance = Instantiate(popupPrefab, uiCanvas.transform);
            // Assuming the popup has a TextMeshProUGUI component
            TextMeshProUGUI popupText = popupInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (popupText != null)
            {
                popupText.text = message;
            }
        }
        else
        {
            Debug.LogWarning("Cannot show popup. Check popupPrefab and uiCanvas assignments in GameManager.");
        }
    }

    public void ShowWarningPopup(string message)
    {
        if (warningPopupPrefab != null && uiCanvas != null)
        {
            GameObject warningPopupInstance = Instantiate(warningPopupPrefab, uiCanvas.transform);
            TextMeshProUGUI warningText = warningPopupInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (warningText != null)
            {
                warningText.text = message;
            }
        }
        else
        {
            Debug.LogWarning("Cannot show warning popup. Check warningPopupPrefab and uiCanvas assignments in GameManager.");
        }
    }
}
