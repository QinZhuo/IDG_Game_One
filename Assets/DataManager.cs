using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {
    public static DataManager Instance
    {
        get
        {
            return _m_instance;
        }
    }
    protected static DataManager _m_instance;
    public void Awake()
    {
        _m_instance = this;
    }
    public GunManager gunManager;
}
