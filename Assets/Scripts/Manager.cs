using UnityEngine;
using UnityEngine.AI;

public class Manager : MonoBehaviour
{
    private ManagerData managerData;
    private bool isHired = false;
    private Business targetBusiness;

    // Referencias a componentes para FX y animaciones
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem idleFX;
    [SerializeField] private ParticleSystem hiredFX;
    [SerializeField] private NavMeshAgent navMeshAgent;

    public void Initialize(ManagerData data, bool hired)
    {
        managerData = data;
        SetHired(hired);
    }

    public void SetHired(bool hired)
    {
        isHired = hired;

        if (isHired)
        {
            // Activar animaciones o efectos de manager contratado
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

            // Comenzar a automatizar el negocio
            AutomateBusiness();
        }
        else
        {
            // Mostrar efectos de idle para manager no contratado
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

    private void AutomateBusiness()
    {
        // Buscar el negocio más cercano que el manager puede automatizar
        targetBusiness = FindNearestBusinessToAutomate();

        if (targetBusiness != null)
        {
            // Moverse hacia el negocio
            if (navMeshAgent != null)
            {
                navMeshAgent.SetDestination(targetBusiness.transform.position);
            }
            else
            {
                Debug.LogError("NavMeshAgent no está asignado en el Manager.");
            }
        }
        else
        {
            Debug.LogError("No se encontró un negocio para automatizar.");
        }
    }

    private Business FindNearestBusinessToAutomate()
    {
        Business[] allBusinesses = FindObjectsOfType<Business>();
        Business nearestBusiness = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Business business in allBusinesses)
        {
            if (business.GetBusinessData() == managerData.businessToAutomate && business.IsHired())
            {
                float distance = Vector3.Distance(transform.position, business.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestBusiness = business;
                }
            }
        }

        return nearestBusiness;
    }

    private void Update()
    {
        if (isHired && targetBusiness != null && navMeshAgent != null)
        {
            // Verificar si ha llegado al destino
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Automatizar el negocio
                targetBusiness.AutomateWithManager(this);
                // Detener el movimiento
                navMeshAgent.isStopped = true;
                // Puedes añadir animaciones o efectos adicionales aquí
            }
        }
    }

    public ManagerData GetManagerData()
    {
        return managerData;
    }
}
