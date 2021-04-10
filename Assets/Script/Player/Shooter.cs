/*****************************************
 *FileName: Shooter.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 15:59:41
 *Description: 控制玩家子弹发射
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    
    float attack1Timer;//用于攻击一计时
    Vector3 mousePositionOnScreen;//用于保存鼠标坐标
    Vector3 mousePositionInWorld;//保存玩家鼠标位置转世界坐标后的位置
    Transform shooterTransform;

    [Header("攻击")]
    [Tooltip("子弹速度")]
    public float bulletSpeed;
    [Tooltip("发射间隔")]
    public float attack1CD;
    [Tooltip("子弹加速度")]
    public float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        bulletSpeed = 0.4f;
        attack1CD = 0.15f;
        shooterTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        #region 攻击1
        attack1Timer += Time.deltaTime;
        if (PlayerInfo.playerAnimator.GetInteger("curState") == (int)PlayerInfo.PlayerState.PlayerAttack1)
        {
            if (attack1Timer >= attack1CD)
            {
                attack1Timer = 0;

                #region 获取鼠标位置
                mousePositionOnScreen = Input.mousePosition;
                mousePositionOnScreen.z = Mathf.Abs(Camera.main.transform.position.z);//这里需要是摄像机的z轴位置
                mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
                #endregion
                //Debug.Log(mousePositionInWorld.ToString());

                #region 发射子弹
                GameObject tmpGO = PoolManager.Instance.Create("鳞弹 (10)");
                LineBullet tmpLineBullet = tmpGO.AddComponent<LineBullet>();
                tmpLineBullet.Init(shooterTransform.position, mousePositionInWorld, bulletSpeed, true, acceleration);
                #endregion

                #region 改变朝向
                if (mousePositionInWorld.x > PlayerInfo.playerTransform.position.x && PlayerInfo.playerTransform.localScale.x == -1)
                {
                    PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
                }
                else if(mousePositionInWorld.x < PlayerInfo.playerTransform.position.x && PlayerInfo.playerTransform.localScale.x == 1)
                {
                    PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);
                }
                #endregion
            }
        }
        #endregion

        #region 右键,暂时不用管
        //if (PlayerInfo.playerAnimator.GetInteger("curState") == (int)PlayerInfo.PlayerState.PlayerAttack2)
        //{
        //    if (attack1Timer >= attack1CD)
        //    {
        //        #region 获取鼠标位置
        //        mousePositionOnScreen = Input.mousePosition;
        //        mousePositionOnScreen.z = Mathf.Abs(Camera.main.transform.position.z);//这里需要是摄像机的z轴位置
        //        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        //        #endregion
        //        //Debug.Log(mousePositionInWorld.ToString());

        //        #region 发射子弹
        //        attack1Timer = 0;
        //        GameObject tmpGO = PoolManager.Instance.Create("小玉 (8)");
        //        CircleBullet tmpLineBullet = tmpGO.AddComponent<CircleBullet>();
        //        tmpLineBullet.Init(shooterTransform.position);
        //        #endregion

        //        #region 改变朝向
        //        if (mousePositionInWorld.x > PlayerInfo.playerTransform.position.x && PlayerInfo.playerTransform.localScale.x == -1)
        //        {
        //            PlayerInfo.playerTransform.localScale = new Vector3(1, 1, 1);
        //        }
        //        else if (mousePositionInWorld.x < PlayerInfo.playerTransform.position.x && PlayerInfo.playerTransform.localScale.x == 1)
        //        {
        //            PlayerInfo.playerTransform.localScale = new Vector3(-1, 1, 1);
        //        }
        //        #endregion
        //    }
        //}
        #endregion

    }
}
