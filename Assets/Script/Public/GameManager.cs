/*****************************************
 *FileName: GameManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:35:57
 *Description: ”Œœ∑◊‹π‹¿Ì∆˜
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
        Boss2.Instance.bossFSMManager.OnUpdate();
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 0.01f;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Time.timeScale = 1;
        }
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
        GO.name = "–°”Ò (8)";
        PoolManager.Instance.PoolInit("–°”Ò (8)", ResourceType.Bullet ,"–°”Ò (8)", GO.transform, 10, 10);

        GO = new GameObject();
        GO.name = "‘˙µØ (8)";
        PoolManager.Instance.PoolInit("‘˙µØ (8)", ResourceType.Bullet, "‘˙µØ (8)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "ª∑”Ò (3)";
        PoolManager.Instance.PoolInit("ª∑”Ò (3)", ResourceType.Bullet, "ª∑”Ò (3)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "¥Û”Ò (2)";
        PoolManager.Instance.PoolInit("¥Û”Ò (2)", ResourceType.Bullet, "¥Û”Ò (2)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "¡€µØ(10)";
        PoolManager.Instance.PoolInit("¡€µØ (10)", ResourceType.Bullet, "¡€µØ (10)", GO.transform, 10, 20);
        

    }
}
