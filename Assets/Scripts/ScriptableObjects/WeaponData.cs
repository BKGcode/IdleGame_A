// Assets/Scripts/ScriptableObjects/WeaponData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Información del Arma")]
    public string weaponName;
    public Sprite weaponIcon;

    [Header("Características de Disparo")]
    public float fireRate; // Velocidad de disparo (disparos por segundo)
    public int magazineSize; // Balas por cargador
    public int totalMagazines; // Número de cargadores
    public float bulletSpread; // Dispersión de disparo
    public float reloadTime; // Tiempo de recarga en segundos

    [Header("Daño y Alcance")]
    public int damage;
    public float range;
}
