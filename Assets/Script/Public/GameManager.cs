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
    // Start is called before the first frame update
    void Start()
    {
        CreatePool();
        PlayerControlFSM.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(PlayerInfo.playerStateManager.curState);
        }
    }

    private void FixedUpdate()
    {
        PlayerControlFSM.Instance.OnFixedUpdate();
    }

    void CreatePool()
    {
        GameObject GO = new GameObject();
        GO.name = "Poooooool";
        PoolManager.Instance.PoolInit("小玉(8)", resourceType.Bullet ,"小玉 (8)", GO.transform);
    }
}
