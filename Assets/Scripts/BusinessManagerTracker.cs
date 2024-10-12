using UnityEngine;
using System.Collections.Generic;

public class BusinessManagerTracker : MonoBehaviour
{
    private static BusinessManagerTracker _instance;
    public static BusinessManagerTracker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BusinessManagerTracker>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("BusinessManagerTracker");
                    _instance = go.AddComponent<BusinessManagerTracker>();
                }
            }
            return _instance;
        }
    }

    [Header("Negocios Contratados")]
    [SerializeField] private List<Business> hiredBusinesses = new List<Business>();

    [Header("Managers Contratados")]
    [SerializeField] private List<Manager> hiredManagers = new List<Manager>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("BusinessManagerTracker instance created and set to DontDestroyOnLoad.");
        }
        else if (_instance != this)
        {
            Debug.LogWarning("Multiple instances of BusinessManagerTracker detected. Destroying the new instance.");
            Destroy(gameObject);
        }
    }

    public void RegisterHiredBusiness(Business business)
    {
        if (business != null && !hiredBusinesses.Contains(business))
        {
            hiredBusinesses.Add(business);
            Debug.Log($"Business registrado: {business.GetBusinessData().businessName}");
        }
    }

    public void UnregisterHiredBusiness(Business business)
    {
        if (business != null && hiredBusinesses.Contains(business))
        {
            hiredBusinesses.Remove(business);
            Debug.Log($"Business desregistrado: {business.GetBusinessData().businessName}");
        }
    }

    public void RegisterHiredManager(Manager manager)
    {
        if (manager != null && !hiredManagers.Contains(manager))
        {
            hiredManagers.Add(manager);
            Debug.Log($"Manager registrado: {manager.GetManagerData().managerName}");
        }
    }

    public void UnregisterHiredManager(Manager manager)
    {
        if (manager != null && hiredManagers.Contains(manager))
        {
            hiredManagers.Remove(manager);
            Debug.Log($"Manager desregistrado: {manager.GetManagerData().managerName}");
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
