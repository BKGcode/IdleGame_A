using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public SaveData[] saveSlots; // Array de ScriptableObjects para los slots de guardado
    public LifeData lifeData;  // Referencia al ScriptableObject de vidas
    public TimeData timeData;  // Referencia al ScriptableObject de tiempo
    public ScoreData scoreData;  // Referencia al ScriptableObject de puntos

    public string saveSuccessMessage; // Mensaje configurable para mostrar cuando se guarda correctamente
    public string loadSuccessMessage; // Mensaje configurable para mostrar cuando se carga correctamente
    public string resetSuccessMessage; // Mensaje configurable para mostrar cuando se resetea un slot

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el SaveManager entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruye la instancia duplicada si ya existe una
        }
    }

    // Crea una nueva partida en un slot específico
    public void CreateNewGame(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlots.Length)
        {
            Debug.LogError($"Índice de slot {slotIndex} fuera de los límites del array saveSlots.");
            return;
        }

        saveSlots[slotIndex].ResetData(lifeData.maxLives);

        // Aplica las vidas restauradas para asegurar que el jugador comienza con vidas completas
        lifeData.currentLives = lifeData.maxLives;
        lifeData.onLifeGained.Invoke(); // Actualiza la UI de vidas

        // Asegura que el tiempo también se reinicie
        timeData.currentTime = 0f;
        timeData.onTimeChanged.Invoke(); // Actualiza la UI del tiempo

        SaveGame(slotIndex);
        Debug.Log(saveSuccessMessage); // Mensaje personalizado al guardar
    }

    // Guarda los datos actuales en el slot seleccionado
    public void SaveGame(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlots.Length)
        {
            Debug.LogError($"Índice de slot {slotIndex} fuera de los límites del array saveSlots.");
            return;
        }

        saveSlots[slotIndex].currentLives = lifeData.currentLives;
        saveSlots[slotIndex].currentTime = timeData.currentTime;
        saveSlots[slotIndex].currentScore = scoreData.currentScore;

        SaveSystem.SaveGame(saveSlots[slotIndex], slotIndex);
        Debug.Log(saveSuccessMessage); // Mensaje personalizado al guardar
    }

    // Carga los datos desde un slot específico y los aplica al juego
    public void LoadGameFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= saveSlots.Length)
        {
            Debug.LogError($"Índice de slot {slotIndex} fuera de los límites del array saveSlots.");
            return;
        }

        if (SaveSystem.LoadGame(saveSlots[slotIndex], slotIndex) != null)
        {
            ApplyLoadedData(slotIndex);
            Debug.Log(loadSuccessMessage); // Mensaje personalizado al cargar
        }
    }

    // Aplica los datos cargados del slot al juego
    private void ApplyLoadedData(int slotIndex)
    {
        lifeData.currentLives = saveSlots[slotIndex].currentLives;
        timeData.currentTime = saveSlots[slotIndex].currentTime;
        scoreData.currentScore = saveSlots[slotIndex].currentScore;

        // Invoca los eventos para actualizar las UI correspondientes
        lifeData.onLifeLost.Invoke();
        timeData.onTimeChanged.Invoke();
        scoreData.onScoreChanged.Invoke();
    }
}
