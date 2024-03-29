/*****************************************
 *FileName: BulletBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:14:19
 *Description: 子弹基类  
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [Header("方向")]
    [Tooltip("初始出现的方向")]
    protected Vector3 initialPosition;
    [Tooltip("最终朝向的方向")]
    protected Vector3 destinationPosition;
    protected abstract Vector3 nextDirection { get; }
    protected float nowAngle;//现在的角度,同时可以用于控制非对称子弹朝向
    //protected float nextAngle;
    protected bool isRotate;//是否需要控制旋转


    [Header("速度")]
    [Tooltip("初始速度")]
    protected float initialSpeed;
    [Tooltip("加速度")]
    protected float acceleration;
    protected float nowSpeed;//当前速度


    protected void OnBecameInvisible()
    {
        //Debug.Log("一次");
        transform.localScale = Vector3.one;//翻转回来
        transform.position = Vector3.zero;
        PoolManager.Instance.Recycle(gameObject, gameObject.name);
        Destroy(this);//放回池子之后销毁本脚本，不然下次取出再加脚本会有问题
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        //Debug.Log(tag);

        if (collision.tag != "Player" && tag == "Bullet")
        {
            gameObject.SetActive(false);//这里不能重新抄上面的方法，因为会导致OnBecameInvisible也被调用一次
        }
        if (collision.tag != "Enemy" && tag == "EnemyBullet")
        {
            gameObject.SetActive(false);
        }
        //Debug.Log("撞到" + collision.name);
    }
}

public enum BulletType
{
    LineBullet,
    CircleBullet
}
