/*****************************************
 *FileName: SpellCast1.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 23:19:22
 *Description: 符卡1（也就是BOSS发射器）
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard1 : SpellCardBase
{
    float timer = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("子类start");
        Init();
    }



    protected override void Init()
    {
        base.Init();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            timer = 0;
            StopAllCoroutines();
            //StartCoroutine(ShootLine("扎弹 (8)", Mathf.PI, Mathf.PI, 0.1f, 24, 0.005f, Mathf.PI / 12, 20, 0));//单个螺旋子弹
            //StartCoroutine(ShootLine("扎弹 (8)", -Mathf.PI, Mathf.PI, Mathf.PI / 6, 1, 0, 0, 100, 0, Mathf.PI / 24));//四周都在螺旋子弹
            //StartCoroutine(ShootLine("环玉 (3)", - Mathf.PI * 23 / 24, Mathf.PI * 23 / 24, Mathf.PI / 12, 4, 0.2f, 0, 5, 0.5f));
            StartCoroutine(ShootCircle("大玉 (2)", -Mathf.PI, Mathf.PI, Mathf.PI / 6));
        }
    }

    /// <summary>
    /// 发射直线子弹
    /// </summary>
    /// <param name="tmpBulletName">子弹类型</param>
    /// <param name="tmpInitialAngle">初始角度</param>
    /// <param name="tmpLastAngle">最终角度,和上面的初始角度一起组成一片扇形区域</param>
    /// <param name="tmpIntervalAngle">间隔角度</param>
    /// <param name="tmpCountPerLine">每条线上的子弹个数</param>
    /// <param name="tmpWaitTimePerLine">每条线上子弹间隔时间</param>
    /// <param name="tmpRotatePerLine">每条线上的子弹旋转,一般不用，用每波子弹旋转更好</param>
    /// <param name="tmpWave">子弹波数</param>
    /// <param name="tmpWaitTimePerWave">每两波子弹直接间隔时间</param>
    /// <param name="tmpRptatePerWave">每两波子弹之间的旋转</param>
    /// <param name="tmpResetPerWave">每两波子弹转了角度是否还原初值，因为设置了每Line子弹旋转考虑这里是否还原</param>
    /// <returns></returns>
    IEnumerator ShootLine(string tmpBulletName, float tmpInitialAngle, float tmpLastAngle, float tmpIntervalAngle, int tmpCountPerLine = 1, float tmpWaitTimePerLine = 0, float tmpRotatePerLine = 0, int tmpWave = 1, float tmpWaitTimePerWave = 0, float tmpRptatePerWave = 0, bool tmpResetPerWave = false)
    {
        float initialAngle = tmpInitialAngle;
        float lastAngle = tmpLastAngle;//记录初值
        if (tmpIntervalAngle == 0)//旋转角度设为0会导致死循环
        {
            tmpIntervalAngle = 0.1f;
        }
        for (int k = 0; k < tmpWave; k++)
        {
            for (int j = 0; j < tmpCountPerLine; j++)
            {
                for (float i = tmpInitialAngle; i <= tmpLastAngle; i += tmpIntervalAngle)
                {
                    GameObject tmpGO = PoolManager.Instance.Create(tmpBulletName);
                    LineBullet tmpLineBullet = tmpGO.AddComponent<LineBullet>();
                    tmpLineBullet.Init(shootCenter, i, 0.1f);
                }
                tmpInitialAngle += tmpRotatePerLine;
                tmpLastAngle += tmpRotatePerLine;
                yield return new WaitForSeconds(tmpWaitTimePerLine);
            }
            if (tmpResetPerWave)//如果每波结束还原
            {
                tmpInitialAngle = initialAngle;
                tmpLastAngle = lastAngle;//还原初值
            }
            tmpInitialAngle += tmpRptatePerWave;
            tmpLastAngle += tmpRptatePerWave;
            yield return new WaitForSeconds(tmpWaitTimePerWave);
        }
    }


    IEnumerator ShootCircle(string tmpBulletName, float tmpInitialAngle, float tmpLastAngle, float tmpIntervalAngle)
    {
        for (int k = 0; k < 10; k++)
        {
            for (float i = tmpInitialAngle; i <= tmpLastAngle; i += tmpIntervalAngle)
            {
                GameObject tmpGO = PoolManager.Instance.Create(tmpBulletName);
                CircleBullet tmpLineBullet = tmpGO.AddComponent<CircleBullet>();
                tmpLineBullet.Init(shootCenter, 1, 0.1f, i, -0.02f);
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;

    }
}
