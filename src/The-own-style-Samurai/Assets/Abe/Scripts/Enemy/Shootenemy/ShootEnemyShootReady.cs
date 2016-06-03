using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("MyScript/ShootEnemyShootReady")]
public class ShootEnemyShootReady: IStateShootEnemy
{

    #region 変数

    Transform enemy;
    Transform player;

    #endregion

    #region プロパティ

    #endregion

    #region メソッド

    public ShootEnemyShootReady(ShootEnemyContext context)
    {
        this.context = context;
        enemy  = context.enemy.transform;
        player = context.enemy.playerObject.transform;
    }

    public override void Move()
    {
        Vector3 dir = player.position - enemy.position;
        dir.y = 0;
        dir.Normalize();

        //固定砲台と化す
        Quaternion from = enemy.rotation;
        Quaternion to   = Quaternion.LookRotation(dir);
        enemy.rotation  = Quaternion.RotateTowards(from, to, context.agent.angularSpeed * Time.deltaTime);

        if(Vector3.Distance(player.position, enemy.position) >= 15)
        {
            context.state = new ShootEnemyApproach(context);
        }
    }

    public override void MoveEnd()
    {
        
    }

    #endregion
}
