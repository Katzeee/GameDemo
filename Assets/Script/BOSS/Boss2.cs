/*****************************************
 *FileName: Boss2.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-09 20:28:34
 *Description: Boss2¿ØÖÆÆ÷
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : BossBase
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            timer = 0;
            StartCoroutine(ShootLine(-Mathf.PI / 2, Mathf.PI / 2, Mathf.PI / 6, 2, 0.2f));
        }
    
    }


    IEnumerator ShootLine(float tmpInitialAngle, float tmpLastAngle, float itmpIntervalAngle, int tmpCountPerLine = 1,float tmpWaitTimePerLine = 0, int tmpWave = 1, float tmpWaitTimePerWave = 0)
    {
        for (int j = 0; j < tmpCountPerLine; j++)
        {
            for (float i = tmpInitialAngle; i <= tmpLastAngle; i += itmpIntervalAngle)
            {
                GameObject tmpGO = PoolManager.Instance.Create("Ôúµ¯ (8)");
                LineBullet tmpLineBullet = tmpGO.AddComponent<LineBullet>();
                tmpLineBullet.Init(transform.position, i, 0.1f);
            }
            yield return new WaitForSeconds(tmpWaitTimePerLine);
        }
        yield return new WaitForSeconds(tmpWaitTimePerWave);
    }



}
