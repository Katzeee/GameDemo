/*****************************************
 *FileName: PoolManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:25:43
 *Description: ����ع���  
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pool
{
    public Stack<object> items;//��ջ�洢����
    public int cacheCount;//���������
    public int maxCacheCount; //��󻺴�����  �������ֻ��������٣�ʣ�ಿ�ֿ������ʵ���ʱ���ֶ����ú�������
    //��Ϊunityobject���� ��Ҫ�ֶ����� Ҳ����destroy��������  class����Ļ���ֻҪ������ã�GC���ƾͻ��æ����  �����Ҫһ���ֶ�����ʶһ�¶��������
    public Transform parent;
    public ResourceType resourceType;//��¼����
    public string resourceName;//��¼���֣�������������ҹ�����

    public Pool()
    {
        items = new Stack<object>();//!!!!!ǧ��ǵ�����Ҫnewһ��
    }
}




public class PoolManager:Singleton<PoolManager>
{

    /// <summary>
    /// ���캯��
    /// </summary>
    public PoolManager()
    {
        allPools = new Dictionary<string, Pool>();
    }


    public static Dictionary<string, Pool> allPools;//�������еĳ���,Key��ʾ����



    /// <summary>
    /// �Ե������ӽ��г�ʼ����ע�ᵽallPools��
    /// </summary>
    /// <param name="tmpPoolName">������</param>
    /// <param name="tmpResourceType">��Դ���ͣ����ļ�·��</param>
    /// <param name="tmpResourceName">��Դ����</param>
    /// <param name="tmpParent">��Դ������</param>
    /// <param name="tmpCacheCount">��ʼ����������</param>
    /// <param name="tmpMaxCacheCount">��󻺴�����</param>
    public void PoolInit(string tmpPoolName, ResourceType tmpResourceType, string tmpResourceName, Transform tmpParent = null, int tmpCacheCount = 10, int tmpMaxCacheCount = 10)
    {
        FactoryBase tmpFactory = FactoryBase.GetFactoryType(tmpResourceType);

        if (allPools.ContainsKey(tmpPoolName))//����Ѿ��г�����
        {
            Debug.LogWarning("PoolInit ERROR, Already has the " + tmpPoolName + "'s Pool!");
            return;
        }

        //��ʼ��������ز���
        Pool tmpPool = new Pool
        {
            cacheCount = tmpCacheCount,
            maxCacheCount = tmpMaxCacheCount,
            parent = tmpParent,
            resourceType = tmpResourceType,
            resourceName = tmpResourceName
        };

        //�����ӷ�������
        for (int i = 0; i < tmpCacheCount; i++)
        {
            GameObject tmpObject = tmpFactory.GetResources(tmpResourceName);
            if(tmpObject == null)
            {
                Debug.Log("tmpObject is null");
                return;
            }
            tmpObject.name = tmpResourceName;
            tmpObject.transform.SetParent(tmpParent);
            tmpObject.SetActive(false);
            tmpPool.items.Push(tmpObject);
        }
        allPools.Add(tmpPoolName, tmpPool);
    }

    public GameObject Create(string tmpPoolName)
    {
        if(!allPools.ContainsKey(tmpPoolName))
        {
            Debug.LogWarning("Create ERROR, We don't have " + tmpPoolName + " Pool!!");
            return null;
        }
        if (allPools[tmpPoolName].items.Count != 0)
        {
            GameObject tmpGO = allPools[tmpPoolName].items.Pop() as GameObject;
            //Debug.Log(allPools[tmpPoolName].items.Count.ToString());
            if (tmpGO != null)
            {
                //Debug.Log("��һ��");
                //tmpGO.SetActive(true);//�ó���֮�������Լ�true
                return tmpGO;
            }
        }
        //Debug.Log("��һ��");
        FactoryBase tmpFactory = FactoryBase.GetFactoryType(allPools[tmpPoolName].resourceType);
        GameObject tmpObject = tmpFactory.GetResources(allPools[tmpPoolName].resourceName);
        tmpObject.SetActive(false);//����Ϊ���ɼ�
        tmpObject.transform.SetParent(allPools[tmpPoolName].parent);
        tmpObject.name = allPools[tmpPoolName].resourceName;
        //allPools[tmpPoolName].items.Push(tmpObject);
        return tmpObject;
    }

    public void Recycle(GameObject tmpGO, string tmpPoolName)
    {
        if (!allPools.ContainsKey(tmpPoolName))
        {
            Debug.LogWarning("Recycle ERROR, We don't have " + tmpPoolName + " Pool!!");
            GameObject.Destroy(tmpGO);
            return;
        }
        if (allPools[tmpPoolName].items.Count < allPools[tmpPoolName].maxCacheCount)
        {
            //Debug.Log(allPools[tmpPoolName].items.Count.ToString() + "    " + allPools[tmpPoolName].maxCacheCount.ToString());
            tmpGO.SetActive(false);
            allPools[tmpPoolName].items.Push(tmpGO);
        }
        else
        {
            //Debug.Log("����");
            GameObject.Destroy(tmpGO);
        }
    }
}

public enum ResourceType
{ 
    Bullet
}
