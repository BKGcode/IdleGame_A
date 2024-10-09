using UnityEngine;
using EnvironmentSystem;

public class DayNightEnvironmentAdjuster : MonoBehaviour
{
    public Light[] streetLights;
    public AudioSource[] nightAmbience;
    public AudioSource[] dayAmbience;

    private DayNightCycle dayNightCycle;

    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        if (dayNightCycle != null)
        {
            dayNightCycle.OnDayStart += HandleDayStart;
            dayNightCycle.OnNightStart += HandleNightStart;
        }
        else
        {
            Debug.LogError("DayNightCycle component not found in the scene.");
        }
    }

    private void OnDestroy()
    {
        if (dayNightCycle != null)
        {
            dayNightCycle.OnDayStart -= HandleDayStart;
            dayNightCycle.OnNightStart -= HandleNightStart;
        }
    }

    private void HandleDayStart()
    {
        foreach (var light in streetLights)
        {
            if (light != null)
                light.enabled = false;
        }

        foreach (var audio in nightAmbience)
        {
            if (audio != null)
                audio.Stop();
        }

        foreach (var audio in dayAmbience)
        {
            if (audio != null)
                audio.Play();
        }
    }

    private void HandleNightStart()
    {
        foreach (var light in streetLights)
        {
            if (light != null)
                light.enabled = true;
        }

        foreach (var audio in dayAmbience)
        {
            if (audio != null)
                audio.Stop();
        }

        foreach (var audio in nightAmbience)
        {
            if (audio != null)
                audio.Play();
        }
    }
}