using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Singleton Instance
    public static AudioManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // Nombres de los par�metros tal como aparecen en el AudioMixer
    private string masterVolumeParam = "Master";
    private string musicVolumeParam = "Music";

    private void Awake()
    {
        // Implementaci�n del Patr�n Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            LoadAndApplyVolumeSettings();
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }

    private void LoadAndApplyVolumeSettings()
    {
        // Cargar los vol�menes guardados o usar valores por defecto (0 dB)
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
    }

    public void SetMasterVolume(float volume)
    {
        if (audioMixer != null)
        {
            bool result = audioMixer.SetFloat(masterVolumeParam, volume);
            if (!result)
            {
                Debug.LogError($"El par�metro '{masterVolumeParam}' no existe en el AudioMixer.");
            }
        }
        else
        {
            Debug.LogError("AudioMixer no est� asignado en AudioManager.");
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioMixer != null)
        {
            bool result = audioMixer.SetFloat(musicVolumeParam, volume);
            if (!result)
            {
                Debug.LogError($"El par�metro '{musicVolumeParam}' no existe en el AudioMixer.");
            }
        }
        else
        {
            Debug.LogError("AudioMixer no est� asignado en AudioManager.");
        }
    }

    public void SaveVolumeSettings()
    {
        // Obtener los valores actuales del AudioMixer y guardarlos
        float masterVolume;
        float musicVolume;

        if (audioMixer.GetFloat(masterVolumeParam, out masterVolume))
        {
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        }
        else
        {
            Debug.LogError($"El par�metro '{masterVolumeParam}' no existe en el AudioMixer.");
        }

        if (audioMixer.GetFloat(musicVolumeParam, out musicVolume))
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        }
        else
        {
            Debug.LogError($"El par�metro '{musicVolumeParam}' no existe en el AudioMixer.");
        }

        PlayerPrefs.Save();
    }
}
