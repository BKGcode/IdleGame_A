using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    private WeaponBehaviour currentWeapon;

    public void EquipWeapon(WeaponData newWeaponData)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        GameObject weaponObject = Instantiate(newWeaponData.weaponPrefab, weaponHolder);
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;

        currentWeapon = weaponObject.GetComponent<WeaponBehaviour>();
        currentWeapon.weaponData = newWeaponData;
    }

    private void Update()
    {
        if (currentWeapon != null && Input.GetButton("Fire1"))
        {
            currentWeapon.Fire();
        }
    }
}