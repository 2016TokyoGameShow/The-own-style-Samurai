// ----- ----- ----- ----- -----
//
// Archer
//
// 作成日：2016/04/19
// 作成者：阿部
//
// <概要>
// 遠距離攻撃をする敵です
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Enemy/ShootEnemy")]
public class ShootEnemy : Enemy
{
    #region 変数

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    #endregion

    #region プロパティ

    #endregion

    #region メソッド
    protected override void _OnMove()
    {
#if UNITY_EDITOR
        if(!agent.enabled) return;
#endif

        //敵以外のレイヤーで判定
        if (IsRayHitPlayer(maxDistance, ~(1<<gameObject.layer)))
        {
            //急に止まらないように
            agent.destination = transform.position;// + transform.forward;
            agent.speed = 0;
            Attack();
            return;
        }

        agent.destination  = player.transform.position;
        agent.speed        = moveSpeed;
    }

    protected override void OnAttackReadyUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
    }

    protected override void OnAttack()
    {
        Instantiate(weapon, attackPoint.transform.position, transform.rotation);
    }

    protected override void PlayerDead()
    {
        agent.destination = transform.position + transform.forward * 0.3f;
    }

#endregion
}