// Assets/Scripts/Player/PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ShooterData shooterData;

    // Método para recoger recursos
    public void CollectResource(int amount)
    {
        playerData.AddResources(amount);
        shooterData.CollectResources(amount);
    }

    // Otros métodos relacionados con el jugador (movimiento, disparo, etc.)
}
