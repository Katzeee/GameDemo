/*****************************************
 *FileName: PlayerStateInfo.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-07 13:57:00
 *Description: �������������Ϣ
 *�������ݲ���Ϣ
 *����״̬,������״̬��PlayerControlFSM�е�FSMManager�������
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct PlayerInfo
{
    #region ��̬����
    [Header("�ƶ�")]
    [Tooltip("�ƶ��ٶ�")]
    public static float moveSpeed;
    [Tooltip("���ٵ�����ٶȵ�ʱ��")]
    public static float accelerateTime;
    [Tooltip("���ٵ����ʱ��")]
    public static float decelerateTime;
    [Tooltip("ҡ�������ж�")]
    public static Vector2 inputDeadZoon;
    public static float velocityX; //�����ý�ȥ��Ϊ������������û��ʵ������



    [Header("��Ծ")]
    [Tooltip("�����ٶ�")]
    public static float jumpSpeed;
    [Tooltip("�½��ٶ�")]
    public static float fallSpeed;
    [Tooltip("�������")]
    public static float fallMultiplicator;
    [Tooltip("С���������")]
    public static float lowerJumpMultiplicator;





    [Header("�����ж�")]
    [Tooltip("�ж���ƫ��")]
    public static Vector2 checkpointOffset;
    [Tooltip("�ж����С")]
    public static Vector2 checkpointSize;
    [Tooltip("��ײ�ж�ͼ��")]
    public static LayerMask groundLayerMask;
    [Tooltip("�Ƿ��ڵ�����")]
    public static bool isOnGround;


    [Header("����")]
    [Tooltip("����2�����ٶȳ���")]
    public static float attack2FallMultiplicator;
    [Tooltip("����1���ٳ���")]
    public static float attack1DecelerateMultiplicator;
    #endregion


    #region ��̬��������
    public static Rigidbody2D playerRigidBody;
    public static Animator playerAnimator;//�����ҵĶ���������
    public static FSMManager playerStateManager;//������ҵ�״̬
    public static Transform playerTransform;//���ٵ���.transform����
    #endregion


    #region ״̬ö��
    public enum PlayerState
    {
        PlayerIdle,
        PlayerRun,
        PlayerJump,
        PlayerAttack1,
        PlayerAttack2,

        StateCount//���ڼ�¼�ж���״̬

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
        #region �����߼�
        if (Input.GetButton("Horizontal"))//��GetButton����ΪҪѭ�����,����ⰴס�����
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerRun);
        }
        if (!Input.GetButton("Horizontal"))
        {
            //���ٵ�0����Ϊ�ɿ�֮���Ѿ�ת��Idle״̬,���������ж�
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, 0, ref PlayerInfo.velocityX, PlayerInfo.decelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

        #region ��Ծ�߼�
        if (Input.GetButtonDown("Jump"))//ת����Ծ״̬,���ﲻ��ѭ����⣬������GetButtonDown
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerJump);
        }

        #endregion

        #region ����1�߼�
        if (Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack1);
        }
        #endregion

        #region ����2�߼�
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion
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
        #region �ƶ��߼�
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);//�ı����ҳ���

            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if(!Input.GetButton("Horizontal"))//��������жϻ���Idle��Run״̬�������л�
        {
            //�����߼���Idle���жϣ�ת��Idle����
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        //���������ǣ���ס�Ҽ�������°������ô����
        #endregion

        #region ��Ծ�߼�
        if (Input.GetButtonDown("Jump"))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerJump);
        }
        #endregion

        #region ����1�߼�
        if (Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack1);
        }
        #endregion

        #region ����2�߼�
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion
    }
}






public class PlayerJump : FSMBase
{
    public PlayerJump(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {
        
    }
    float timer;
    public override void OnEnter()
    {
        CheckOnGround();
        if (PlayerInfo.isOnGround)//��Ծ������Enterִ��
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.jumpSpeed);
            stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerJump);
        }
    }
    public override void OnUpdate()
    {

        #region ��Ծ�߼�
        CheckOnGround();

        if (PlayerInfo.playerRigidBody.velocity.y < 0)//����״̬,��������
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }

        else if (PlayerInfo.playerRigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)//�����������û�г�����Ծ��ʱ��С��
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.lowerJumpMultiplicator - 1) * Time.fixedDeltaTime;
        }

        if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0)//��ظĻ�Idle״̬,���Ӻ�����жϻ��ö̰��ո��С���߼��޷�ִ��
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        #endregion

        #region ��Ծ�ƶ�
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);//�ı����ҳ���

            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

        #region ����2�߼�
        if (Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerAttack2);
        }
        #endregion

    }

    public override void OnExit()
    {

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
        #region ����1�߼�
        if (!Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        #endregion

        #region �ƶ��߼�
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            if (!Input.GetMouseButton(0))//������ڹ�����ת��
            {
                PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
            }
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * PlayerInfo.attack1DecelerateMultiplicator * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
            if (!Input.GetMouseButton(0))//������ڹ�����ת��
            {
                PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);//�ı����ҳ���
            }

            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * PlayerInfo.attack1DecelerateMultiplicator * Time.fixedDeltaTime * 60 * -1, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (!Input.GetButton("Horizontal"))//����
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
        #region ����2�߼�
        if (!Input.GetMouseButton(1))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        #endregion

        #region ��Ծ�����߼�
        if (!PlayerInfo.isOnGround)//����״̬,��������
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }
        #endregion
    }
}

