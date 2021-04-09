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
    [Tooltip("�Ƿ�������Ծ")]
    public static bool canJump;//Ϊ���ü����Ծ��ʹ��GetButton������GetButtonDown����ΪGetButtonDown�п��ܶ���,�����ֲ��밴ס�ո�ѭ������,���˳�run״̬ʱ�ᱻ��ֵ
    public static bool canDoubleJump;




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
    //[Tooltip("����1�ӵ�����CD")]
    //public static float attack1CD;
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
        PlayerDoubleJump,

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
        //������Ӻ����жϻ���Idle��Run״̬�������л�,Ҫ�������Ҽ�ͬʱ���²�������뱼��
        //Ҳ���ǵ������run������һ�𰴻��run�������Ҳ����ٽ�run
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") != 0)//��GetButton����ΪҪѭ�����,����ⰴס�����
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerRun);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)//������Input.GetButton("Horizontal")�ж�,��Ϊ���Ҽ�һ�𰴵����Ҳ��ͣ
        {
            //���ٵ�0����Ϊ�ɿ�֮���Ѿ�ת��Idle״̬,���������ж�
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, 0, ref PlayerInfo.velocityX, PlayerInfo.decelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        #endregion

        #region ��Ծ�߼�
        if (PlayerInfo.canJump && Input.GetButton("Jump"))//ת����Ծ״̬,���˺ܾ����ﻹ�ǵ���GetButton,��Ȼ�����׶�����,�����������canJump������ס��Ծ����
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerJump);
            PlayerInfo.canJump = false;
        }
        if (!Input.GetButton("Jump"))//��ֹ��ҽ��У�����ͣ���Ķ���,������������������ûд�����д���,�Ǿ�����������
        {
            PlayerInfo.canJump = true;
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
        else
        {
            //Debug.Log("IDEL!");
            //�����߼���Idle���жϣ�ת��Idle����
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        //���������ǣ���ס�Ҽ�������°������ô����
        #endregion

        #region ��Ծ�߼�
        if (PlayerInfo.canJump && Input.GetButton("Jump"))//���ﻹ�ǵ���GetButton,��������
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerJump);
            PlayerInfo.canJump = false;
            //Debug.Log("JUMP!");
        }

        if (!Input.GetButton("Jump"))//��ֹ����Ұ�ס�ո񲻷����ˣ�����ͣ�ܣ��Ķ���,������������ɿ��ո�ͼ�ⲻ����
        {
            PlayerInfo.canJump = true;
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

    public override void OnExit()
    {
        if (!Input.GetButton("Jump"))
        {
            PlayerInfo.canJump = true;//ֻ���˳�����ʱû�а���Ծ���Ż������´�����
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
        if (Input.GetButtonDown("Jump") && PlayerInfo.isOnGround)//������Ȼ��Ҫ�����������
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.jumpSpeed);
            //stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerJump);
        }


        if (PlayerInfo.playerRigidBody.velocity.y < 0)//����״̬,��������
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }

        else if (PlayerInfo.playerRigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)//�����������û�г�����Ծ��ʱ��С��
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.lowerJumpMultiplicator - 1) * Time.fixedDeltaTime;
        }

        if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0)//��ظĻ�Idle״̬,�����汻ע�͵��������ж���˼Ӧ��һ��
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }

        //if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0 && !Input.GetButton("Jump"))//��ظĻ�Idle״̬,�����м���жϻ��ö̰��ո��С���߼��޷�ִ��
        //{
        //    stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        //}
        //else if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0 && Input.GetButton("Jump"))//�����סJump����,��ת��run״̬
        //{
        //    stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerRun);
        //}
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

        #region �������߼�
        if (!Input.GetButton("Jump"))
        {
            PlayerInfo.canDoubleJump = true;
        }


        if (Input.GetButton("Jump") && PlayerInfo.canDoubleJump)//���������GetButtonDown��Լ�����GetButton�ᵼ��һ�����տ�ʼֱ�ӵ���������������ҪcanDoubleJump�����Ծ��̧��
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerDoubleJump);
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
        #region ����1�߼�
        if (!Input.GetMouseButton(0))
        {
            stateFSMManager.ChangeState((sbyte)PlayerInfo.PlayerState.PlayerIdle);
        }
        #endregion

        #region �ƶ��߼�
        if (Input.GetAxisRaw("Horizontal") > PlayerInfo.inputDeadZoon.x)
        {
            PlayerInfo.playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.moveSpeed * PlayerInfo.attack1DecelerateMultiplicator * Time.fixedDeltaTime * 60, ref PlayerInfo.velocityX, PlayerInfo.accelerateTime), PlayerInfo.playerRigidBody.velocity.y);
        }
        else if (Input.GetAxisRaw("Horizontal") < PlayerInfo.inputDeadZoon.x * -1)
        {
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

public class PlayerDoubleJump : FSMBase
{
    public PlayerDoubleJump(Animator tmpAnimator, FSMManager tmpFSMManager) : base(tmpAnimator, tmpFSMManager)
    {

    }
    public override void OnEnter()
    {
        PlayerInfo.playerRigidBody.velocity = new Vector2(PlayerInfo.playerRigidBody.velocity.x, PlayerInfo.jumpSpeed);//���������״̬��һ�����ϵ��ٶ�
        stateAnimator.SetInteger("curState", (int)PlayerInfo.PlayerState.PlayerDoubleJump);
    }
    public override void OnUpdate()
    {
        CheckOnGround();
        #region ��Ծ�߼�



        if (PlayerInfo.playerRigidBody.velocity.y < 0)//����״̬,��������
        {
            PlayerInfo.playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (PlayerInfo.fallMultiplicator - 1) * Time.fixedDeltaTime;
        }


        if (PlayerInfo.isOnGround && PlayerInfo.playerRigidBody.velocity.y == 0)//��ظĻ�Idle״̬
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