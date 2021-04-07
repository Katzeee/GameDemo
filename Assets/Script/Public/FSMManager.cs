/*****************************************
 *FileName: FSMManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:21:11
 *Description: 状态机管理器
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager
{
    FSMBase[] allStates;//存储所有状态的数组
    public sbyte stateCount { get; private set; }//存储当前有多少状态
    public sbyte curState { get; private set; }//存储当前所在状态

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="tmpStateCount">传入状态个数</param>
    public FSMManager(int tmpStateCount)
    {
        Init(tmpStateCount);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="tmpStateCount">传入状态个数</param>
    private void Init(int tmpStateCount)
    {
        stateCount = -1;
        allStates = new FSMBase[tmpStateCount];
    }

    /// <summary>
    /// 将状态注册到状态管理器
    /// </summary>
    /// <param name="tmpFSMBase">传入状态类</param>
    public void AddState(FSMBase tmpFSMBase)
    {
        if (stateCount > allStates.Length - 1)
        {
            Debug.Log("allStates's Length is " + allStates.Length.ToString() + ", Out of capacity!");
            return;
        }
        stateCount++;
        allStates[stateCount] = tmpFSMBase;
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="tmpIndex">需要切换到的状态编号</param>
    public void ChangeState(sbyte tmpIndex)
    {
        if (tmpIndex > allStates.Length || curState == tmpIndex)//超出范围或者切换到自己当前状态
        {
            return;
        }
        if (curState != -1)//第一次进入状态
        {
            allStates[curState].OnExit();
        }
        
        curState = tmpIndex;
        allStates[curState].OnEnter();
    }

    /// <summary>
    /// 在Update中调用
    /// </summary>
    public void OnUpdate()
    {
        if (curState != -1)
        {
            allStates[curState].OnUpdate();
        }
    }

}
