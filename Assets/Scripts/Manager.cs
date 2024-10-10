using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Manager : MonoBehaviour
{
    private ManagerData managerData;
    private bool isHired = false;
    private Business targetBusiness;

    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem idleFX;
    [SerializeField] private ParticleSystem hiredFX;
    [SerializeField] private NavMeshAgent navMeshAgent;

    private Coroutine findBusinessCoroutine;

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
            if (idleFX != null) idleFX.Stop();
            if (hiredFX != null) hiredFX.Play();
            if (animator != null) animator.SetBool("IsHired", true);

            AutomateBusiness();
        }
        else
        {
            if (idleFX != null) idleFX.Play();
            if (animator != null) animator.SetBool("IsHired", false);
            
            if (findBusinessCoroutine != null)
            {
                StopCoroutine(findBusinessCoroutine);
                findBusinessCoroutine = null;
            }
        }
    }

    private void AutomateBusiness()
    {
        targetBusiness = FindNearestBusinessToAutomate();

        if (targetBusiness != null)
        {
            MoveToTargetBusiness();
        }
        else
        {
            Debug.Log("No se encontró un negocio para automatizar. Buscando periódicamente...");
            if (findBusinessCoroutine == null)
            {
                findBusinessCoroutine = StartCoroutine(FindBusinessPeriodically());
            }
        }
    }

    private IEnumerator FindBusinessPeriodically()
    {
        while (targetBusiness == null)
        {
            yield return new WaitForSeconds(1f);
            targetBusiness = FindNearestBusinessToAutomate();
            if (targetBusiness != null)
            {
                Debug.Log("Se encontró un negocio para automatizar.");
                MoveToTargetBusiness();
                break;
            }
        }
    }

    private void MoveToTargetBusiness()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(targetBusiness.transform.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent no está asignado en el Manager.");
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
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                targetBusiness.AutomateWithManager(this);
                navMeshAgent.isStopped = true;
                
                if (findBusinessCoroutine != null)
                {
                    StopCoroutine(findBusinessCoroutine);
                    findBusinessCoroutine = null;
                }
            }
        }
    }

    public ManagerData GetManagerData()
    {
        return managerData;
    }
}