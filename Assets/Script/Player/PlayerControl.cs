/*****************************************
 *FileName: PlayerControl.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-05 22:08:57
 *Description: 角色控制
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoSingleton<PlayerControl>
{
    //角色状态枚举
    public enum PlayerState
    {
        Normal,//普通   
        SprintFlying,//冲刺飞行
        Sprint//地面冲刺
    }
    public PlayerState State = PlayerState.Normal;
    



    [Header("移动")]
    [Tooltip("移动速度")]
    public float moveSpeed;
    [Tooltip("加速到最大速度的时间")]
    public float accelerateTime;
    [Tooltip("减速到零的时间")]
    public float decelerateTime;
    [Tooltip("摇杆死区判定")]
    public Vector2 inputDeadZoon;
    private float velocityX; //传引用进去作为函数参数，并没有实际意义
    [Tooltip("是否处于可移动状态")]
    public bool canMove;


    [Header("跳跃")]
    [Tooltip("上升速度")]
    public float jumpSpeed;
    [Tooltip("下降速度")]
    public float fallSpeed;
    [Tooltip("是否允许跳跃")]
    public bool canJump;
    [Tooltip("下落乘数")]
    public float fallMultiplicator;
    [Tooltip("小跳降落乘数")]
    public float lowerJumpMultiplicator;





    [Header("触地判定")]
    [Tooltip("判定点偏移")]
    public Vector2 checkpointOffset;
    [Tooltip("判定点大小")]
    public Vector2 checkpointSize;
    [Tooltip("碰撞判定图层")]
    public LayerMask groundLayerMask;
    [Tooltip("是否在地面上")]
    public bool isOnGround;


    [Header("攻击")]
    [Tooltip("是否正在攻击2")]
    public bool isAttack2;
    [Tooltip("是否正在攻击1")]
    public bool isAttack1;



    Rigidbody2D playerRigidBody;
    Animator playerAnimator;





    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackControl();
    }

    private void FixedUpdate()
    {
        CheckOnGround();

        MoveControl();
    }


    /// <summary>
    /// 参数初始化
    /// </summary>
    private void Init()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();


        //移动数据初始化
        moveSpeed = 5;
        accelerateTime = 0.09f;
        decelerateTime = 0.09f;
        inputDeadZoon = new Vector2(0.2f, 0.2f);
        canMove = true;

        //跳跃数据初始化
        jumpSpeed = 5;
        checkpointOffset = new Vector2(0.07f, -0.05f);
        checkpointSize = new Vector2(0.07f, 0.15f);
        fallMultiplicator = 2.5f;
        lowerJumpMultiplicator = 2;


        //图层蒙版初始化
        groundLayerMask = 1<<8;


    }

    /// <summary>
    /// 移动控制
    /// </summary>
    private void MoveControl()
    {
        #region 左右移动
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") > inputDeadZoon.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                playerAnimator.SetBool("Run", true);
                playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(playerRigidBody.velocity.x, moveSpeed * Time.fixedDeltaTime * 60, ref velocityX, accelerateTime), playerRigidBody.velocity.y);
            }
            else if (Input.GetAxisRaw("Horizontal") < inputDeadZoon.x * -1)
            {
                transform.localScale = new Vector3(-1, 1, 1);//改变左右朝向
                playerAnimator.SetBool("Run", true);
                playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(playerRigidBody.velocity.x, moveSpeed * Time.fixedDeltaTime * 60 * -1, ref velocityX, accelerateTime), playerRigidBody.velocity.y);
            }
            else
            {
                playerAnimator.SetBool("Run", false);
                playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(playerRigidBody.velocity.x, 0, ref velocityX, decelerateTime), playerRigidBody.velocity.y);
            }
        }
        #endregion

        #region 跳跃

        if (Input.GetAxis("Jump") == 1 && canJump && isOnGround)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
            playerAnimator.SetBool("Jump", true);
            canJump = false;
        }
        if (isOnGround && Input.GetAxis("Jump") == 0 && !isAttack2 && !isAttack1)
        {
            canJump = true;
        }

        #endregion

        #region 小跳与加速下落
        if (playerRigidBody.velocity.y < 0)//下落状态
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplicator - 1) * Time.fixedDeltaTime;
        }
        else if (playerRigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)//当玩家上升且没有长按跳跃键时，小跳
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowerJumpMultiplicator - 1) * Time.fixedDeltaTime;
        }
        #endregion

    }


    /// <summary>
    /// 攻击控制
    /// </summary>
    private void AttackControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("Attack2", true);
            canJump = false;//地面攻击时不能起跳
            isAttack2 = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("Attack2", false);
            isAttack2 = false;
            if (isOnGround)
            {
                canJump = true;//空中按左边不能重置跳跃状态
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isAttack1 = true;
            playerAnimator.SetBool("Attack1", true);
            canJump = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isAttack1 = false;
            canJump = true;
            canMove = true;
            playerAnimator.SetBool("Attack1", false);
        }
    }



    /// <summary>
    /// 判断是否在地面上，每FixedUpdate执行
    /// </summary>
    private void CheckOnGround()
    {
        Collider2D tmpColl = Physics2D.OverlapBox((Vector2)transform.position + checkpointOffset, checkpointSize, 0, groundLayerMask);
        if(tmpColl == null)
        {
            isOnGround = false;
            playerAnimator.SetBool("Idle", false);
        }
        else
        {
            isOnGround = true;
            playerAnimator.SetBool("Idle", true);
        }
    }


    /// <summary>
    /// 用于在Scene窗口调节判定点位置与大小
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + checkpointOffset, checkpointSize);
    }



    #region 动画事件
    /// <summary>
    /// 跳跃动画播放完毕，动画事件
    /// </summary>
    private void JumpAnimationOver()
    {
        playerAnimator.SetBool("Jump", false);
    }

    /// <summary>
    /// Attack1动画播放完毕后不松右键不给移动
    /// </summary>
    private void Attack1AnimationOver()
    {
        playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(playerRigidBody.velocity.x, 0, ref velocityX, decelerateTime), playerRigidBody.velocity.y);
        canMove = false;//在这里设false也是可以的，如果快速点击并不会造成canMove无法被设true的情况，因为只要送了键就能设true
    }
    #endregion


}
