using UnityEngine;
using System.Collections;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : Enemy
{

    #region 変数
    [SerializeField, Tooltip("攻撃前の信号")]
    GameObject sign;

    [SerializeField, Tooltip("信号を出す位置")]
    GameObject aboveHead;

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    [SerializeField, Tooltip("アニメーションのコンポーネント")]
    Animator animator;

    [SerializeField, HideInInspector]
    EnemyAction eAction;

    #endregion

    public Animator GetAnimator
    {
        get { return animator; }
    }

    #region メソッド
    void Awake()
    {
        eAction = GetComponent<EnemyAction>();
    }
    protected override void _OnMove()
    {
        if (IsRayHitPlayer(maxDistance))
        {
            agent.speed = 0;
            animator.SetFloat("Speed", agent.speed);
            eAction.ActionStart();
            return;
        }
        agent.destination = player.transform.position;
        agent.speed = moveSpeed;
        animator.SetFloat("Speed", agent.speed);
    }

    protected override void OnAttackReadyStart()
    {
        animator.SetTrigger("Attack");
        Instantiate(sign, aboveHead.transform.position, transform.rotation);
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

    public IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(0.2f);
        Attack();
    }

    public Player GetPlayer()
    {
        return player;
    }

    #endregion
}
