using UnityEngine;
using System;

namespace EnvironmentSystem
{
    [ExecuteAlways]
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Tiempo")]
        [Range(0, 24)] public float timeOfDay;
        public float dayDuration = 24f; // Duración de un día completo en minutos reales

        [Header("Sol")]
        public Light sun;
        public Gradient sunColor;
        public AnimationCurve sunIntensity;

        [Header("Luna")]
        public Light moon;
        public Gradient moonColor;
        public AnimationCurve moonIntensity;

        [Header("Otros Ajustes")]
        public AnimationCurve atmosphereThickness;
        public AnimationCurve starIntensity;

        [Header("Skybox")]
        public Material skyboxMaterial;
        public float skyboxBlendSpeed = 0.02f;

        // Eventos
        public event Action<float> OnTimeChanged;
        public event Action OnDayStart;
        public event Action OnNightStart;

        private float lastTimeOfDay = -1;

        private void Update()
        {
            if (Application.isPlaying)
            {
                // Avanzar el tiempo
                timeOfDay += (Time.deltaTime / (dayDuration * 60)) * 24f;
                if (timeOfDay >= 24)
                {
                    timeOfDay -= 24;
                }
            }

            // Actualizar el ciclo
            UpdateDayNightCycle();
        }

        private void UpdateDayNightCycle()
        {
            // Asegurarse de que tenemos todas las referencias necesarias
            if (sun == null || moon == null || skyboxMaterial == null) return;

            // Calcular la rotación del sol y la luna
            float sunRotation = timeOfDay * 15f; // 15 grados por hora
            float moonRotation = sunRotation + 180f;

            // Rotar el sol y la luna
            sun.transform.rotation = Quaternion.Euler(sunRotation, 0, 0);
            moon.transform.rotation = Quaternion.Euler(moonRotation, 0, 0);

            // Actualizar colores e intensidades
            sun.color = sunColor.Evaluate(timeOfDay / 24f);
            sun.intensity = sunIntensity.Evaluate(timeOfDay / 24f);

            moon.color = moonColor.Evaluate(timeOfDay / 24f);
            moon.intensity = moonIntensity.Evaluate(timeOfDay / 24f);

            // Actualizar skybox
            skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphereThickness.Evaluate(timeOfDay / 24f));
            skyboxMaterial.SetFloat("_SunSize", 0.04f + (0.02f * Mathf.Sin(timeOfDay * Mathf.PI))); // Cambio sutil en el tamaño del sol

            // Estrellas (asumiendo que usas el shader estándar de Unity para el skybox)
            skyboxMaterial.SetFloat("_StarsIntensity", starIntensity.Evaluate(timeOfDay / 24f));

            // Disparar eventos
            if (Mathf.FloorToInt(timeOfDay) != Mathf.FloorToInt(lastTimeOfDay))
            {
                OnTimeChanged?.Invoke(timeOfDay);

                if (timeOfDay < lastTimeOfDay)
                {
                    // Nuevo día
                    OnDayStart?.Invoke();
                }
                else if (timeOfDay >= 18 && lastTimeOfDay < 18)
                {
                    // Noche (6 PM)
                    OnNightStart?.Invoke();
                }
            }

            lastTimeOfDay = timeOfDay;
        }

        // Métodos públicos para control externo
        public void SetTime(float newTime)
        {
            timeOfDay = Mathf.Clamp(newTime, 0f, 24f);
        }

        public float GetTime()
        {
            return timeOfDay;
        }
    }
}