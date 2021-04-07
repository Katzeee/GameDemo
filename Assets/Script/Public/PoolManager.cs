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
    //public ItemType type;
    public int Count { get; }
    public Transform Parent;

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
    public void PoolInit(string tmpPoolName, resourceType tmpResourceType, string tmpResourceName, Transform tmpParent = null, int tmpCacheCount = 10, int tmpMaxCacheCount = 50)
    {
        FactoryBase tmpFactory = new FactoryBase();//���๤��ָ��
        switch (tmpResourceType)
        {
            case resourceType.Bullet:
            {
                tmpFactory = new BulletFactory();//�����ӵ��Ĺ���
                break;
            }
        }

        if (allPools.ContainsKey(tmpPoolName))//����Ѿ��г�����
        {
            Debug.Log("Already has the " + tmpPoolName + "'s Pool!");
            return;
        }

        //��ʼ��������ز���
        Pool tmpPool = new Pool
        {
            cacheCount = tmpCacheCount,
            maxCacheCount = tmpMaxCacheCount,
            Parent = tmpParent
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
