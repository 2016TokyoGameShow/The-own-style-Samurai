using UnityEngine;
using System.Collections;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : Enemy
{
    #region 変数
    [SerializeField, Tooltip("信号を出す位置")]
    GameObject aboveHead;

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    [SerializeField, Tooltip("アニメーションのコンポーネント")]
    Animator animator;

    [SerializeField,Tooltip("行動パターン")]
    IMeleeState meleeState;

    #endregion

    public Animator GetAnimator
    {
        get { return animator; }
    }

    #region メソッド
    protected override void _OnMove()
    {
        if (IsRayHitPlayer(maxDistance))
        {
            agent.speed = 0;
            animator.SetFloat("Speed", agent.speed);
            meleeState.ActionStart();
            return;
        }
        agent.destination = player.transform.position;
        agent.speed = moveSpeed;
        animator.SetFloat("Speed", agent.speed);
    }

    protected override void OnAttackReadyStart()
    {
        animator.SetTrigger("Attack");
        if (player.isPlayerAttacking()) player.SetTarget(this.gameObject);
    }

    protected override void OnAttackReadyUpdate()
    {
    }

    protected override void OnAttack()
    {
        AttackInstantiate();
        animator.SetTrigger("AttackEnd");
        player.SetTarget(null);
    }

    //判定生成メソット、生成判定、生成位置
    protected void AttackInstantiate()
    {
        GameObject hit;

        hit = CreateWeapon(weapon, attackPoint.transform.position, attackPoint.transform.rotation);
        hit.transform.parent = attackPoint.transform;
    }

    public void StartAttack()
    {
        Attack();
    }

    public Player GetPlayer()
    {
        return player;
    }

    #endregion
}
