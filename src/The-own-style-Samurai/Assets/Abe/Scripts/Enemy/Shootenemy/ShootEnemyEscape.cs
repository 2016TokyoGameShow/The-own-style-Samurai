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

        //前のパスに影響されて正常に逃げてくれないため
        context.agent.ResetPath();
    }


    public override void Move()
    {
        Vector3 dir = context.enemy.transform.position - player.transform.position;
        dir.y = 0;
        dir.Normalize();

        //手動で動かす
        context.agent.Move(((ShootEnemy)context.enemy).EscapeMoveSpeed * dir * Time.deltaTime);

        //回転
        Quaternion from = context.enemy.transform.rotation;
        Quaternion to   = Quaternion.LookRotation(dir);
        context.enemy.transform.rotation = Quaternion.RotateTowards(from, to, context.agent.angularSpeed * Time.deltaTime);

        if(Vector3.Distance(player.position, context.enemy.transform.position) >= 15)
        {
            context.state = new ShootEnemyApproach(context);
        }
    }

    public override void MoveEnd()
    {
        context.agent.destination = context.enemy.transform.position;
        context.agent.speed = 0;
    }

    #endregion
}