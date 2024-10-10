using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        // Implementación del patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: si deseas que el GameManager persista entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Validar que las referencias están asignadas
        if (uiCanvas == null)
        {
            uiCanvas = FindObjectOfType<Canvas>();
            if (uiCanvas == null)
            {
                Debug.LogError("No se encontró un Canvas en la escena. Asegúrate de que existe un Canvas activo.");
            }
        }

        if (popupPrefab == null || warningPopupPrefab == null)
        {
            Debug.LogError("Asignar los prefabs de popups en el GameManager.");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No se encontró un AudioSource en el GameManager. Asignarlo o añadir un componente AudioSource.");
            }
        }
    }
}
