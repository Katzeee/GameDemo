/*****************************************
 *FileName: FactoryBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:33:24
 *Description: 工厂基类，所有工厂都继承于此类  
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBase
{

    protected string loadPath;
    public static Dictionary<string, GameObject> allPrefabs;

    public FactoryBase()
    {
        loadPath = "Prefabs/";//默认必须放在Resources文件夹下
        allPrefabs = new Dictionary<string, GameObject>();
    }

    //造GO
    public GameObject GetResources(string itemName)
    {
        GameObject itemPrefab;
        string itemLoadPath = loadPath + itemName;
        if(!allPrefabs.ContainsKey(itemName))
        {
            itemPrefab = Resources.Load<GameObject>(itemLoadPath);
            allPrefabs.Add(itemName, itemPrefab);
            //判断是否加载成功
            if (itemPrefab == null)
            {
                Debug.Log("Can't get " + itemName + " whose path is " + itemLoadPath);
                return null;
            }
        }
        return GameObject.Instantiate(allPrefabs[itemName]);

    }

}

