using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PopupController : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI costText;
    public Button hireButton;
    public Button closeButton;

    public event Action OnHireButtonClicked;
    public event Action OnCloseButtonClicked;

    public void SetPopupData(Sprite icon, string nombre, double cost)
    {
        iconImage.sprite = icon;
        nombreText.text = nombre;
        costText.text = cost.ToString("N0");
    }

    private void Start()
    {
        hireButton.onClick.AddListener(() => OnHireButtonClicked?.Invoke());
        closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
    }
}
