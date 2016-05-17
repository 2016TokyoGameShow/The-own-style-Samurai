// ----- ----- ----- ----- -----
//
// ShootEnemyContext
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

[AddComponentMenu("MyScript/ShootEnemyContext")]
public class ShootEnemyContext
{
    [HideInInspector]
	public IStateShootEnemy state;

    [HideInInspector]
    public NavMeshAgent agent;

    public Enemy enemy;
    
    public void _OnMove()
    {
        state.Move();
    }

    public void _OnMoveEnd()
    {
        state.MoveEnd();
    }
}