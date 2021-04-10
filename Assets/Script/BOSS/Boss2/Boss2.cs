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

    protected override void Init()
    {
        base.Init();
        #region 注册状态
        //注册顺序要与enum一致
        bossFSMManager = new FSMManager((int)BossState.Count);
        
        bossStateBase = new BossIdle(bossFSMManager);
        bossFSMManager.AddState(bossStateBase);

        bossStateBase = new BossAttack1(bossFSMManager);
        bossFSMManager.AddState(bossStateBase);

        bossStateBase = new BossMoveRight(bossFSMManager);
        bossFSMManager.AddState(bossStateBase);

        bossStateBase = new BossMoveLeft(bossFSMManager);
        bossFSMManager.AddState(bossStateBase);

        bossFSMManager.ChangeState((sbyte)BossState.BossIdle);//进入状态
        #endregion



    }

    // Update is called once per frame
    void Update()
    {
        


    }



}


public enum BossState
{
    BossIdle,
    BossAttack1,
    BossMoveRight,
    BossMoveLeft,

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
        GameManager.Instance.globleTimer.AddTimeTask(a => { stateFSMManager.ChangeState((sbyte)BossState.BossMoveRight); }, timer, PETimeUnit.Second);
    }
    public override void OnUpdate()
    {

    }

}

public class BossAttack1 : FSMBase
{

    float waitTimeLowerBound;//Idle状态等待时间下界
    float waitTimeUpperBound;//Idle状态等待时间上界
    float timer;//计时器
    SpellCardBase tmpSpellCard;
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
        GameManager.Instance.globleTimer.AddTimeTask(a => { stateFSMManager.ChangeState((sbyte)BossState.BossMoveLeft); }, timer, PETimeUnit.Second);



        #region 挂载发射器
        GameObject tmpGO = new GameObject();
        tmpGO.name = "SpellCast1";
        tmpGO.transform.parent = Boss2.Instance.bossInfo.bossTransform;
        tmpSpellCard = tmpGO.AddComponent<SpellCard1>();
        #endregion
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        GameObject.Destroy(tmpSpellCard);
    }
}


public class BossMoveRight : FSMBase
{

    float waitTimeLowerBound;//Idle状态等待时间下界
    float waitTimeUpperBound;//Idle状态等待时间上界
    float timer;//计时器
    public BossMoveRight(FSMManager tmpFSMManager, Animator tmpAnimator = null) : base(tmpAnimator, tmpFSMManager)
    {

    }

    public override void OnEnter()
    {
        waitTimeLowerBound = 1;
        waitTimeUpperBound = 5;
        //base.OnEnter();
        Debug.Log("Move Right!");
        timer = Random.Range(waitTimeLowerBound, waitTimeUpperBound);
        Boss2.Instance.bossInfo.bossRigidBody.velocity = new Vector3(1, 0, 0);
        GameManager.Instance.globleTimer.AddTimeTask(a => { stateFSMManager.ChangeState((sbyte)BossState.BossAttack1); }, timer, PETimeUnit.Second);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        Boss2.Instance.bossInfo.bossRigidBody.velocity = new Vector3(0, 0, 0);
    }
}


public class BossMoveLeft : FSMBase
{

    float waitTimeLowerBound;//Idle状态等待时间下界
    float waitTimeUpperBound;//Idle状态等待时间上界
    float timer;//计时器
    public BossMoveLeft(FSMManager tmpFSMManager, Animator tmpAnimator = null) : base(tmpAnimator, tmpFSMManager)
    {

    }

    public override void OnEnter()
    {
        waitTimeLowerBound = 1;
        waitTimeUpperBound = 5;
        //base.OnEnter();
        Debug.Log("Move Left!");
        timer = Random.Range(waitTimeLowerBound, waitTimeUpperBound);
        Boss2.Instance.bossInfo.bossRigidBody.velocity = new Vector3(-1, 0, 0);
        GameManager.Instance.globleTimer.AddTimeTask(a => { stateFSMManager.ChangeState((sbyte)BossState.BossIdle); }, timer, PETimeUnit.Second);
    }

    public override void OnUpdate()
    {

    }
    public override void OnExit()
    {
        Boss2.Instance.bossInfo.bossRigidBody.velocity = new Vector3(0, 0, 0);
        //base.OnExit();
    }
}