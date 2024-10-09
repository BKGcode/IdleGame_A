using UnityEngine;
using EnvironmentSystem;

public class AtmosphericEffects : MonoBehaviour
{
    public ParticleSystem cloudSystem;
    public ParticleSystem fogSystem;

    private DayNightCycle dayNightCycle;

    private void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        if (dayNightCycle != null)
        {
            dayNightCycle.OnTimeChanged += UpdateAtmosphericEffects;
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
            dayNightCycle.OnTimeChanged -= UpdateAtmosphericEffects;
        }
    }

    private void UpdateAtmosphericEffects(float time)
    {
        if (cloudSystem != null)
        {
            var emission = cloudSystem.emission;
            emission.rateOverTime = Mathf.Lerp(10, 50, Mathf.PingPong(time / 24f, 1));
        }

        if (fogSystem != null)
        {
            var emission = fogSystem.emission;
            emission.rateOverTime = time > 18 || time < 6 ? 50 : 10; // Más niebla en la noche
        }
    }
}