// Assets/Scripts/UI/WeaponUI.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private Image weaponIconImage;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void OnEnable()
    {
        playerData.OnWeaponChanged += UpdateWeaponUI;
        playerData.OnAmmoChanged += UpdateAmmoUI;
    }

    private void OnDisable()
    {
        playerData.OnWeaponChanged -= UpdateWeaponUI;
        playerData.OnAmmoChanged -= UpdateAmmoUI;
    }

    private void UpdateWeaponUI(WeaponData newWeapon)
    {
        if (newWeapon != null)
        {
            weaponNameText.text = $"Arma: {newWeapon.weaponName}";
            weaponIconImage.sprite = newWeapon.weaponIcon;
            UpdateAmmoUI(playerData.Ammo);
        }
        else
        {
            weaponNameText.text = "Arma: Ninguna";
            weaponIconImage.sprite = null;
            ammoText.text = $"Munición: 0/0";
        }
    }

    private void UpdateAmmoUI(int newAmmo)
    {
        if (playerData.CurrentWeapon != null)
        {
            ammoText.text = $"Munición: {newAmmo}/{playerData.CurrentWeapon.magazineSize}";
        }
        else
        {
            ammoText.text = $"Munición: 0/0";
        }
    }
}
