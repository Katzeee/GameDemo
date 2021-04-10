/*****************************************
 *FileName: SpellCardBase.cs
 *Author: Cat
 *E-Mail: 326578901@qq.com
 *Version: 1.0
 *UnityVersion: 2019.4.11f1
 *Date: 2021-04-06 19:39:36
 *Description: ·û¿¨»ùÀà  
*****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardBase : MonoBehaviour
{
    protected Vector3 shootCenter;

    protected virtual void Init()
    {
        shootCenter = transform.parent.position + new Vector3(0, 0.4f, 0);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
