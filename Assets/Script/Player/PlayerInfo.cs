/*****************************************
 *FileName: PlayerStateInfo.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:57:00
 *Description: 管理玩家所有信息
 *包括数据层信息
 *包括状态,而所有状态被PlayerControlFSM中的FSMManager对象管理
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct PlayerInfo
{
    #region 静态参数
    [Header("移动")]
    [Tooltip("移动速度")]
    public static float moveSpeed;
    [Tooltip("加速到最大速度的时间")]
    public static float accelerateTime;
    [Tooltip("减速到零的时间")]
    public static float decelerateTime;
    [Tooltip("摇杆死区判定")]
    public static Vector2 inputDeadZoon;
    public static float velocityX; //传引用进去作为函数参数，并没有实际意义



    [Header("跳跃")]
    [Tooltip("上升速度")]
    public static float jumpSpeed;
    [Tooltip("下降速度")]
    public static float fallSpeed;
    [Tooltip("下落乘数")]
    public static float fallMultiplicator;
    [Tooltip("小跳降落乘数")]
    public static float lowerJumpMultiplicator;
    [Tooltip("是否允许跳跃")]
    public static bool canJump;//为了让检测跳跃能使用GetButton而不是GetButtonDown，因为GetButtonDown有可能丢键,但是又不想按住空格循环起跳,在退出run状态时会被充值
    public static bool canDoubleJump;




    [Header("触地判定")]
    [Tooltip("判定点偏移")]
    public static Vector2 checkpointOffset;
    [Tooltip("判定点大小")]
    public static Vector2 checkpointSize;
    [Tooltip("碰撞判定图层")]
    public static LayerMask groundLayerMask;
    [Tooltip("是否在地面上")]
    public static bool isOnGround;


    [Header("攻击")]
    [Tooltip("攻击2下落速度乘数")]
    public static float attack2FallMultiplicator;
    [Tooltip("攻击1减速乘数")]
    public static float attack1DecelerateMultiplicator;
    //[Tooltip("攻击1子弹发射CD")]
    //public static float attack1CD;
    #endregion


    #region 动态组件或变量
    public static Rigidbody2D playerRigidBody;
    public static Animator playerAnimator;//获得玩家的动画控制器
    public static FSMManager playerStateManager;//管理玩家的状态
    public static Transform playerTransform;//减少调用.transform开销
    #endregion


    #region 状态枚举
    public enum PlayerState
    {
        PlayerIdle,
        PlayerRun,
        PlayerJump,
        PlayerAttack1,
        PlayerAttack2,
        PlayerDoubleJump,

        StateCount//用于记录有多少状态

    }
    #endregion

}


public class PlayerIdel : FSMBase
{
    public PlayerIdel(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {

    }


    public override void OnEnter()
    {
        stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerIdle);

    }


    public override void OnUpdate()
    {
        #region 奔跑逻辑
        //如果不加后面判断会在Idle和Run状态中来回切换,要考虑左右键同时按下并不会进入奔跑
        //也就是单按会进run，两个一起按会从run回来并且不会再进run
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)//用GetButton是因为要循环检测,即检测按住的情况
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerRun);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)//不能用Input.GetButton("Horizontal")判断,因为左右键一起按的情况也得停
        {
            //减速到0，因为松开之后已经转到Idle状态,所以在这判断
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, 0, ref PlayerInfo.velocityX, PlayerInfo.decelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

        #region 跳跃逻辑
        if (PlayerInfo.canJump && Input.GetButton("Jump"))//转到跳跃状态,改了很久这里还是得用GetButton,不然很容易丢操作,但这里加入了canJump进行锁住跳跃动作
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerJump);
            PlayerInfo.canJump = false;
        }
        if (!Input.GetButton("Jump"))//防止玩家进行（跑跳停）的动作,如果做了这个动作但是没写这三行代码,那就跳不起来了
        {
            PlayerInfo.canJump = true;
        }
        #endregion

        #region 攻击1逻辑
        if (Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack1);
        }
        #endregion

        #region 攻击2逻辑
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion
    }

    public override void OnExit()
    {
        if (!Input.GetButton("Jump"))
        {
            PlayerInfo.canJump = true;
        }
    }
}







public class PlayerRun : FSMBase
{
    public PlayerRun(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {

    }
    public override void OnEnter()
    {
        stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerRun);

    }
    public override void OnUpdate()
    {
        #region 移动逻辑
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);//改变左右朝向
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else
        {
            //Debug.Log("IDEL!");
            //减速逻辑在Idle中判断，转回Idle即可
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        //这里问题是：按住右键的情况下按左键怎么处理
        #endregion

        #region 跳跃逻辑
        if (PlayerInfo.canJump && Input.GetButton("Jump"))//这里还是得用GetButton,理由如上
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerJump);
            PlayerInfo.canJump = false;
            //Debug.Log("JUMP!");
        }

        if (!Input.GetButton("Jump"))//防止有玩家按住空格不放做了（跑跳停跑）的动作,如果这样那再松开空格就检测不到了
        {
            PlayerInfo.canJump = true;
        }
        #endregion

        #region 攻击1逻辑
        if (Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack1);
        }
        #endregion

        #region 攻击2逻辑
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion
    }

    public override void OnExit()
    {
        if (!Input.GetButton("Jump"))
        {
            PlayerInfo.canJump = true;//只有退出奔跑时没有按跳跃键才会允许下次起跳
        }
    }
}






public class PlayerJump : FSMBase
{
    public PlayerJump(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {

    }
        
    public override void OnEnter()
    {
        PlayerInfo.canDoubleJump = false;
        CheckOnGround();
        if (PlayerInfo.isOnGround)//跳跃动作在Enter执行
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.jumpSpeed);
            stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerJump);
        }
    }
    public override void OnUpdate()
    {

        #region 跳跃逻辑
        CheckOnGround();
        if (Input.GetButtonDown("Jump") && PlayerInfo.isOnGround)//这里依然需要检测连续起跳
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.jumpSpeed);
            //stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerJump);
        }


        if (PlayerInfo.playerRigidBody.velocity.y < 0)//下落状态,加速下落
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }

        else if (PlayerInfo.playerRigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)//当玩家上升且没有长按跳跃键时，小跳
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.lowerJumpMultiplicator - 1) * Time.fixedDeltaTime;
        }

        if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0)//落地改回Idle状态,和下面被注释掉的两个判断意思应该一样
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }

        //if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0 && !Input.GetButton("Jump"))//落地改回Idle状态,不加中间的判断会让短按空格的小跳逻辑无法执行
        //{
        //    stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        //}
        //else if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0 && Input.GetButton("Jump"))//如果按住Jump键了,则转到run状态
        //{
        //    stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerRun);
        //}
        #endregion

        #region 跳跃移动
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);//改变左右朝向

            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

        #region 二段跳逻辑
        if (!Input.GetButton("Jump"))
        {
            PlayerInfo.canDoubleJump = true;
        }


        if (Input.GetButton("Jump") && PlayerInfo.canDoubleJump)//这里如果用GetButtonDown会吃键，用GetButton会导致一段跳刚开始直接到二段跳，所以需要canDoubleJump检测跳跃键抬起
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerDoubleJump);
        }
        #endregion

        #region 攻击2逻辑
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion

    }

    public override void OnExit()
    {
        if (!Input.GetButton("Jump"))
        {
            PlayerInfo.canJump = true;
        }
    }

    private void CheckOnGround()
    {
        Collider2D tmpColl = Physics2D.OverlapBox((Vector2)PlayerInfo.playerTransform.position + PlayerInfo.checkpointOffset, PlayerInfo.checkpointSize, 0, PlayerInfo.groundLayerMask);
        if (tmpColl == null)
        {
            PlayerInfo.isOnGround = false;
        }
        else
        {
            PlayerInfo.isOnGround = true;
        }
    }

}







public class PlayerAttack1 : FSMBase
{
    public PlayerAttack1(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {
        
    }
    public override void OnEnter()
    {
        stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerAttack1);
    }

    public override void OnUpdate()
    {
        #region 攻击1逻辑
        if (!Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        #endregion

        #region 移动逻辑
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * PlayerInfo.attack1DecelerateMultiplicator * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * PlayerInfo.attack1DecelerateMultiplicator * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (!Input.GetButton("Horizontal"))//减速
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, 0, ref PlayerInfo.velocityX, PlayerInfo.decelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

    }
    public override void OnExit()
    {

    }
}

public class PlayerAttack2 : FSMBase
{
    public PlayerAttack2(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {

    }
    public override void OnEnter()
    {
        stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerAttack2);
    }
    public override void OnUpdate()
    {
        #region 攻击2逻辑
        if (!Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        #endregion

        #region 跳跃下劈逻辑
        if (!PlayerInfo.isOnGround)//下落状态,加速下落
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }
        #endregion
    }
}

public class PlayerDoubleJump : FSMBase
{
    public PlayerDoubleJump(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {

    }
    public override void OnEnter()
    {
        PlayerInfo.playerRigidBody.velocity = new Vector2(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.jumpSpeed);//进入二段跳状态给一个向上的速度
        stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerDoubleJump);
    }
    public override void OnUpdate()
    {
        CheckOnGround();
        #region 跳跃逻辑



        if (PlayerInfo.playerRigidBody.velocity.y < 0)//下落状态,加速下落
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }


        if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0)//落地改回Idle状态
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }

        #endregion

        #region 跳跃移动
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);//改变左右朝向

            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

        #region 攻击2逻辑
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion

    }

    private void CheckOnGround()
    {
        Collider2D tmpColl = Physics2D.OverlapBox((Vector2)PlayerInfo.playerTransform.position + PlayerInfo.checkpointOffset, PlayerInfo.checkpointSize, 0, PlayerInfo.groundLayerMask);
        if (tmpColl == null)
        {
            PlayerInfo.isOnGround = false;
        }
        else
        {
            PlayerInfo.isOnGround = true;
        }
    }
}