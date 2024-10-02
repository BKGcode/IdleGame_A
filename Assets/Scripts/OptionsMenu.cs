using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    // Referencias a los controles de la UI
    public GameObject optionsPopup;          // Panel de opciones
    public TMP_Dropdown screenModeDropdown;  // Dropdown para seleccionar el modo de pantalla
    public Slider soundSlider;               // Slider para el sonido
    public Slider musicSlider;               // Slider para la música
    public TMP_Dropdown qualityDropdown;     // Dropdown para cambiar la calidad gráfica
    public Button closeOptionsButton;        // Botón para cerrar el panel de opciones

    private void Start()
    {
        // Aseguramos que el pop-up de opciones esté oculto al inicio
        optionsPopup.SetActive(false);

        // Inicializamos los controles y listeners
        InitializeOptions();

        // Asignar el método CloseOptions al evento onClick del botón
        closeOptionsButton.onClick.AddListener(CloseOptions);
    }

    private void InitializeOptions()
    {
        // Inicializar el dropdown de pantalla completa
        screenModeDropdown.value = Screen.fullScreen ? 1 : 0;

        // Inicializar los sliders con los valores guardados (PlayerPrefs)
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 5);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 5);

        // Inicializar el dropdown de calidad gráfica
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        // Añadir listeners a los controles
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    // Mostrar el menú de opciones
    public void ShowOptions()
    {
        optionsPopup.SetActive(true);  // Mostrar el pop-up
    }

    // Cerrar el menú de opciones
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
        // Aquí puedes añadir la lógica para ajustar el volumen de los sonidos del juego
    }

    // Ajustar el volumen de la música
    public void SetMusicVolume(float volume)
    {
        Debug.Log("Música ajustada a: " + volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        // Aquí puedes añadir la lógica para ajustar el volumen de la música del juego
    }

    // Cambiar la calidad gráfica según el valor del dropdown
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Calidad gráfica ajustada a: " + qualityIndex);
    }
}
