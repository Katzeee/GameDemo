/*****************************************
 *FileName: GameManager.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 20:35:57
 *Description: ��Ϸ�ܹ�����
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
        GO.name = "С�� (8)";
        PoolManager.Instance.PoolInit("С�� (8)", ResourceType.Bullet ,"С�� (8)", GO.transform, 10, 10);

        GO = new GameObject();
        GO.name = "���� (8)";
        PoolManager.Instance.PoolInit("���� (8)", ResourceType.Bullet, "���� (8)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "���� (3)";
        PoolManager.Instance.PoolInit("���� (3)", ResourceType.Bullet, "���� (3)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "���� (2)";
        PoolManager.Instance.PoolInit("���� (2)", ResourceType.Bullet, "���� (2)", GO.transform, 300, 300);

        GO = new GameObject();
        GO.name = "�۵�(10)";
        PoolManager.Instance.PoolInit("�۵� (10)", ResourceType.Bullet, "�۵� (10)", GO.transform, 10, 20);
        

    }
}
