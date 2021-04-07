/*****************************************
 *FileName: FSMBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:18:56
 *Description: 有限状态机控制的状态基类
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBase
{
    public Animator stateAnimator;//播放此状态的动画机
    public FSMManager stateFSMManager;//管理此状态的管理员
    public FSMBase(Animator tmpAnimator, FSMManager tmpFSMManager)
    {
        stateAnimator = tmpAnimator;
        stateFSMManager = tmpFSMManager;
    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
}
