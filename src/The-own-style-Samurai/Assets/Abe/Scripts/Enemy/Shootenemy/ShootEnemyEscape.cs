// ----- ----- ----- ----- -----
//
// ShootEnemyEscape
//
// 作成日：
// 作成者：
//
// <概要>
//
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("MyScript/ShootEnemyEscape")]
public class ShootEnemyEscape : IStateShootEnemy
{
    #region 変数

    //[SerializeField, Tooltip("説明文")]
    Transform enemy;
    Transform player;

    #endregion


    #region プロパティ



    #endregion


    #region メソッド

    // 初期化処理
    public ShootEnemyEscape(ShootEnemyContext context)
    {
        this.context = context;
        enemy  = context.enemy.transform;
        player = context.enemy.playerObject.transform;

        RaycastHit hitInfo;

#if UNITY_EDITOR
        bool ishit = Physics.Raycast(enemy.position, 
                        enemy.forward ,
                        out hitInfo   ,
                        Mathf.Infinity, 
                        LayerMask.NameToLayer("Wall")
        );

        Debug.Assert(ishit);
#else
        Physics.Raycast(enemy.position, 
                        enemy.forward ,
                        out hitInfo   ,
                        Mathf.Infinity, 
                        LayerMask.NameToLayer("Wall")
        );
#endif
        Vector3 point = hitInfo.point;
        point.y = 0;

        context.agent.destination = hitInfo.point;
    } 


    public override void Move()
    {
        
    }

    public override void MoveEnd()
    {
        context.agent.destination = context.enemy.transform.position;
        context.agent.speed = 0;
    }

    public override void TriggerEnter(Collider other)
    {
        if(other.tag == "Wall")
        {
            //追い詰められたとき
        }
    }
    public override void TriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            context.state = new ShootEnemyApproach(context);
        }
    }
#endregion
}