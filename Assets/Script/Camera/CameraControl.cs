/*****************************************
 *FileName: CameraControl.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-13 17:29:20
 *Description: ���������
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoSingleton<CameraControl>
{

    public float velocitySoomthFactor;
    public Vector3 playerToCameraOffset;//��Ҫ���ڿ��������y��,������̶�ʱ��������ƫ���Զ,��Vector3��Ϊ����drawGizmos������position���
    public Vector2 cameraDeadZone;//������ҳ�����Ƭ����������Ż��ƶ�
    //public Vector4 cameraRange;//��������ܳ����ķ�Χ


    void Start()
    {
        //Init();
    }



    public void Init()
    {
        cameraDeadZone = new Vector2(3, 2);
        velocitySoomthFactor = 2f;
        playerToCameraOffset = new Vector2(0, 1.8f);
        Debug.Log(playerToCameraOffset.ToString());
    }


    private void FixedUpdate()
    {
        if (Mathf.Abs(PlayerInfo.playerTransform.position.x - transform.position.x) > cameraDeadZone.x / 2 || //�������ұ߽�
            (playerToCameraOffset.y - (transform.position.y - PlayerInfo.playerTransform.position.y)) > cameraDeadZone.y / 2 || //�����ϱ߽�
            (transform.position.y - PlayerInfo.playerTransform.position.y) > playerToCameraOffset.y)//�����±߽�
        {

            #region ���������λ��
            Vector3 target = PlayerInfo.playerTransform.position;
            target.y = target.y + playerToCameraOffset.y;
            target.z = transform.position.z;
            #endregion


            //Debug.Log(target.ToString());

            transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * velocitySoomthFactor);
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, mCameraRange.z, mCameraRange.w), Mathf.Clamp(transform.position.y, mCameraRange.y, mCameraRange.x), transform.position.z);//�����ƶ���Χ
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - playerToCameraOffset, new Vector3(cameraDeadZone.x, cameraDeadZone.y));//�����Ͻ�
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(cameraDeadZone.x, 2 * playerToCameraOffset.y));//�½�
    }
}
