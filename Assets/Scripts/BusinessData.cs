using UnityEngine;

[CreateAssetMenu(fileName = "New Business Data", menuName = "Business Data")]
public class BusinessData : ScriptableObject
{
    public string businessName;
    public Sprite icon;              // Campo para el icono
    public float baseIncome;         // Ingreso base del negocio
    public float baseIncomeInterval; // Intervalo de tiempo para generar ingresos
    public double hiringCost;        // Costo de contratación del negocio
}
