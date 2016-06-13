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

    Vector3 previousPosition;

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
        Vector3 dir = enemy.position - player.position;
        dir.y = 0;
        dir.Normalize();

        //回転
        Quaternion from = enemy.rotation;
        Quaternion to   = Quaternion.LookRotation(dir);
        enemy.rotation = Quaternion.RotateTowards(from, to, context.agent.angularSpeed * Time.deltaTime);
        
        ShootEnemy shootEnemy = (ShootEnemy)context.enemy;

        //手動で動かす
        context.agent.Move(shootEnemy.EscapeMoveSpeed * dir * Time.deltaTime);

        //距離が開いたら近づくようにする
        if(Vector3.Distance(player.position, enemy.position) >= 15)
        {
            context.state = new ShootEnemyApproach(context);
        }
        
        float moveDistance = Vector3.Distance(enemy.position,  previousPosition);

        context.animator.SetFloat("Speed", 1);

        //端に追い詰められたとき
        if(moveDistance < shootEnemy.EscapeMoveSpeed * Time.unscaledDeltaTime/2)
        {
            context.state = new ShootEnemyShootReady(context);
        }
        previousPosition = enemy.position;
    }

    public override void MoveEnd()
    {
        context.agent.destination = enemy.position;
        context.agent.speed = 0;
    }

    #endregion
}