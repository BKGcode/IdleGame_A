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
        }
        else if (_instance != this)
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