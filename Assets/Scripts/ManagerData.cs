using UnityEngine;

[CreateAssetMenu(fileName = "New Manager Data", menuName = "Game Data/Manager Data")]
public class ManagerData : ScriptableObject
{
    public string managerName;
    public Sprite managerIcon;
    public float hiringCost;
    public BusinessData businessToAutomate;
}