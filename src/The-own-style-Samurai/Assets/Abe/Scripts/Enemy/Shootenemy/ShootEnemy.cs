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

    [SerializeField, Range(0.0f, 30.0f), Tooltip("攻撃準備時 プレイヤーを向くスピード")]
    float attackRotateSpeed;

    [SerializeField]
    float escapeMoveSpeed;

    ShootEnemyContext context;

    public readonly Vector3 rayOffset = new Vector3(0, 1f, 0);

    #endregion

    #region プロパティ

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public float EscapeMoveSpeed
    {
        get
        {
            return escapeMoveSpeed;
        }
    }

    #endregion

    #region メソッド

    void OnAwake()
    {
        animator.SetBool("IsGround", true);
    }

    protected override void OnStart()
    {
        lineRenderer.enabled = false;
        context = new ShootEnemyContext();
        context.agent = agent;
        context.enemy = this;
        context.state = new ShootEnemyApproach(context);
    }

    protected override void _OnMove()
    {
#if UNITY_EDITOR
        if(!agent.enabled)
            return;
#endif

        //敵以外のレイヤーで判定
        if(IsRayHitPlayer(maxDistance, ~(1 << LayerMask.NameToLayer("Enemy")), rayOffset))
        {
            Attack();
            return;
        }
        context._OnMove();
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    protected override void _OnMoveEnd()
    {
        context._OnMoveEnd();
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    protected override void OnAttackReadyStart()
    {
        animator.SetTrigger("Attack");
        lineRenderer.enabled = true;
    }

    protected override void OnAttackReadyUpdate()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position + rayOffset, transform.forward, out hitInfo))
        {
            if(hitInfo.collider.tag != "Player")
            {
                animator.SetTrigger("AttackEnd");
                AttackCancel();
            }
        }

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 dir = player.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        
        Quaternion playerAngle = Quaternion.LookRotation(player.transform.position - transform.position);
        Quaternion.RotateTowards(transform.rotation, playerAngle, agent.angularSpeed * Time.deltaTime);
    }

    protected override void OnAttack()
    {
        animator.SetTrigger("AttackEnd");
        lineRenderer.enabled = false;
        CreateWeapon(weapon, attackPoint.transform.position, transform.rotation);

        Dead(2);
    }

    protected override void OnAttackCancel()
    {
        lineRenderer.enabled = false;
    }

    protected override void PlayerDead()
    {
        agent.destination = transform.position + transform.forward * 0.3f;
    }

    public void OnDestroy()
    {
        //パーティクル
    }

    #endregion
}