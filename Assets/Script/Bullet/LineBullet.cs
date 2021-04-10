/*****************************************
 *FileName: LineBullet.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 14:15:57
 *Description: ֱ�߹켣�ӵ�
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : BulletBase
{
    [Tooltip("ͨ����������ĽǶ�")]
    protected float angle;

    protected override Vector3 nextDirection => transform.position + new Vector3(Mathf.Cos(nowAngle) * nowSpeed, Mathf.Sin(nowAngle) * nowSpeed);

    Vector3 temp;

    void FixedUpdate()
    {
        transform.position = nextDirection;//ÿ֡����nextDirection����λ��



        if (acceleration != 0)
        {
            nowSpeed = nowSpeed > 0 ? nowSpeed + acceleration : nowSpeed - acceleration;//���ٶ�
        }

        if (isRotate)
        {
//            Debug.Log((nowAngle * Mathf.Rad2Deg).ToString());

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * nowAngle - 90));
        }
    }


    /// <summary>
    /// ������ʼ��
    /// </summary>
    /// <param name="tmpInitialPosition">��ʼ����λ��</param>
    /// <param name="tmpDestinationPosition">Ŀ��λ��</param>
    /// <param name="tmpInitalSpeed">���ٶ�</param>
    /// <param name="tmpAcceleration">���ٶ�</param>
    public void Init(Vector3 tmpInitialPosition, Vector3 tmpDestinationPosition, float tmpInitalSpeed = 0.5f, bool tmpIsRotate = false, float tmpAcceleration = 0)
    {

        transform.position = tmpInitialPosition;
        gameObject.SetActive(true);//��������Ϊtrue,ȡ������ʱ��ֱ����true�ᵼ�����ϴ������ص�����һ��

        #region ��ʼ����ֵ
        initialPosition = tmpInitialPosition;
        destinationPosition = tmpDestinationPosition;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        destinationPosition = destinationPosition - initialPosition;//���㳯��������
        angle = Mathf.Atan(destinationPosition.y / destinationPosition.x);//ͨ���������㳯��Ƕ�
        nowSpeed = initialSpeed;//�趨�ٶ�
        nowAngle = angle;//�趨�Ƕ�
        isRotate = tmpIsRotate;
        #endregion

        #region �������߷���
        //��ΪArctan��ֵ��ֻ��-pi/2��pi/2,���Ի���Ҫx�᷽��֪���ӵ������ı߷�
        if (destinationPosition.x < 0)//������߷���,�����ٶ����������Լ���ת��ͼ����,��ת������������Է��ڻ��ճ��ӵĵط���
        {
            nowSpeed = -nowSpeed;
            transform.localScale = new Vector3(1, -1, 1);
        }
        #endregion

    }

    /// <summary>
    /// ����Init����Boss�����ӵ�
    /// </summary>
    /// <param name="tmpInitialPosition"></param>
    /// <param name="tmpAngle">����Ƕ�</param>
    /// <param name="tmpInitalSpeed"></param>
    /// <param name="tmpAcceleration"></param>
    public void Init(Vector3 tmpInitialPosition, float tmpAngle, float tmpInitalSpeed = 0.5f, float tmpAcceleration = 0)
    {
        #region ��ʼ����ֵ
        initialPosition = tmpInitialPosition;
        initialSpeed = tmpInitalSpeed;
        acceleration = tmpAcceleration;
        nowSpeed = initialSpeed;//�趨�ٶ�
        nowAngle = tmpAngle;//�趨�Ƕ�
        #endregion

        if (destinationPosition.x < 0)//������߷���
        {
            nowSpeed = -nowSpeed;
        }

        transform.position = initialPosition;//��ʼλ��
    }



}
