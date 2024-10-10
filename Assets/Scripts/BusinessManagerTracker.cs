using UnityEngine;
using System.Collections.Generic;

public class BusinessManagerTracker : MonoBehaviour
{
    public static BusinessManagerTracker Instance { get; private set; }

    [Header("Negocios Contratados")]
    [SerializeField] private List<Business> hiredBusinesses = new List<Business>();

    [Header("Managers Contratados")]
    [SerializeField] private List<Manager> hiredManagers = new List<Manager>();

    private void Awake()
    {
        // Patrón Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterHiredBusiness(Business business)
    {
        if (!hiredBusinesses.Contains(business))
        {
            hiredBusinesses.Add(business);
        }
    }

    public void RegisterHiredManager(Manager manager)
    {
        if (!hiredManagers.Contains(manager))
        {
            hiredManagers.Add(manager);
        }
    }

    public List<Business> GetHiredBusinesses()
    {
        return hiredBusinesses;
    }

    public List<Manager> GetHiredManagers()
    {
        return hiredManagers;
    }
}
