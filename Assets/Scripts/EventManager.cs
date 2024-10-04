using UnityEngine;
using System;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    [Serializable]
    public class GameEvent
    {
        public string eventName;
        public DateTime startTime;
        public DateTime endTime;
        public int rewardMultiplier;
        public bool isActive;
    }

    public List<GameEvent> activeEvents = new List<GameEvent>(); // Lista de eventos activos

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckActiveEvents(); // Verifica los eventos activos
    }

    // Método para activar eventos si están en el rango de tiempo
    private void CheckActiveEvents()
    {
        foreach (GameEvent gameEvent in activeEvents)
        {
            if (DateTime.Now >= gameEvent.startTime && DateTime.Now <= gameEvent.endTime)
            {
                if (!gameEvent.isActive)
                {
                    ActivateEvent(gameEvent);
                }
            }
            else if (gameEvent.isActive)
            {
                EndEvent(gameEvent);
            }
        }
    }

    // Método para activar un evento
    private void ActivateEvent(GameEvent gameEvent)
    {
        gameEvent.isActive = true;
        Debug.Log("Evento activado: " + gameEvent.eventName);
        // Aplica las recompensas del evento (por ejemplo, multiplicador de recompensas)
    }

    // Método para finalizar un evento
    private void EndEvent(GameEvent gameEvent)
    {
        gameEvent.isActive = false;
        Debug.Log("Evento finalizado: " + gameEvent.eventName);
        // Finaliza las recompensas del evento
    }

    // Método para añadir nuevos eventos (puede ser llamado desde ContentUpdater)
    public void AddNewEvent(GameEvent newEvent)
    {
        activeEvents.Add(newEvent);
        Debug.Log("Nuevo evento añadido: " + newEvent.eventName);
    }
}
