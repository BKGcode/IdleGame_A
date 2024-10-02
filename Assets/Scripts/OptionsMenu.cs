using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    // Referencias a los controles de la UI
    public GameObject optionsPopup;          // Panel de opciones
    public TMP_Dropdown screenModeDropdown;  // Dropdown para seleccionar el modo de pantalla
    public Slider soundSlider;               // Slider para el sonido
    public Slider musicSlider;               // Slider para la m�sica
    public TMP_Dropdown qualityDropdown;     // Dropdown para cambiar la calidad gr�fica
    public Button closeOptionsButton;        // Bot�n para cerrar el panel de opciones

    private void Start()
    {
        // Aseguramos que el pop-up de opciones est� oculto al inicio
        optionsPopup.SetActive(false);

        // Inicializamos los controles y listeners
        InitializeOptions();

        // Asignar el m�todo CloseOptions al evento onClick del bot�n
        closeOptionsButton.onClick.AddListener(CloseOptions);
    }

    private void InitializeOptions()
    {
        // Inicializar el dropdown de pantalla completa
        screenModeDropdown.value = Screen.fullScreen ? 1 : 0;

        // Inicializar los sliders con los valores guardados (PlayerPrefs)
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 5);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 5);

        // Inicializar el dropdown de calidad gr�fica
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        // A�adir listeners a los controles
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    // Mostrar el men� de opciones
    public void ShowOptions()
    {
        optionsPopup.SetActive(true);  // Mostrar el pop-up
    }

    // Cerrar el men� de opciones
    public void CloseOptions()
    {
        optionsPopup.SetActive(false);  // Ocultar el pop-up
    }

    // Cambiar entre ventana y pantalla completa
    public void SetScreenMode(int mode)
    {
        Screen.fullScreen = mode == 1;  // 1 es pantalla completa, 0 es ventana
    }

    // Ajustar el volumen del sonido
    public void SetSoundVolume(float volume)
    {
        Debug.Log("Sonido ajustado a: " + volume);
        PlayerPrefs.SetFloat("SoundVolume", volume);
        // Aqu� puedes a�adir la l�gica para ajustar el volumen de los sonidos del juego
    }

    // Ajustar el volumen de la m�sica
    public void SetMusicVolume(float volume)
    {
        Debug.Log("M�sica ajustada a: " + volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        // Aqu� puedes a�adir la l�gica para ajustar el volumen de la m�sica del juego
    }

    // Cambiar la calidad gr�fica seg�n el valor del dropdown
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Calidad gr�fica ajustada a: " + qualityIndex);
    }
}
