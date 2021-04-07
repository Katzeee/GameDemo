/*****************************************
 *FileName: FSMManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:21:11
 *Description: ״̬��������
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager
{
    FSMBase[] allStates;//�洢����״̬������
    public sbyte stateCount { get; private set; }//�洢��ǰ�ж���״̬
    public sbyte curState { get; private set; }//�洢��ǰ����״̬

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="tmpStateCount">����״̬����</param>
    public FSMManager(int tmpStateCount)
    {
        Init(tmpStateCount);
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    /// <param name="tmpStateCount">����״̬����</param>
    private void Init(int tmpStateCount)
    {
        stateCount = -1;
        allStates = new FSMBase[tmpStateCount];
    }

    /// <summary>
    /// ��״̬ע�ᵽ״̬������
    /// </summary>
    /// <param name="tmpFSMBase">����״̬��</param>
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
    /// �л�״̬
    /// </summary>
    /// <param name="tmpIndex">��Ҫ�л�����״̬���</param>
    public void ChangeState(sbyte tmpIndex)
    {
        if (tmpIndex > allStates.Length || curState == tmpIndex)//������Χ�����л����Լ���ǰ״̬
        {
            return;
        }
        if (curState != -1)//��һ�ν���״̬
        {
            allStates[curState].OnExit();
        }
        
        curState = tmpIndex;
        allStates[curState].OnEnter();
    }

    /// <summary>
    /// ��Update�е���
    /// </summary>
    public void OnUpdate()
    {
        if (curState != -1)
        {
            allStates[curState].OnUpdate();
        }
    }

}
