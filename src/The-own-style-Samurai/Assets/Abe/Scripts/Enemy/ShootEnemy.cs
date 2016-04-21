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
public class ShootEnemy : IEnemy
{
    #region 変数

    [SerializeField, Tooltip("武器")]
    IShootWeapon weapon;

    [SerializeField, Tooltip("プレイヤー")]
    Player player;

    [SerializeField, Tooltip("飛道具が発射されるポイント")]
    GameObject shootPoint;

    [SerializeField, Tooltip("移動のスピード")]
    float moveSpeed;

    [SerializeField, Tooltip("射程")]
    float range;

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    GameObject _weapon;

    #endregion

    #region プロパティ



    #endregion


    #region メソッド
    protected override void _OnMove()
    {
        if(IsRayHitPlayer(range))
        {
            agent.destination = transform.position + transform.forward / 0.8f;
            Attack();
        }
        else
        {
            agent.destination  = player.transform.position;
            agent.speed        = moveSpeed;
        }
    }

    protected override void OnAttackReadyUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;

    }

    protected override void OnAttack()
    {
        Instantiate(weapon, shootPoint.transform.position, transform.rotation);
    }

    protected override void PlayerDead()
    {
        agent.destination = transform.position + transform.forward / 0.8f;
    }

    #endregion
}