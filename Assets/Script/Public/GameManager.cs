/*****************************************
 *FileName: GameManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:35:57
 *Description: 游戏总管理器
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{



    public PETimer globleTimer;
    void Start()
    {
        Init();
        CreatePool();
        PlayerControlFSM.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        globleTimer.Update();
    }

    private void FixedUpdate()
    {
        PlayerControlFSM.Instance.OnFixedUpdate();
    }



    void Init()
    {
        globleTimer = new PETimer();
    }

    void CreatePool()
    {
        GameObject GO = new GameObject();
        GO.name = "小玉 (8)";
        PoolManager.Instance.PoolInit("小玉 (8)", ResourceType.Bullet ,"小玉 (8)", GO.transform, 10, 10);

        GO = new GameObject();
        GO.name = "扎弹 (8)";
        PoolManager.Instance.PoolInit("扎弹 (8)", ResourceType.Bullet, "扎弹 (8)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "环玉 (3)";
        PoolManager.Instance.PoolInit("环玉 (3)", ResourceType.Bullet, "环玉 (3)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "大玉 (2)";
        PoolManager.Instance.PoolInit("大玉 (2)", ResourceType.Bullet, "大玉 (2)", GO.transform, 300, 300);

    }
}
