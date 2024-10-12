using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

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
        if (isHired == hired) return; // Evitar cambios innecesarios

        isHired = hired;
        if (isHired)
        {
            BusinessManagerTracker.Instance.RegisterHiredManager(this);
            StartCoroutine(FindBusinessPeriodically());
            Debug.Log($"Manager {managerData.managerName} ha sido contratado.");
        }
        else
        {
            StopFindingBusiness();
            BusinessManagerTracker.Instance.UnregisterHiredManager(this);
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
            Debug.Log($"Manager {managerData.managerName} ha sido descontratado.");
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
        List<Business> hiredBusinesses = BusinessManagerTracker.Instance.GetHiredBusinesses();
        foreach (Business business in hiredBusinesses)
        {
            if (business.GetBusinessData() == managerData.businessToAutomate 
                && !business.IsAutomated())
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
            Debug.Log($"Manager {managerData.managerName} se dirige a {targetBusiness.GetBusinessData().businessName}");
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
            Debug.Log($"Manager {managerData.managerName} ha automatizado el negocio: {targetBusiness.GetBusinessData().businessName}");
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
        SetHired(false);
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
