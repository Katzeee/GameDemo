/*****************************************
 *FileName: CircleBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 21:32:50
 *Description: 圆形轨迹子弹
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : BulletBase
{
    protected override Vector3 nextDirection => (new Vector3(Mathf.Cos(nowAngle + angularSpeed), Mathf.Sin(nowAngle + angularSpeed)) * radius) + initialPosition;

    protected float radius;//半径

    protected float radiusSpeed;//半径扩张速度


    protected float angularSpeed;//角速度


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpInitalPosition">初始位置,不是方向</param>
    /// <param name="tmpRadius">半径</param>
    /// <param name="tmpRadiusSpeed">半径扩张速度</param>
    /// <param name="tmpNowAngle">初始朝向角度,方向在这里定</param>
    /// <param name="tmpAngularSpeed">角速度</param>
    public void Init(Vector3 tmpInitialPosition, float tmpRadius = 1, float tmpRadiusSpeed = 0,float tmpNowAngle = 0, float tmpAngularSpeed = 0.02f)
    {

        transform.position = tmpInitialPosition;
        gameObject.SetActive(true);//在这里设为true,取出来的时候直接设true会导致再上次死亡地点闪现一次

        #region 赋初值
        initialPosition = tmpInitialPosition;
        radius = tmpRadius;
        radiusSpeed = tmpRadiusSpeed;
        nowAngle = tmpNowAngle;
        angularSpeed = tmpAngularSpeed;
        #endregion



    }


    void FixedUpdate()
    {
        transform.position = nextDirection;
        radius += radiusSpeed;
        nowAngle += angularSpeed;
    }
}
