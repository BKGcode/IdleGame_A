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
        if (playerData != null)
        {
            playerData.OnWeaponChanged += UpdateWeaponUI;
            playerData.OnAmmoChanged += UpdateAmmoUI;
        }
        else
        {
            Debug.LogError("PlayerData no está asignado en WeaponUI.");
        }
    }

    private void OnDisable()
    {
        if (playerData != null)
        {
            playerData.OnWeaponChanged -= UpdateWeaponUI;
            playerData.OnAmmoChanged -= UpdateAmmoUI;
        }
    }

    /// <summary>
    /// Actualiza la UI del arma con la nueva arma equipada.
    /// </summary>
    /// <param name="newWeapon">Nueva arma equipada.</param>
    public void UpdateWeaponUI(WeaponData newWeapon)
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

    /// <summary>
    /// Actualiza la UI de la munición con la nueva cantidad de munición.
    /// </summary>
    /// <param name="newAmmo">Nueva cantidad de munición.</param>
    public void UpdateAmmoUI(int newAmmo)
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
