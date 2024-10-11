using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Manager : MonoBehaviour
{
    [SerializeField] private ManagerData managerData;
    private bool isHired = false;
    private Business targetBusiness;
    
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
            StartCoroutine(FindBusinessPeriodically());
        }
        else
        {
            StopFindingBusiness();
        }
    }

    private IEnumerator FindBusinessPeriodically()
    {
        while (isHired && targetBusiness == null)
        {
            yield return new WaitForSeconds(1f);
            targetBusiness = FindBusinessToAutomate();
            if (targetBusiness != null)
            {
                MoveToTargetBusiness();
            }
        }
    }

    private void StopFindingBusiness()
    {
        if (findBusinessCoroutine != null)
        {
            StopCoroutine(findBusinessCoroutine);
            findBusinessCoroutine = null;
        }
    }

    private Business FindBusinessToAutomate()
    {
        Business[] allBusinesses = FindObjectsOfType<Business>();
        foreach (Business business in allBusinesses)
        {
            if (business.GetBusinessData() == managerData.businessToAutomate && !business.IsAutomated())
            {
                return business;
            }
        }
        return null;
    }

    private void MoveToTargetBusiness()
    {
        if (navMeshAgent != null && targetBusiness != null)
        {
            navMeshAgent.SetDestination(targetBusiness.transform.position);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent o targetBusiness es null en MoveToTargetBusiness");
        }
    }

    private void Update()
    {
        if (isHired && targetBusiness != null && navMeshAgent != null)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                AutomateBusiness();
            }
        }
    }

    private void AutomateBusiness()
    {
        if (targetBusiness != null)
        {
            targetBusiness.AutomateWithManager(this);
            navMeshAgent.isStopped = true;
            Debug.Log($"Manager ha automatizado el negocio: {targetBusiness.GetBusinessData().businessName}");
            targetBusiness = null;
            StartCoroutine(FindBusinessPeriodically());
        }
        else
        {
            Debug.LogWarning("Intento de automatizar un negocio null");
        }
    }

    public ManagerData GetManagerData()
    {
        return managerData;
    }

    private void OnDisable()
    {
        StopFindingBusiness();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (targetBusiness != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, targetBusiness.transform.position);
        }
    }
#endif
}