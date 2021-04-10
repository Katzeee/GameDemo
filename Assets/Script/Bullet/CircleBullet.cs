/*****************************************
 *FileName: CircleBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 21:32:50
 *Description: Բ���ӵ�
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBullet : BulletBase
{
    protected override Vector3 nextDirection => (new Vector3(Mathf.Cos(nowAngle + angularSpeed), Mathf.Sin(nowAngle + angularSpeed)) * radius) + initialDirection;

    protected float radius;//�뾶

    protected float radiusSpeed;//�뾶�����ٶ�


    protected float angularSpeed;//���ٶ�


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpInitalDirection">��ʼλ��,���Ƿ���</param>
    /// <param name="tmpRadius"></param>
    /// <param name="tmpRadiusSpeed"></param>
    /// <param name="tmpNowAngle">��ʼ����Ƕ�,���������ﶨ</param>
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
