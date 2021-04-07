/*****************************************
 *FileName: FSMBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:18:56
 *Description: ����״̬�����Ƶ�״̬����
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBase
{
    public Animator stateAnimator;//���Ŵ�״̬�Ķ�����
    public FSMManager stateFSMManager;//�����״̬�Ĺ���Ա
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
