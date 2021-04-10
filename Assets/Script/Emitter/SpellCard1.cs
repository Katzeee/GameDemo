/*****************************************
 *FileName: SpellCast1.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 23:19:22
 *Description: ����1��Ҳ����BOSS��������
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
        //Debug.Log("����start");
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
            //StartCoroutine(ShootLine("���� (8)", Mathf.PI, Mathf.PI, 0.1f, 24, 0.005f, Mathf.PI / 12, 20, 0));//���������ӵ�
            //StartCoroutine(ShootLine("���� (8)", -Mathf.PI, Mathf.PI, Mathf.PI / 6, 1, 0, 0, 100, 0, Mathf.PI / 24));//���ܶ��������ӵ�
            //StartCoroutine(ShootLine("���� (3)", - Mathf.PI * 23 / 24, Mathf.PI * 23 / 24, Mathf.PI / 12, 4, 0.2f, 0, 5, 0.5f));
            StartCoroutine(ShootCircle("���� (2)", -Mathf.PI, Mathf.PI, Mathf.PI / 6));
        }
    }

    /// <summary>
    /// ����ֱ���ӵ�
    /// </summary>
    /// <param name="tmpBulletName">�ӵ�����</param>
    /// <param name="tmpInitialAngle">��ʼ�Ƕ�</param>
    /// <param name="tmpLastAngle">���սǶ�,������ĳ�ʼ�Ƕ�һ�����һƬ��������</param>
    /// <param name="tmpIntervalAngle">����Ƕ�</param>
    /// <param name="tmpCountPerLine">ÿ�����ϵ��ӵ�����</param>
    /// <param name="tmpWaitTimePerLine">ÿ�������ӵ����ʱ��</param>
    /// <param name="tmpRotatePerLine">ÿ�����ϵ��ӵ���ת,һ�㲻�ã���ÿ���ӵ���ת����</param>
    /// <param name="tmpWave">�ӵ�����</param>
    /// <param name="tmpWaitTimePerWave">ÿ�����ӵ�ֱ�Ӽ��ʱ��</param>
    /// <param name="tmpRptatePerWave">ÿ�����ӵ�֮�����ת</param>
    /// <param name="tmpResetPerWave">ÿ�����ӵ�ת�˽Ƕ��Ƿ�ԭ��ֵ����Ϊ������ÿLine�ӵ���ת���������Ƿ�ԭ</param>
    /// <returns></returns>
    IEnumerator ShootLine(string tmpBulletName, float tmpInitialAngle, float tmpLastAngle, float tmpIntervalAngle, int tmpCountPerLine = 1, float tmpWaitTimePerLine = 0, float tmpRotatePerLine = 0, int tmpWave = 1, float tmpWaitTimePerWave = 0, float tmpRptatePerWave = 0, bool tmpResetPerWave = false)
    {
        float initialAngle = tmpInitialAngle;
        float lastAngle = tmpLastAngle;//��¼��ֵ
        if (tmpIntervalAngle == 0)//��ת�Ƕ���Ϊ0�ᵼ����ѭ��
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
            if (tmpResetPerWave)//���ÿ��������ԭ
            {
                tmpInitialAngle = initialAngle;
                tmpLastAngle = lastAngle;//��ԭ��ֵ
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
