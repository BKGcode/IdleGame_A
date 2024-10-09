using UnityEngine;

[CreateAssetMenu(fileName = "NewManagerType", menuName = "Manager/New Manager Type")]
public class ManagerType : ScriptableObject
{
    public string managerName; // Nombre del manager
    public double hiringCost; // Coste para contratar este manager
    public BusinessType businessType; // Tipo de negocio que el manager puede automatizar
    public float bonusAmount; // Cantidad de bonificación que el manager otorga
}
