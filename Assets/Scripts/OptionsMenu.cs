using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    // Referencias a los controles de la UI asignadas desde el Inspector
    public GameObject optionsPopup;          // Panel de opciones
    public TMP_Dropdown screenModeDropdown;  // Dropdown para seleccionar el modo de pantalla
    public Slider soundSlider;               // Slider para el volumen del sonido
    public Slider musicSlider;               // Slider para el volumen de la música
    public TMP_Dropdown qualityDropdown;     // Dropdown para cambiar la calidad gráfica
    public Button closeOptionsButton;        // Botón para cerrar el panel de opciones

    private bool isInitialized = false;      // Bandera para verificar si ya se inicializó

    private void Awake()
    {
        // Activamos temporalmente el optionsPopup para inicializar los componentes
        bool wasActive = optionsPopup.activeSelf;
        if (!wasActive)
        {
            optionsPopup.SetActive(true);
        }

        // Asignamos los listeners y realizamos la inicialización necesaria
        AssignListeners();

        // Restauramos el estado original del optionsPopup
        if (!wasActive)
        {
            optionsPopup.SetActive(false);
        }
    }

    private void AssignListeners()
    {
        // Asignar el método CloseOptions al evento onClick del botón
        if (closeOptionsButton != null)
        {
            closeOptionsButton.onClick.AddListener(CloseOptions);
        }
        else
        {
            Debug.LogError("closeOptionsButton no está asignado en el Inspector.");
        }

        // Añadir listeners a los controles si aún no se han asignado
        if (!isInitialized)
        {
            // Configurar sliders
            soundSlider.minValue = 0;
            soundSlider.maxValue = 10;
            soundSlider.wholeNumbers = true;

            musicSlider.minValue = 0;
            musicSlider.maxValue = 10;
            musicSlider.wholeNumbers = true;

            // Cargar valores guardados
            soundSlider.value = PlayerPrefs.GetInt("SoundVolume", 10);
            musicSlider.value = PlayerPrefs.GetInt("MusicVolume", 10);

            // Inicializar el dropdown de pantalla completa
            screenModeDropdown.value = Screen.fullScreen ? 1 : 0;

            // Inicializar el dropdown de calidad gráfica
            qualityDropdown.value = QualitySettings.GetQualityLevel();

            // Añadir listeners a los controles
            screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
            soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            qualityDropdown.onValueChanged.AddListener(SetQuality);

            isInitialized = true;
            Debug.Log("Opciones inicializadas y listeners asignados.");
        }
    }

    // Mostrar el menú de opciones
    public void ShowOptions()
    {
        optionsPopup.SetActive(true);  // Mostrar el panel de opciones
    }

    // Cerrar el menú de opciones
    public void CloseOptions()
    {
        optionsPopup.SetActive(false);  // Ocultar el panel de opciones
    }

    // Cambiar entre ventana y pantalla completa
    public void SetScreenMode(int mode)
    {
        Screen.fullScreen = mode == 1;  // 1 es pantalla completa, 0 es ventana
    }

    // Método llamado cuando el slider de sonido cambia
    public void OnSoundSliderChanged(float value)
    {
        Debug.Log("Volumen del sonido ajustado a: " + value);
        PlayerPrefs.SetInt("SoundVolume", (int)value);
        AudioManager.instance.SetSoundVolume(value);
    }

    // Método llamado cuando el slider de música cambia
    public void OnMusicSliderChanged(float value)
    {
        Debug.Log("Volumen de la música ajustado a: " + value);
        PlayerPrefs.SetInt("MusicVolume", (int)value);
        AudioManager.instance.SetMusicVolume(value);
    }

    // Cambiar la calidad gráfica según el valor del dropdown
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Calidad gráfica ajustada a: " + qualityIndex);
    }
}
