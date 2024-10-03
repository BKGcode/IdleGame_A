using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    private void Start()
    {
        // Inicializar sliders con los valores actuales del AudioManager
        if (AudioManager.instance != null)
        {
            float masterVolume;
            float musicVolume;

            AudioManager.instance.audioMixer.GetFloat("Master", out masterVolume);
            AudioManager.instance.audioMixer.GetFloat("Music", out musicVolume);

            // Convertir de dB a slider (asumiendo slider rango de -80 a 0)
            masterVolumeSlider.value = masterVolume;
            musicVolumeSlider.value = musicVolume;
        }
        else
        {
            Debug.LogError("AudioManager.instance es null. Asegúrate de que AudioManager está en la escena.");
        }
    }

    public void OnMasterVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(value);
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(value);
        }
    }

    public void SaveSettings()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SaveVolumeSettings();
        }
    }
}
