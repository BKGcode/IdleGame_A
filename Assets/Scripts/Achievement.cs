using UnityEngine;

public class Achievement : MonoBehaviour
{
    [Header("Achievement Details")]
    public string achievementName; // Nombre del logro
    public string description; // Descripción del logro
    public Sprite icon; // Icono del logro

    [Header("Unlock Condition")]
    public AchievementCondition condition; // Condición del logro
    public int conditionValue; // Valor requerido para cumplir la condición

    [Header("State")]
    public bool isUnlocked = false; // Estado del logro

    private void Start()
    {
        UpdateUI();
    }

    public void CheckCondition(GameManager gameManager)
    {
        if (!isUnlocked && ConditionMet(gameManager))
        {
            Unlock();
            UpdateUI();
        }
    }

    private bool ConditionMet(GameManager gameManager)
    {
        switch (condition)
        {
            case AchievementCondition.Points:
                return gameManager.GetPoints() >= conditionValue;
            case AchievementCondition.Money:
                return gameManager.GetMoney() >= conditionValue;
            case AchievementCondition.TimePlayed:
                return gameManager.GetTimePlayed() >= conditionValue;
            default:
                return false;
        }
    }

    private void Unlock()
    {
        isUnlocked = true;
        Debug.Log("Achievement Unlocked: " + achievementName);
    }

    private void UpdateUI()
    {
        Transform nameTransform = transform.Find("AchievementName");
        Transform descriptionTransform = transform.Find("AchievementDescription");
        Transform iconTransform = transform.Find("AchievementIcon");
        Transform lockOverlayTransform = transform.Find("LockOverlay");

        if (nameTransform != null)
            nameTransform.GetComponent<TMPro.TextMeshProUGUI>().text = achievementName;

        if (descriptionTransform != null)
            descriptionTransform.GetComponent<TMPro.TextMeshProUGUI>().text = description;

        if (iconTransform != null)
            iconTransform.GetComponent<UnityEngine.UI.Image>().sprite = icon;

        if (lockOverlayTransform != null)
            lockOverlayTransform.gameObject.SetActive(!isUnlocked);
    }
}

public enum AchievementCondition
{
    Points,
    Money,
    TimePlayed
}
