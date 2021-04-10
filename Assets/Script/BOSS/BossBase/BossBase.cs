/*****************************************
 *FileName: BossBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 20:28:19
 *Description: Boss����
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoSingleton<BossBase>
{

    public FSMManager bossFSMManager { get; protected set; }//ÿ��boss��״̬������
    protected FSMBase bossStateBase;//ָ������ָ��
    public BossInfoBase bossInfo { get; protected set; }//����Boss��Ϣ


    protected virtual void Init()
    {
        //Debug.Log("�һ�ִ����");
        bossInfo = new BossInfoBase(transform);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
