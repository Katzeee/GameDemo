/*****************************************
 *FileName: BulletBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:14:19
 *Description: �ӵ�����  
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [Header("����")]
    [Tooltip("��ʼ���ֵķ���")]
    protected Vector3 initialPosition;
    [Tooltip("���ճ���ķ���")]
    protected Vector3 destinationPosition;
    protected abstract Vector3 nextDirection { get; }
    protected float nowAngle;//���ڵĽǶ�,ͬʱ�������ڿ��ƷǶԳ��ӵ�����
    //protected float nextAngle;
    protected bool isRotate;//�Ƿ���Ҫ������ת


    [Header("�ٶ�")]
    [Tooltip("��ʼ�ٶ�")]
    protected float initialSpeed;
    [Tooltip("���ٶ�")]
    protected float acceleration;
    protected float nowSpeed;//��ǰ�ٶ�


    protected void OnBecameInvisible()
    {
        //Debug.Log("һ��");
        transform.localScale = Vector3.one;//��ת����
        transform.position = Vector3.zero;
        PoolManager.Instance.Recycle(gameObject, gameObject.name);
        Destroy(this);//�Żس���֮�����ٱ��ű�����Ȼ�´�ȡ���ټӽű���������
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        //Debug.Log(tag);

        if (collision.tag != "Player" && tag == "Bullet")
        {
            gameObject.SetActive(false);//���ﲻ�����³�����ķ�������Ϊ�ᵼ��OnBecameInvisibleҲ������һ��
        }
        if (collision.tag != "Enemy" && tag == "EnemyBullet")
        {
            gameObject.SetActive(false);
        }
        //Debug.Log("ײ��" + collision.name);
    }
}

public enum BulletType
{
    LineBullet,
    CircleBullet
}
