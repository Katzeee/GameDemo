/*****************************************
 *FileName: PlayerControlFSM.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:18:04
 *Description: 重构了PlayerControl部分的代码，改成由有限状态机情况进行控制
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlFSM : MonoSingleton<PlayerControlFSM>
{
    FSMBase playerState;//指向基类的指针
    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init()
    {
        #region 获取组件
        PlayerInfo.playerRigidBody = GetComponent<Rigidbody2D>();
        PlayerInfo.playerAnimator = GetComponent<Animator>();
        PlayerInfo.playerTransform = transform;
        #endregion

        #region 添加组件
        GameObject shooter = new GameObject();
        shooter.name = "shooter";
        shooter.transform.parent = PlayerInfo.playerTransform;
        shooter.transform.position = PlayerInfo.playerTransform.position + new Vector3(0.18f, 0.42f, 0);
        shooter.AddComponent<Shooter>();
        #endregion

        #region 参数初始化
        //移动数据初始化
        PlayerInfo.moveSpeed = 5;
        PlayerInfo.accelerateTime = 0.09f;
        PlayerInfo.decelerateTime = 0.09f;
        PlayerInfo.inputDeadZoon = new Vector2(0.2f, 0.2f);
        //PlayerInfo.canMove = true;

        //跳跃数据初始化
        PlayerInfo.jumpSpeed = 5;
        PlayerInfo.checkpointOffset = new Vector2(0.07f, -0.05f);
        PlayerInfo.checkpointSize = new Vector2(0.07f, 0.15f);
        PlayerInfo.fallMultiplicator = 2.5f;
        PlayerInfo.lowerJumpMultiplicator = 2;
        PlayerInfo.canJump = true;

        //攻击数据初始化
        PlayerInfo.attack1DecelerateMultiplicator = 0.5f;
        //PlayerInfo.attack1CD = 0.3f;

        //图层蒙版初始化
        PlayerInfo.groundLayerMask = 1 << 8;
        #endregion


        #region 注册状态到状态管理器
        PlayerInfo.playerFSMManager = new FSMManager((int)PlayerInfo.PlayerState.StateCount);

        playerState = new PlayerIdel(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerState);

        playerState = new PlayerRun(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerState);

        playerState = new PlayerJump(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerState);

        playerState = new PlayerAttack1(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerState);

        playerState = new PlayerAttack2(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerState);

        playerState = new PlayerDoubleJump(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerState);

        PlayerInfo.playerFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);//进入状态
        #endregion
    }


    public void OnFixedUpdate()
    {
        PlayerInfo.playerFSMManager.OnUpdate();
    }


}
