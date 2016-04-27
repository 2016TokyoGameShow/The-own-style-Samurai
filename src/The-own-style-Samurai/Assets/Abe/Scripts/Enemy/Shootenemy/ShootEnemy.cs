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

    [SerializeField, Tooltip("アニメーションのコンポーネント")]
    Animator animator;

    [SerializeField, Tooltip("LineRendererのコンポーネント")]
    LineRenderer lineRenderer;

    Vector3 rayOffset = new Vector3(0, 0.8f, 0);

    #endregion

    #region プロパティ

    #endregion

    #region メソッド

    protected override void OnStart()
    {
        lineRenderer.enabled = false;
    }

    protected override void _OnMove()
    {
#if UNITY_EDITOR
        if(!agent.enabled) return;
#endif

        //敵以外のレイヤーで判定
        if (IsRayHitPlayer(maxDistance, ~(1<<gameObject.layer), rayOffset))
        {
            //急に止まらないように
            agent.destination = transform.position;// + transform.forward;
            agent.speed = 0;
            animator.SetFloat("Speed", agent.speed);
            Attack();
            return;
        }

        agent.destination  = player.transform.position;
        agent.speed        = moveSpeed;
        animator.SetFloat("Speed", agent.speed);
    }

    protected override void OnAttackReadyStart()
    {
        animator.SetTrigger("Attack");
        lineRenderer.enabled = true;
    }

    protected override void OnAttack()
    {
        animator.SetTrigger("AttackEnd");
        lineRenderer.enabled = false;
        Instantiate(weapon, attackPoint.transform.position, transform.rotation);
    }

    protected override void PlayerDead()
    {
        agent.destination = transform.position + transform.forward * 0.3f;
    }

#endregion
}