/*****************************************
 *FileName: PlayerControl.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-05 22:08:57
 *Description: ��ɫ����
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoSingleton<PlayerControl>
{
    //��ɫ״̬ö��
    public enum PlayerState
    {
        Normal,//��ͨ   
        SprintFlying,//��̷���
        Sprint//������
    }
    public PlayerState State = PlayerState.Normal;
    



    [Header("�ƶ�")]
    [Tooltip("�ƶ��ٶ�")]
    public float moveSpeed;
    [Tooltip("���ٵ�����ٶȵ�ʱ��")]
    public float accelerateTime;
    [Tooltip("���ٵ����ʱ��")]
    public float decelerateTime;
    [Tooltip("ҡ�������ж�")]
    public Vector2 inputDeadZoon;
    private float velocityX; //�����ý�ȥ��Ϊ������������û��ʵ������
    [Tooltip("�Ƿ��ڿ��ƶ�״̬")]
    public bool canMove;


    [Header("��Ծ")]
    [Tooltip("�����ٶ�")]
    public float jumpSpeed;
    [Tooltip("�½��ٶ�")]
    public float fallSpeed;
    [Tooltip("�Ƿ�������Ծ")]
    public bool canJump;
    [Tooltip("�������")]
    public float fallMultiplicator;
    [Tooltip("С���������")]
    public float lowerJumpMultiplicator;





    [Header("�����ж�")]
    [Tooltip("�ж���ƫ��")]
    public Vector2 checkpointOffset;
    [Tooltip("�ж����С")]
    public Vector2 checkpointSize;
    [Tooltip("��ײ�ж�ͼ��")]
    public LayerMask groundLayerMask;
    [Tooltip("�Ƿ��ڵ�����")]
    public bool isOnGround;


    [Header("����")]
    [Tooltip("�Ƿ����ڹ���2")]
    public bool isAttack2;
    [Tooltip("�Ƿ����ڹ���1")]
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
    /// ������ʼ��
    /// </summary>
    private void Init()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();


        //�ƶ����ݳ�ʼ��
        moveSpeed = 5;
        accelerateTime = 0.09f;
        decelerateTime = 0.09f;
        inputDeadZoon = new Vector2(0.2f, 0.2f);
        canMove = true;

        //��Ծ���ݳ�ʼ��
        jumpSpeed = 5;
        checkpointOffset = new Vector2(0.07f, -0.05f);
        checkpointSize = new Vector2(0.07f, 0.15f);
        fallMultiplicator = 2.5f;
        lowerJumpMultiplicator = 2;


        //ͼ���ɰ��ʼ��
        groundLayerMask = 1<<8;


    }

    /// <summary>
    /// �ƶ�����
    /// </summary>
    private void MoveControl()
    {
        #region �����ƶ�
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
                transform.localScale = new Vector3(-1, 1, 1);//�ı����ҳ���
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

        #region ��Ծ

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

        #region С�����������
        if (playerRigidBody.velocity.y < 0)//����״̬
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplicator - 1) * Time.fixedDeltaTime;
        }
        else if (playerRigidBody.velocity.y > 0 && Input.GetAxis("Jump") != 1)//�����������û�г�����Ծ��ʱ��С��
        {
            playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowerJumpMultiplicator - 1) * Time.fixedDeltaTime;
        }
        #endregion

    }


    /// <summary>
    /// ��������
    /// </summary>
    private void AttackControl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetBool("Attack2", true);
            canJump = false;//���湥��ʱ��������
            isAttack2 = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetBool("Attack2", false);
            isAttack2 = false;
            if (isOnGround)
            {
                canJump = true;//���а���߲���������Ծ״̬
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
    /// �ж��Ƿ��ڵ����ϣ�ÿFixedUpdateִ��
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
    /// ������Scene���ڵ����ж���λ�����С
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + checkpointOffset, checkpointSize);
    }



    #region �����¼�
    /// <summary>
    /// ��Ծ����������ϣ������¼�
    /// </summary>
    private void JumpAnimationOver()
    {
        playerAnimator.SetBool("Jump", false);
    }

    /// <summary>
    /// Attack1����������Ϻ����Ҽ������ƶ�
    /// </summary>
    private void Attack1AnimationOver()
    {
        playerRigidBody.velocity = new Vector2(Mathf.SmoothDamp(playerRigidBody.velocity.x, 0, ref velocityX, decelerateTime), playerRigidBody.velocity.y);
        canMove = false;//��������falseҲ�ǿ��Եģ�������ٵ�����������canMove�޷�����true���������ΪֻҪ���˼�������true
    }
    #endregion


}
