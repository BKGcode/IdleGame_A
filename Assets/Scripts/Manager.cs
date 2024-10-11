using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Manager : MonoBehaviour
{
    [SerializeField] private ManagerData managerData;
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
        Debug.Log($"Manager {managerData.managerName} contratado: {isHired}");

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
            if (hiredFX != null) hiredFX.Stop();
            if (animator != null) animator.SetBool("IsHired", false);
            RemoveFromBusiness();
        }
    }

    private void AutomateBusiness()
    {
        targetBusiness = FindNearestBusinessToAutomate();

        if (targetBusiness != null)
        {
            Debug.Log($"Manager {managerData.managerName} encontró un negocio para automatizar: {targetBusiness.GetBusinessData().businessName}");
            MoveToTargetBusiness();
        }
        else
        {
            Debug.Log($"Manager {managerData.managerName} no encontró un negocio para automatizar. Buscando periódicamente...");
            if (findBusinessCoroutine == null)
            {
                findBusinessCoroutine = StartCoroutine(FindBusinessPeriodically());
            }
        }
    }

    private Business FindNearestBusinessToAutomate()
    {
        Business[] allBusinesses = FindObjectsOfType<Business>();
        Business nearestBusiness = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Business business in allBusinesses)
        {
            if (business.GetBusinessData() == managerData.businessToAutomate && business.IsHired() && !business.IsAutomated())
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

    private void MoveToTargetBusiness()
    {
        if (navMeshAgent != null && targetBusiness != null)
        {
            navMeshAgent.SetDestination(targetBusiness.transform.position);
            Debug.Log($"Manager {managerData.managerName} se dirige hacia {targetBusiness.GetBusinessData().businessName}");
        }
    }

    private void Update()
    {
        if (isHired && targetBusiness != null && navMeshAgent != null)
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                Debug.Log($"Manager {managerData.managerName} llegó al negocio {targetBusiness.GetBusinessData().businessName}. Automatizando...");
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

    private IEnumerator FindBusinessPeriodically()
    {
        while (targetBusiness == null)
        {
            yield return new WaitForSeconds(1f);
            targetBusiness = FindNearestBusinessToAutomate();
            if (targetBusiness != null)
            {
                Debug.Log($"Manager {managerData.managerName} encontró un negocio para automatizar: {targetBusiness.GetBusinessData().businessName}");
                MoveToTargetBusiness();
                break;
            }
        }
    }

    public void RemoveFromBusiness()
    {
        if (targetBusiness != null)
        {
            targetBusiness.RemoveManager();
            targetBusiness = null;
            Debug.Log($"Manager {managerData.managerName} removido del negocio");
        }
    }

    public ManagerData GetManagerData()
    {
        return managerData;
    }
}