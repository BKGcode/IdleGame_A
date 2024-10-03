using UnityEngine; // Aseg�rate de incluir esta directiva
using System;

public static class GameEvents
{
    // Evento para cuando el jugador recibe da�o
    public static event Action<int> OnPlayerDamaged;

    // Evento para cuando el jugador se cura
    public static event Action<int> OnPlayerHealed;

    // Evento para cuando el jugador muere
    public static event Action OnPlayerDeath;

    // M�todos para emitir los eventos
    public static void PlayerDamaged(int damage)
    {
        OnPlayerDamaged?.Invoke(damage);
    }

    public static void PlayerHealed(int amount)
    {
        OnPlayerHealed?.Invoke(amount);
    }

    public static void PlayerDied()
    {
        OnPlayerDeath?.Invoke();
    }
}
