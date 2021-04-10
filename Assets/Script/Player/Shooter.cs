/*****************************************
 *FileName: Shooter.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 15:59:41
 *Description: ��������ӵ�����
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    
    float attack1Timer;//���ڹ���һ��ʱ
    Vector3 mousePositionOnScreen;//���ڱ����������
    Vector3 mousePositionInWorld;//����������λ��ת����������λ��
    Transform shooterTransform;

    [Header("����")]
    [Tooltip("�ӵ��ٶ�")]
    public float bulletSpeed;
    [Tooltip("������")]
    public float attack1CD;
    [Tooltip("�ӵ����ٶ�")]
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
        #region ����1
        attack1Timer += Time.deltaTime;
        if (PlayerInfo.playerAnimator.GetInteger("curState") == (int)PlayerInfo.PlayerState.PlayerAttack1)
        {
            if (attack1Timer >= attack1CD)
            {
                attack1Timer = 0;

                #region ��ȡ���λ��
                mousePositionOnScreen = Input.mousePosition;
                mousePositionOnScreen.z = Mathf.Abs(Camera.main.transform.position.z);//������Ҫ���������z��λ��
                mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
                #endregion
                //Debug.Log(mousePositionInWorld.ToString());

                #region �����ӵ�
                GameObject tmpGO = PoolManager.Instance.Create("�۵� (10)");
                LineBullet tmpLineBullet = tmpGO.AddComponent<LineBullet>();
                tmpLineBullet.Init(shooterTransform.position, mousePositionInWorld, bulletSpeed, true, acceleration);
                #endregion

                #region �ı䳯��
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

        #region �Ҽ�,��ʱ���ù�
        //if (PlayerInfo.playerAnimator.GetInteger("curState") == (int)PlayerInfo.PlayerState.PlayerAttack2)
        //{
        //    if (attack1Timer >= attack1CD)
        //    {
        //        #region ��ȡ���λ��
        //        mousePositionOnScreen = Input.mousePosition;
        //        mousePositionOnScreen.z = Mathf.Abs(Camera.main.transform.position.z);//������Ҫ���������z��λ��
        //        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
        //        #endregion
        //        //Debug.Log(mousePositionInWorld.ToString());

        //        #region �����ӵ�
        //        attack1Timer = 0;
        //        GameObject tmpGO = PoolManager.Instance.Create("С�� (8)");
        //        CircleBullet tmpLineBullet = tmpGO.AddComponent<CircleBullet>();
        //        tmpLineBullet.Init(shooterTransform.position);
        //        #endregion

        //        #region �ı䳯��
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
