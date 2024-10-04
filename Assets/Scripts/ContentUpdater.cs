using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ContentUpdater : MonoBehaviour
{
    private string eventFilePath;

    private void Awake()
    {
        eventFilePath = Path.Combine(Application.streamingAssetsPath, "events.json");
    }

    // Llamar a LoadEvents() cuando el juego comienza
    private void Start()
    {
        LoadEvents();
    }

    // Método para cargar eventos desde un archivo JSON
    public void LoadEvents()
    {
        if (File.Exists(eventFilePath))
        {
            string json = File.ReadAllText(eventFilePath);
            List<EventManager.GameEvent> newEvents = JsonUtility.FromJson<EventList>(json).events;
            foreach (var gameEvent in newEvents)
            {
                EventManager.Instance.AddNewEvent(gameEvent); // Añadir eventos al EventManager
            }
            Debug.Log("Eventos cargados desde archivo JSON.");
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de eventos.");
        }
    }

    [System.Serializable]
    public class EventList
    {
        public List<EventManager.GameEvent> events;
    }
}
