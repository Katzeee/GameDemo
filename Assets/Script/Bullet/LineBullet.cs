/*****************************************
 *FileName: LineBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 14:15:57
 *Description: ֱ���ӵ�
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : BulletBase
{
    [Tooltip("ͨ����������ĽǶ�")]
    protected float angle;

    protected override Vector3 nextDirection => transform.position + new Vector3(Mathf.Cos(nowAngle) * nowSpeed, Mathf.Sin(nowAngle) * nowSpeed);



    void FixedUpdate()
    {
        transform.position = nextDirection;//ÿ֡����nextDirection����λ��
        nowSpeed = nowSpeed > 0 ? nowSpeed + acceleration : nowSpeed - acceleration;//���ٶ�
    }


    /// <summary>
    /// ������ʼ��
    /// </summary>
    /// <param name="tmpInitialDirection">��ʼ����λ��</param>
    /// <param name="tmpDestinationDirection">Ŀ��λ��</param>
    /// <param name="tmpInitalSpeed">���ٶ�</param>
    /// <param name="tmpAcceleration">���ٶ�</param>
    public void Init(Vector3 tmpInitialDirection, Vector3 tmpDestinationDirection, float tmpInitalSpeed = 0.5f, float tmpAcceleration = 0)
    {
        #region ��ʼ����ֵ
        initialDirection = tmpInitialDirection;
        destinationDirection = tmpDestinationDirection;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        destinationDirection = destinationDirection - initialDirection;
        angle = Mathf.Atan(destinationDirection.y / destinationDirection.x);//���㳯��Ƕ�
        nowSpeed = initialSpeed;//�趨�ٶ�
        nowAngle = angle;//�趨�Ƕ�
        #endregion

        if (destinationDirection.x < 0)//������߷���
        {
            nowSpeed = -nowSpeed;
        }

        transform.position = initialDirection;//��ʼλ��
    }

    /// <summary>
    /// ����Init����Boss�����ӵ�
    /// </summary>
    /// <param name="tmpInitialDirection"></param>
    /// <param name="tmpAngle">����Ƕ�</param>
    /// <param name="tmpInitalSpeed"></param>
    /// <param name="tmpAcceleration"></param>
    public void Init(Vector3 tmpInitialDirection, float tmpAngle, float tmpInitalSpeed = 0.5f, float tmpAcceleration = 0)
    {
        #region ��ʼ����ֵ
        initialDirection = tmpInitialDirection;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        nowSpeed = initialSpeed;//�趨�ٶ�
        nowAngle = tmpAngle;//�趨�Ƕ�
        #endregion

        if (destinationDirection.x < 0)//������߷���
        {
            nowSpeed = -nowSpeed;
        }

        transform.position = initialDirection;//��ʼλ��
    }



}
