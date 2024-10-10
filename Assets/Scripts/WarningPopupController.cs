using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WarningPopupController : MonoBehaviour
{
    public TextMeshProUGUI warningMessageText;
    public Button closeButton;

    public event Action OnCloseButtonClicked;

    public void SetWarningMessage(string message)
    {
        warningMessageText.text = message;
    }

    private void Start()
    {
        closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
    }
}
