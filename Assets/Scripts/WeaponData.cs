using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    public float fireRate;
    public int magazineSize;
    public int totalAmmo;
    public float reloadTime;
}