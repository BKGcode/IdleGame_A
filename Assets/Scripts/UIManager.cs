using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI moneyText; // Text element for money
    [SerializeField] private TextMeshProUGUI pointsText; // Text element for points
    [SerializeField] private TextMeshProUGUI timePlayedText; // Text element for time played

    private void Start()
    {
        UpdateMoneyText();
        UpdatePointsText();
        UpdateTimePlayedText();
    }

    private void Update()
    {
        UpdateTimePlayedText();
    }

    private void UpdateMoneyText()
    {
        if (GameManager.Instance != null)
        {
            moneyText.text = GameManager.Instance.GetMoney().ToString();
        }
    }

    private void UpdatePointsText()
    {
        if (GameManager.Instance != null)
        {
            pointsText.text = GameManager.Instance.GetPoints().ToString();
        }
    }

    private void UpdateTimePlayedText()
    {
        if (GameManager.Instance != null)
        {
            float timePlayed = GameManager.Instance.GetTimePlayed();
            int minutes = Mathf.FloorToInt(timePlayed / 60F);
            int seconds = Mathf.FloorToInt(timePlayed % 60F);
            timePlayedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}
