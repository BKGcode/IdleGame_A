using UnityEngine;

[CreateAssetMenu(fileName = "New Manager Data", menuName = "Manager Data")]
public class ManagerData : ScriptableObject
{
    public string managerName;
    public Sprite icon;                      // Campo para el icono
    public BusinessData businessToAutomate;  // Referencia al BusinessData que este Manager puede automatizar
    public float efficiencyBonus;            // Bonificación de eficiencia que aporta al negocio
    public double hiringCost;                // Costo de contratación del Manager
}
