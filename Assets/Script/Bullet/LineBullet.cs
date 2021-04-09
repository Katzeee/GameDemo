/*****************************************
 *FileName: LineBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 14:15:57
 *Description: 直线子弹
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : BulletBase
{
    [Tooltip("通过方向算出的角度")]
    protected float angle;

    protected override Vector3 nextDirection => transform.position + new Vector3(Mathf.Cos(nowAngle) * nowSpeed, Mathf.Sin(nowAngle) * nowSpeed);



    void FixedUpdate()
    {
        transform.position = nextDirection;//每帧调用nextDirection更新位置
        nowSpeed = nowSpeed > 0 ? nowSpeed + acceleration : nowSpeed - acceleration;//加速度
    }


    /// <summary>
    /// 参数初始化
    /// </summary>
    /// <param name="tmpInitialDirection">初始发射位置</param>
    /// <param name="tmpDestinationDirection">目标位置</param>
    /// <param name="tmpInitalSpeed">初速度</param>
    /// <param name="tmpAcceleration">加速度</param>
    public void Init(Vector3 tmpInitialDirection, Vector3 tmpDestinationDirection, float tmpInitalSpeed = 0.5f, float tmpAcceleration = 0)
    {
        #region 初始化赋值
        initialDirection = tmpInitialDirection;
        destinationDirection = tmpDestinationDirection;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        destinationDirection = destinationDirection - initialDirection;
        angle = Mathf.Atan(destinationDirection.y / destinationDirection.x);//计算朝向角度
        nowSpeed = initialSpeed;//设定速度
        nowAngle = angle;//设定角度
        #endregion

        if (destinationDirection.x < 0)//若往左边发射
        {
            nowSpeed = -nowSpeed;
        }

        transform.position = initialDirection;//初始位置
    }

    /// <summary>
    /// 重载Init用作Boss发射子弹
    /// </summary>
    /// <param name="tmpInitialDirection"></param>
    /// <param name="tmpAngle">反射角度</param>
    /// <param name="tmpInitalSpeed"></param>
    /// <param name="tmpAcceleration"></param>
    public void Init(Vector3 tmpInitialDirection, float tmpAngle, float tmpInitalSpeed = 0.5f, float tmpAcceleration = 0)
    {
        #region 初始化赋值
        initialDirection = tmpInitialDirection;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        nowSpeed = initialSpeed;//设定速度
        nowAngle = tmpAngle;//设定角度
        #endregion

        if (destinationDirection.x < 0)//若往左边发射
        {
            nowSpeed = -nowSpeed;
        }

        transform.position = initialDirection;//初始位置
    }



}
