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

    [SerializeField]
    MeleeAI m_AI;

    private float speed;
    private MeleeState state;
    #endregion

    #region メソッド
    void Awake()
    {
        speed = Random.Range(2, 5);
        state = MeleeState.NORMAL;
    }
    protected override void _OnMove()
    {
        //if (m_AI.IsRayHitEnemy(transform.position, transform.right) ||
        //    m_AI.IsRayHitEnemy(transform.position, transform.forward))
        //{
        //    for (float i = 0; i <= 1.0f; i += Time.deltaTime)
        //    {
        //        transform.RotateAround(player.transform.position, Vector3.up, angle);
        //        return;
        //    }
        //}
        if (m_AI.CanRayHitPlayer(player.transform.position,5))
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
            {
                transform.position += -transform.forward / 50;
                return;
            }
            //StartStandBy();
            agent.speed = 0;
            StartRotate();
            return;
        }
        m_AI.MoveTowardsTarget(agent, player.transform.position, speed, animator);
    }

    void StartRotate()
    {
        Vector3 rotateOrigin = player.transform.position;    //最初に回転中心を決める
        StopAllCoroutines();
        StartCoroutine(RotateAroundTarget(rotateOrigin, m_AI.GetAngle(EnemyGenerator.singleton.Angle)));
    }

    public IEnumerator RotateAroundTarget(Vector3 target, float angle)
    {
        float rotate = m_AI.RotateLeftOrRight(m_AI.AngleFromTarget(target), angle);
        while (Mathf.Abs(m_AI.AngleFromTarget(target) - angle) > 10.0f)
        {
            transform.RotateAround(target, Vector3.up, rotate);
            yield return null;
        }
        animator.SetFloat("Speed", -1);
        StartStandBy();
    }

    void StartStandBy()
    {
        StopAllCoroutines();
        StartCoroutine(StandBy());
    }
    IEnumerator StandBy()
    {
        state = MeleeState.ATTACKREADY;
        while (true)
        {
            WaitForAttack();
            yield return null;
        }
    }

    void WaitForAttack()
    {
        if (m_AI.CanRayHitPlayer(player.transform.position,8)) return;
        StartCoolTime();
    }

    public void StartAttack()
    {
        if (state != MeleeState.ATTACKREADY) return;
        StopAllCoroutines();
        if (IsRayHitPlayer(maxDistance))
        {
            agent.speed = 0;
            animator.SetFloat("Speed", -1);
            Attack();
        }
        m_AI.MoveTowardsTarget(agent, player.transform.position, speed, animator);
    }

    protected override void OnAttackReadyStart()
    {
        state = MeleeState.NORMAL;
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
    #endregion
}
