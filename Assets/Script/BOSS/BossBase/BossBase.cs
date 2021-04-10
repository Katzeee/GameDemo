/*****************************************
 *FileName: BossBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 20:28:19
 *Description: Boss基类
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoSingleton<BossBase>
{

    public FSMManager bossFSMManager { get; protected set; }//每个boss的状态管理器
    protected FSMBase bossStateBase;//指向基类的指针
    public BossInfoBase bossInfo { get; protected set; }//管理Boss信息


    protected virtual void Init()
    {
        //Debug.Log("我会执行吗？");
        bossInfo = new BossInfoBase(transform);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
