// ----- ----- ----- ----- -----
//
// IStateShootEnemy
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

[AddComponentMenu("MyScript/IStateShootEnemy")]
public abstract class IStateShootEnemy
{
    public ShootEnemyContext context;

    public abstract void Move();
    public abstract void MoveEnd();
    public abstract void TriggerEnter(Collider other);
    public abstract void TriggerExit(Collider other);
}