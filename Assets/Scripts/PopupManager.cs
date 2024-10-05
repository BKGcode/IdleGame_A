using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPopup;
    [SerializeField] private TextMeshProUGUI gameOverPointsText;
    [SerializeField] private TextMeshProUGUI gameOverMoneyText;
    [SerializeField] private Button restartButton;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameOverPopup.SetActive(false); // Asegurarse de que el popup est� oculto al inicio

        // A�adir listener para el bot�n de reinicio
        restartButton.onClick.AddListener(RestartGame);
    }

    // Mostrar el popup de GameOver
    public void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);

        // Actualizar los textos con la informaci�n del GameManager
        gameOverPointsText.text = gameManager.GetPoints().ToString();
        gameOverMoneyText.text = gameManager.GetMoney().ToString();
    }

    private void RestartGame()
    {
        gameManager.RestartGame(); // Reiniciar el juego
    }
}
