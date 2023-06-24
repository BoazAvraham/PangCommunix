using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonobehavior<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;
    private static readonly object _instanceLock = new object();
    private static bool _quitting = false;

    virtual protected void OnAwake() { }

    public static T Instance {
        get {
            lock(_instanceLock){
                if(_instance==null && !_quitting){

                    _instance = FindObjectOfType<T>();
                    if(_instance==null){
                        GameObject go = new GameObject(typeof(T).ToString());
                        _instance = go.AddComponent<T>();

                        // DontDestroyOnLoad(_instance.gameObject);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = gameObject.GetComponent<T>();
            OnAwake();
        }
        else if (_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            Debug.LogWarning($"Instance of {GetType().FullName} already exists, removing {ToString()}");
        }
    }

    protected virtual void OnApplicationQuit() 
    {
        _quitting = true;
    }

}
