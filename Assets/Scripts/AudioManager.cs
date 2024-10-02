using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;  // Singleton

    public AudioMixer audioMixer;  // Asigna el MasterMixer desde el Inspector

    private void Awake()
    {
        // Implementación del patrón Singleton para mantener una sola instancia
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // No destruir al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Cargar y aplicar los ajustes de volumen
        LoadAndApplyVolumeSettings();
    }

    private void LoadAndApplyVolumeSettings()
    {
        // Cargar los valores guardados o establecer valores por defecto
        int soundVolume = PlayerPrefs.GetInt("SoundVolume", 10);
        int musicVolume = PlayerPrefs.GetInt("MusicVolume", 10);

        // Aplicar los volúmenes
        SetSoundVolume(soundVolume);
        SetMusicVolume(musicVolume);
    }

    public void SetSoundVolume(float volume)
    {
        // Convertir el volumen de 0-10 a decibelios (-80dB a 0dB)
        float dB = (volume == 0) ? -80f : Mathf.Lerp(-80f, 0f, volume / 10f);
        audioMixer.SetFloat("SoundVolume", dB);
    }

    public void SetMusicVolume(float volume)
    {
        // Convertir el volumen de 0-10 a decibelios (-80dB a 0dB)
        float dB = (volume == 0) ? -80f : Mathf.Lerp(-80f, 0f, volume / 10f);
        audioMixer.SetFloat("MusicVolume", dB);
    }
}
