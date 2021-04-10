/*****************************************
 *FileName: BossInfoBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-10 15:00:40
 *Description: Boss��Ϣ����,������һ�������ԭ��������Boss���Ҳ��Ҫ���Boss�й�������Ϣ
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfoBase
{

    public Transform bossTransform { get; private set; }
    public Rigidbody2D bossRigidBody { get; private set; }
    public BossInfoBase(Transform tmptransform)
    {
        bossTransform = tmptransform;
        bossRigidBody = bossTransform.GetComponent<Rigidbody2D>();
    }
}
