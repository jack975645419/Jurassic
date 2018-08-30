using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T:Singleton<T> {
    private static T m_SingleInstance;
    public static T Instance
    {
        get
        {
            if(m_SingleInstance==null)
            {
                m_SingleInstance = GameObject.FindObjectOfType(typeof(T)) as T;
            }
            if(m_SingleInstance==null)
            {
                m_SingleInstance = new GameObject("Singleton" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
            }
            return m_SingleInstance;
        }

    }

    public virtual void Start()
    {

    }
}
