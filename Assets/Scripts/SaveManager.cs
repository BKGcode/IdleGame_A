using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public SaveData[] saveSlots; // Array de ScriptableObjects para los 4 slots de guardado
    public LifeData lifeData;  // Referencia al ScriptableObject de vidas
    public TimeData timeData;  // Referencia al ScriptableObject de tiempo
    public ScoreData scoreData;  // Referencia al ScriptableObject de puntos

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

    private void Start()
    {
        // Cargamos la partida del primer slot por defecto, o inicializamos si no hay datos.
        if (SaveSystem.LoadGame(saveSlots[0]) == null)
        {
            ResetGame(lifeData.maxLives);
        }
        else
        {
            ApplyLoadedData(0);
        }
    }

    // Método para guardar los datos en el slot seleccionado
    public void SaveGame(int slotIndex)
    {
        saveSlots[slotIndex].currentLives = lifeData.currentLives;
        saveSlots[slotIndex].currentTime = timeData.currentTime;
        saveSlots[slotIndex].currentScore = scoreData.currentScore;

        SaveSystem.SaveGame(saveSlots[slotIndex]);
        Debug.Log($"Game saved successfully in slot {slotIndex}.");
    }

    // Método para cargar datos de un slot específico en el juego
    public void ApplyLoadedData(int slotIndex)
    {
        lifeData.currentLives = saveSlots[slotIndex].currentLives;
        timeData.currentTime = saveSlots[slotIndex].currentTime;
        scoreData.currentScore = saveSlots[slotIndex].currentScore;

        // Actualiza las UI correspondientes
        lifeData.onLifeLost.Invoke();  
        timeData.onTimeChanged.Invoke();  
        scoreData.onScoreChanged.Invoke();  

        Debug.Log($"Game data loaded successfully from slot {slotIndex}.");
    }

    // Método para cargar los datos desde un slot específico y aplicarlos
    public void LoadGameFromSlot(int slotIndex)
    {
        if (SaveSystem.LoadGame(saveSlots[slotIndex]) != null)
        {
            ApplyLoadedData(slotIndex);
        }
    }

    // Verifica si un slot tiene datos guardados
    public bool IsSlotUsed(int slotIndex)
    {
        return SaveSystem.LoadGame(saveSlots[slotIndex]) != null;
    }

    // Método para resetear el juego, cargando los datos iniciales en el primer slot
    public void ResetGame(int maxLives)
    {
        saveSlots[0].ResetData(maxLives); // Resetea el primer slot por defecto
        ApplyLoadedData(0);
        Debug.Log("Game reset successfully.");
    }

    // Método para crear una nueva partida en un slot específico
    public void CreateNewGame(int slotIndex)
    {
        saveSlots[slotIndex].ResetData(lifeData.maxLives);
        SaveGame(slotIndex);
    }

    // Método para cargar la información de todos los slots (para mostrar en la UI)
    public void LoadGameData()
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            var data = SaveSystem.LoadGame(saveSlots[i]);
            if (data != null)
            {
                Debug.Log($"Slot {i}: Lives - {saveSlots[i].currentLives}, Time - {saveSlots[i].currentTime}, Score - {saveSlots[i].currentScore}");
            }
            else
            {
                Debug.Log($"Slot {i}: Empty");
            }
        }
    }
}
