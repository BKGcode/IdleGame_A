// Assets/Scripts/UI/FarmResourceListener.cs
using UnityEngine;

public class FarmResourceListener : MonoBehaviour
{
    [SerializeField] private ShooterData shooterData;
    [SerializeField] private FarmData farmData;

    private void OnEnable()
    {
        shooterData.OnResourcesCollected += AddResourcesToFarm;
    }

    private void OnDisable()
    {
        shooterData.OnResourcesCollected -= AddResourcesToFarm;
    }

    private void AddResourcesToFarm(int amount)
    {
        farmData.AddResources(amount);
    }
}
