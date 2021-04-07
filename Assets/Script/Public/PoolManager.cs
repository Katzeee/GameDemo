/*****************************************
 *FileName: PoolManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:25:43
 *Description: 对象池管理  
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool
{
    public Stack<object> items;//用栈存储物体
    public int cacheCount;//缓存的容量
    public int maxCacheCount; //最大缓存容量  超出部分会立即销毁，剩余部分可以在适当的时机手动调用函数销毁
    //因为unityobject类型 需要手动销毁 也就是destroy才能销毁  class对象的话，只要清空引用，GC机制就会帮忙销毁  因此需要一个字段来标识一下对象的类型
    //public ItemType type;
    public int Count { get; }
    public Transform Parent;

    public Pool()
    {
        items = new Stack<object>();//!!!!!千万记得这里要new一个
    }
}




public class PoolManager:Singleton<PoolManager>
{

    /// <summary>
    /// 构造函数
    /// </summary>
    public PoolManager()
    {
        allPools = new Dictionary<string, Pool>();
    }


    public static Dictionary<string, Pool> allPools;//管理所有的池子,Key表示池名



    /// <summary>
    /// 对单个池子进行初始化并注册到allPools中
    /// </summary>
    /// <param name="tmpPoolName">池子名</param>
    /// <param name="tmpResourceType">资源类型，即文件路径</param>
    /// <param name="tmpResourceName">资源名称</param>
    /// <param name="tmpParent">资源父物体</param>
    /// <param name="tmpCacheCount">初始化缓存数量</param>
    /// <param name="tmpMaxCacheCount">最大缓存数量</param>
    public void PoolInit(string tmpPoolName, resourceType tmpResourceType, string tmpResourceName, Transform tmpParent = null, int tmpCacheCount = 10, int tmpMaxCacheCount = 50)
    {
        FactoryBase tmpFactory = new FactoryBase();//基类工厂指针
        switch (tmpResourceType)
        {
            case resourceType.Bullet:
            {
                tmpFactory = new BulletFactory();//生产子弹的工厂
                break;
            }
        }

        if (allPools.ContainsKey(tmpPoolName))//如果已经有池子了
        {
            Debug.Log("Already has the " + tmpPoolName + "'s Pool!");
            return;
        }

        //初始化池子相关参数
        Pool tmpPool = new Pool
        {
            cacheCount = tmpCacheCount,
            maxCacheCount = tmpMaxCacheCount,
            Parent = tmpParent
        };


        //给池子放入物体
        for (int i = 0; i < tmpCacheCount; i++)
        {
            GameObject tmpObject = tmpFactory.GetResources(tmpResourceName);
            if(tmpObject == null)
            {
                Debug.Log("tmpObject is null");
                return;
            }
            tmpObject.transform.SetParent(tmpParent);
            tmpObject.SetActive(false);
            tmpPool.items.Push(tmpObject);
        }
        allPools.Add(tmpPoolName, tmpPool);
    }
}

public enum resourceType
{ 
    Bullet
}
