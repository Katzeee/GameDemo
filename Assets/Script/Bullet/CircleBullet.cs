/*****************************************
 *FileName: CircleBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 21:32:50
 *Description: 圆形子弹
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : BulletBase
{
    protected override Vector3 nextDirection => (new Vector3(Mathf.Cos(nowAngle + angularSpeed), Mathf.Sin(nowAngle + angularSpeed)) * radius) + initialDirection;

    protected float radius;//半径

    protected float radiusSpeed;//半径扩张速度


    protected float angularSpeed;//角速度


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpInitalDirection">初始位置,不是方向</param>
    /// <param name="tmpRadius"></param>
    /// <param name="tmpRadiusSpeed"></param>
    /// <param name="tmpNowAngle">初始朝向角度,方向在这里定</param>
    /// <param name="tmpAngularSpeed"></param>
    public void Init(Vector3 tmpInitalDirection, float tmpRadius = 1, float tmpRadiusSpeed = 0,float tmpNowAngle = 0, float tmpAngularSpeed = 0.02f)
    {

        initialDirection = tmpInitalDirection;
        radius = tmpRadius;
        radiusSpeed = tmpRadiusSpeed;
        nowAngle = tmpNowAngle;
        angularSpeed = tmpAngularSpeed;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = nextDirection;
        radius += radiusSpeed;
        nowAngle += angularSpeed;
    }
}
