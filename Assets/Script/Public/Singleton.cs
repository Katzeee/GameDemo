/*****************************************
 *FileName: Singleton.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 18:21:55
 *Description: ·ÇMonoµ¥Àý
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    private readonly static object threadLockObj = new object();
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (threadLockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }

}
