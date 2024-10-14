// Assets/Scripts/ScriptableObjects/CollectableResourceData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectableResource", menuName = "ScriptableObjects/CollectableResourceData")]
public class CollectableResourceData : ScriptableObject
{
    public string resourceName;
    public int amount;
    public Sprite resourceIcon;
}
