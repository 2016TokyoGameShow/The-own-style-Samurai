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
    Transform enemy;

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
        enemy  = context.enemy.transform;
        agent.destination = player.position;
    }

    public override void Move()
    {
        agent.destination = player.position;
        agent.speed       = ((ShootEnemy)context.enemy).MoveSpeed;

        //destinationを設定するだけではうまくプレイヤーの方向を向いてくれないため
        Vector3 dir = player.position - enemy.position;
        dir.y = 0;
        dir.Normalize();

        Quaternion from = enemy.rotation;
        Quaternion to   = Quaternion.LookRotation(dir);
        enemy.rotation  = Quaternion.RotateTowards(from, to, context.agent.angularSpeed * Time.deltaTime);
        
        if(Vector3.Distance(player.position, context.enemy.transform.position) <= 10)
        {
            context.state = new ShootEnemyEscape(context);
        }

        context.animator.SetFloat("Speed", context.agent.velocity.magnitude);
    }

    public override void MoveEnd()
    {
        agent.destination = context.enemy.transform.position;
        agent.speed       = 0;
    }

    #endregion
}