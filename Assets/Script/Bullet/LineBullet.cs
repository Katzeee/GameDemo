/*****************************************
 *FileName: LineBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 14:15:57
 *Description: 直线轨迹子弹
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : BulletBase
{
    [Tooltip("通过方向算出的角度")]
    protected float angle;

    protected override Vector3 nextDirection => transform.position + new Vector3(Mathf.Cos(nowAngle) * nowSpeed, Mathf.Sin(nowAngle) * nowSpeed);

    Vector3 temp;

    void FixedUpdate()
    {
        transform.position = nextDirection;//每帧调用nextDirection更新位置



        if (acceleration != 0)
        {
            nowSpeed = nowSpeed > 0 ? nowSpeed + acceleration : nowSpeed - acceleration;//加速度
        }

        if (isRotate)
        {
//            Debug.Log((nowAngle * Mathf.Rad2Deg).ToString());

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * nowAngle - 90));
        }
    }


    /// <summary>
    /// 参数初始化
    /// </summary>
    /// <param name="tmpInitialPosition">初始发射位置</param>
    /// <param name="tmpDestinationPosition">目标位置</param>
    /// <param name="tmpInitalSpeed">初速度</param>
    /// <param name="tmpAcceleration">加速度</param>
    public void Init(Vector3 tmpInitialPosition, Vector3 tmpDestinationPosition, float tmpInitalSpeed = 0.5f, bool tmpIsRotate = false, float tmpAcceleration = 0)
    {

        transform.position = tmpInitialPosition;
        gameObject.SetActive(true);//在这里设为true,取出来的时候直接设true会导致再上次死亡地点闪现一次

        #region 初始化赋值
        initialPosition = tmpInitialPosition;
        destinationPosition = tmpDestinationPosition;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        destinationPosition = destinationPosition - initialPosition;//计算朝向方向向量
        angle = Mathf.Atan(destinationPosition.y / destinationPosition.x);//通过向量计算朝向角度
        nowSpeed = initialSpeed;//设定速度
        nowAngle = angle;//设定角度
        isRotate = tmpIsRotate;
        #endregion

        #region 如果往左边发射
        //因为Arctan的值域只有-pi/2到pi/2,所以还需要x轴方向知道子弹再往哪边飞
        if (destinationPosition.x < 0)//若往左边发射,考虑速度向量反向以及反转贴图问题,翻转回来的问题可以放在回收池子的地方做
        {
            nowSpeed = -nowSpeed;
            transform.localScale = new Vector3(1, -1, 1);
        }
        #endregion

    }

    /// <summary>
    /// 重载Init用作Boss发射子弹
    /// </summary>
    /// <param name="tmpInitialPosition"></param>
    /// <param name="tmpAngle">反射角度</param>
    /// <param name="tmpInitalSpeed"></param>
    /// <param name="tmpAcceleration"></param>
    public void Init(Vector3 tmpInitialPosition, float tmpAngle, float tmpInitalSpeed = 0.5f, float tmpAcceleration = 0)
    {
        #region 初始化赋值
        initialPosition = tmpInitialPosition;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        nowSpeed = initialSpeed;//设定速度
        nowAngle = tmpAngle;//设定角度
        #endregion

        if (destinationPosition.x < 0)//若往左边发射
        {
            nowSpeed = -nowSpeed;
        }

        transform.position = initialPosition;//初始位置
    }



}
