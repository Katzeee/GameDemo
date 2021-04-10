/*****************************************
 *FileName: Boss2.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 20:28:34
 *Description: Boss2控制器
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : BossBase
{


    void Start()
    {
        Init();
    }

    void Init()
    {
        #region 注册状态
        bossFSMManager = new FSMManager((int)BossState.Count);
        
        bossStateBase = new BossIdle(bossFSMManager);
        bossFSMManager.AddState(bossStateBase);

        bossStateBase = new BossAttack1(bossFSMManager);
        bossFSMManager.AddState(bossStateBase);
        #endregion




        //GameObject tmpGO = new GameObject();
        //tmpGO.name = "SpellCast1";
        //tmpGO.transform.parent = transform;
        //tmpGO.AddComponent<SpellCast1>();
    }

    // Update is called once per frame
    void Update()
    {
        bossFSMManager.OnUpdate();


    }



}


public enum BossState
{
    BossIdle,
    BossAttack1,
    Count
}


public class BossIdle : FSMBase
{

    float waitTimeLowerBound;//Idle状态等待时间下界
    float waitTimeUpperBound;//Idle状态等待时间上界
    float timer;//计时时间

    public BossIdle(FSMManager tmpFSMManager, Animator tmpAnimator = null) : base(tmpAnimator, tmpFSMManager)
    {

    }

    public override void OnEnter()
    {
        waitTimeLowerBound = 1;
        waitTimeUpperBound = 5;
        Debug.Log("Idle Mode!");
        timer = Random.Range(waitTimeLowerBound, waitTimeUpperBound);
    }
    public override void OnUpdate()
    {
        GameManager.Instance.globleTimer.AddTimeTask(a => { stateFSMManager.ChangeState((sbyte)BossState.BossAttack1); }, timer, PETimeUnit.Second);
    }

}

public class BossAttack1 : FSMBase
{

    float waitTimeLowerBound;//Idle状态等待时间下界
    float waitTimeUpperBound;//Idle状态等待时间上界
    float timer;//计时器
    public BossAttack1(FSMManager tmpFSMManager, Animator tmpAnimator = null) : base(tmpAnimator, tmpFSMManager)
    {

    }

    public override void OnEnter()
    {
        waitTimeLowerBound = 1;
        waitTimeUpperBound = 5;
        //base.OnEnter();
        Debug.Log("Attack Mode!");
        timer = Random.Range(waitTimeLowerBound, waitTimeUpperBound);
        GameManager.Instance.globleTimer.AddTimeTask(a => { stateFSMManager.ChangeState((sbyte)BossState.BossIdle); }, timer, PETimeUnit.Second);
    }

    public override void OnUpdate()
    {
        
    }
}