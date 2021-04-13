/*****************************************
 *FileName: CameraControl.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-13 17:29:20
 *Description: 摄像机控制
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControl : MonoSingleton<CameraControl>
{

    public float velocitySoomthFactor;
    public Vector3 playerToCameraOffset;//主要用于控制摄像机y轴,摄像机固定时相对于玩家偏离多远,用Vector3是为了在drawGizmos里面与position相减
    public Vector2 cameraDeadZone;//控制玩家超出哪片区域摄像机才会移动
    //public Vector4 cameraRange;//摄像机不能超出的范围


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
        if (Mathf.Abs(PlayerInfo.playerTransform.position.x - transform.position.x) > cameraDeadZone.x / 2 || //超出左右边界
            (playerToCameraOffset.y - (transform.position.y - PlayerInfo.playerTransform.position.y)) > cameraDeadZone.y / 2 || //超出上边界
            (transform.position.y - PlayerInfo.playerTransform.position.y) > playerToCameraOffset.y)//超出下边界
        {

            #region 更正摄像机位置
            Vector3 target = PlayerInfo.playerTransform.position;
            target.y = target.y + playerToCameraOffset.y;
            target.z = transform.position.z;
            #endregion


            //Debug.Log(target.ToString());

            transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * velocitySoomthFactor);
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, mCameraRange.z, mCameraRange.w), Mathf.Clamp(transform.position.y, mCameraRange.y, mCameraRange.x), transform.position.z);//限制移动范围
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - playerToCameraOffset, new Vector3(cameraDeadZone.x, cameraDeadZone.y));//左右上界
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(cameraDeadZone.x, 2 * playerToCameraOffset.y));//下界
    }
}
