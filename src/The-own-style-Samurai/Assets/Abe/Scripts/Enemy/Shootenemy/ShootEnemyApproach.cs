// ----- ----- ----- ----- -----
//
// ShootEnemyApproach
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

[AddComponentMenu("MyScript/ShootEnemyApproach")]
public class ShootEnemyApproach : IStateShootEnemy
{
    
    #region 変数

    //[SerializeField, Tooltip("説明文")]
    Transform player;

    NavMeshAgent agent;

    #endregion


    #region プロパティ



    #endregion


    #region メソッド

    public ShootEnemyApproach(ShootEnemyContext context)
    {
        this.context = context;
        agent  = context.agent;
        player = context.enemy.playerObject.transform;
        agent.destination = player.position;
    }

    public override void Move()
    {
        agent.destination = player.position;
        agent.speed       = ((ShootEnemy)context.enemy).MoveSpeed;
        if(Vector3.Distance(player.position, context.enemy.transform.position) <= 10)
        {
            context.state = new ShootEnemyEscape(context);
        }
    }

    public override void MoveEnd()
    {
        agent.destination = context.enemy.transform.position;
        agent.speed       = 0;
    }

    #endregion
}