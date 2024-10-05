using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public static LifeSystem Instance { get; private set; }
    public int maxLives = 3;
    public int currentLives;

    [SerializeField] private UIManager uiManager; // Referencia a UIManager para actualizar la UI de vidas
    [SerializeField] private PopupManager popupManager; // Referencia al PopupManager para mostrar GameOver

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetLives();  // Llamar para asegurarse que las vidas se inicializan correctamente
    }

    // Resetear vidas al valor máximo y actualizar la UI al inicio del juego
    public void ResetLives()
    {
        currentLives = maxLives;
        uiManager.UpdateLivesUI(currentLives); // Asegurarse de que la UI se actualice correctamente
    }

    public void LoseLife(int damage = 1)
    {
        currentLives -= damage;
        uiManager.UpdateLivesUI(currentLives); // Actualizar UI

        if (currentLives <= 0)
        {
            GameOver(); // Mostrar GameOver cuando las vidas lleguen a 0
        }
    }

    public void GainLife(int amount = 1)
    {
        currentLives += amount;
        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }
        uiManager.UpdateLivesUI(currentLives); // Actualizar UI
    }

    // Método para manejar el fin del juego
    private void GameOver()
    {
        popupManager.ShowGameOverPopup(); // Mostrar el popup de GameOver
    }
}
