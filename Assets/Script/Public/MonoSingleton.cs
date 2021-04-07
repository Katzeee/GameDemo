/*****************************************
 *FileName: MonoSingleton.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-05 22:05:54
 *Description: Monoµ¥ÀýÄ£°å  
*****************************************/


using UnityEngine;
using System;
using System.Collections.Generic;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    private static readonly object mLock = new object();
    public static T Instance
    {
        get
        {
            lock (mLock)
            {
                if (instance == null)
                {
                    T[] ts = FindObjectsOfType<T>();
                    if (ts.Length > 0)
                    {
                        if (ts.Length == 1)
                        {
                            instance = ts[0];
                            instance.gameObject.name = typeof(T).Name;
                            return Instance;
                        }
                        else
                        {
                            Debug.LogError("class " + typeof(T).Name + " more than one, Destroying all copies");
                            foreach (T t in ts)
                            {
                                Destroy(t.gameObject);
                            }
                        }
                    }

                    string instanceName = typeof(T).Name;
                    GameObject instanceObj = GameObject.Find(instanceName);
                    if (instanceObj == null)
                    {
                        instanceObj = new GameObject(instanceName);
                    }
                    instance = instanceObj.AddComponent<T>();
                    DontDestroyOnLoad(instanceObj);
                    Debug.Log("Add New MonoSingleton:" + instanceName);
                }
                return instance;
            }
        }
    }
    protected virtual void OnDestroy()
    {
        instance = null;
    }
}

