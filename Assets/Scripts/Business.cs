using UnityEngine;

public class Business : MonoBehaviour
{
    private BusinessData businessData;
    private bool isHired = false;

    // Referencias a componentes para FX y animaciones
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem idleFX;
    [SerializeField] private ParticleSystem hiredFX;

    public void Initialize(BusinessData data, bool hired)
    {
        businessData = data;
        SetHired(hired);
    }

    public void SetHired(bool hired)
    {
        isHired = hired;

        if (isHired)
        {
            // Activar animaciones o efectos de negocio contratado
            if (idleFX != null)
            {
                idleFX.Stop();
            }
            if (hiredFX != null)
            {
                hiredFX.Play();
            }
            if (animator != null)
            {
                animator.SetBool("IsHired", true);
            }

            // Comenzar a generar ingresos u otras lógicas
            StartGeneratingIncome();
        }
        else
        {
            // Mostrar efectos de idle para negocio no contratado
            if (idleFX != null)
            {
                idleFX.Play();
            }
            if (animator != null)
            {
                animator.SetBool("IsHired", false);
            }
        }
    }

    private void StartGeneratingIncome()
    {
        // Lógica para comenzar a generar ingresos
    }

    public BusinessData GetBusinessData()
    {
        return businessData;
    }

    public bool IsHired()
    {
        return isHired;
    }

    public void AutomateWithManager(Manager manager)
    {
        // Lógica para automatizar el negocio con el manager
        Debug.Log($"El manager {manager.GetManagerData().managerName} está automatizando el negocio {businessData.businessName}.");
        // Implementa aquí la lógica de automatización
    }
}
