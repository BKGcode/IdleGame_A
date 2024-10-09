using UnityEngine;

[CreateAssetMenu(fileName = "NewBusinessType", menuName = "Business/New Business Type")]
public class BusinessType : ScriptableObject
{
    public string businessName; // Nombre del negocio
    public float baseIncome; // Ingresos base del negocio
    public float baseIncomeInterval; // Intervalo de ingresos base
    public double hiringCost; // Coste para contratar este negocio
}
