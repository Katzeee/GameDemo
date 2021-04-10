/*****************************************
 *FileName: PlayerControlFSM.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:18:04
 *Description: �ع���PlayerControl���ֵĴ��룬�ĳ�������״̬��������п���
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlFSM : MonoSingleton<PlayerControlFSM>
{

    // Start is called before the first frame update
    void Start()
    {

    }


    public void Init()
    {
        #region ��ȡ���
        PlayerInfo.playerRigidBody = GetComponent<Rigidbody2D>();
        PlayerInfo.playerAnimator = GetComponent<Animator>();
        PlayerInfo.playerTransform = transform;
        #endregion

        #region ������
        GameObject shooter = new GameObject();
        shooter.name = "shooter";
        shooter.transform.parent = PlayerInfo.playerTransform;
        shooter.transform.position = PlayerInfo.playerTransform.position + new Vector3(0.18f, 0.42f, 0);
        shooter.AddComponent<Shooter>();
        #endregion

        #region ������ʼ��
        //�ƶ����ݳ�ʼ��
        PlayerInfo.moveSpeed = 5;
        PlayerInfo.accelerateTime = 0.09f;
        PlayerInfo.decelerateTime = 0.09f;
        PlayerInfo.inputDeadZoon = new Vector2(0.2f, 0.2f);
        //PlayerInfo.canMove = true;

        //��Ծ���ݳ�ʼ��
        PlayerInfo.jumpSpeed = 5;
        PlayerInfo.checkpointOffset = new Vector2(0.07f, -0.05f);
        PlayerInfo.checkpointSize = new Vector2(0.07f, 0.15f);
        PlayerInfo.fallMultiplicator = 2.5f;
        PlayerInfo.lowerJumpMultiplicator = 2;
        PlayerInfo.canJump = true;

        //�������ݳ�ʼ��
        PlayerInfo.attack1DecelerateMultiplicator = 0.5f;
        //PlayerInfo.attack1CD = 0.3f;

        //ͼ���ɰ��ʼ��
        PlayerInfo.groundLayerMask = 1 << 8;
        #endregion


        #region ע��״̬��״̬������
        PlayerInfo.playerFSMManager = new FSMManager((int)PlayerInfo.PlayerState.StateCount);

        PlayerIdel playerIdle = new PlayerIdel(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerIdle);

        PlayerRun playerRun = new PlayerRun(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerRun);

        PlayerJump playerJump = new PlayerJump(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerJump);

        PlayerAttack1 playerAttack1 = new PlayerAttack1(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerAttack1);

        PlayerAttack2 playerAttack2 = new PlayerAttack2(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerAttack2);

        PlayerDoubleJump playerDoubleJump = new PlayerDoubleJump(PlayerInfo.playerAnimator, PlayerInfo.playerFSMManager);
        PlayerInfo.playerFSMManager.AddState(playerDoubleJump);
        #endregion
    }


    public void OnFixedUpdate()
    {
        PlayerInfo.playerFSMManager.OnUpdate();
    }


}
