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

    private MeleeState state;       //攻撃可能かどうかの状態
    #endregion

    #region メソッド
    void Awake()
    {
        state = MeleeState.NORMAL;      //普通状態（攻撃不能）
    }
    protected override void _OnMove()
    {
        // レイがプレイヤーを探知出来るまでプレイヤーに向かって移動
        if (m_AI.CanRayHitTarget(player.transform.position, 5, "Player", Color.red))
        {
            // プレイヤーとの距離が近すぎだったらさがる
            if (m_AI.IsNearTarget(player.transform.position, 2.0f))
            {
                transform.position += -transform.forward / 50;
                return;
            }
            agent.speed = 0;
            StartRotate(player.transform.position);// 回転中心を決めって、回転開始
            return;
        }
        m_AI.MoveTowardsTarget(agent, player.transform.position, animator);
    }

    void Move(Vector3 target, string tag, Color color)
    {
        // レイがプレイヤーを探知出来るまでプレイヤーに向かって移動
        if (m_AI.CanRayHitTarget(target, 5, name, color))
        {
            // プレイヤーとの距離が近すぎだったらさがる
            if (m_AI.IsNearTarget(target, 2.0f))
            {
                transform.position += -transform.forward / 50;
                return;
            }
            agent.speed = 0;
            StartRotate(target);// 回転中心を決めって、回転開始
            return;
        }
        m_AI.MoveTowardsTarget(agent, target, animator);
    }

    void StartRotate(Vector3 origin)
    {
        StopAllCoroutines();
        StartCoroutine(RotateAroundTarget(origin, m_AI.GetAngle(EnemyGenerator.singleton.Angle)));
    }

    // プレイヤーを囲む
    IEnumerator RotateAroundTarget(Vector3 target, float angle)
    {
        float rotate = m_AI.RotateDirection(m_AI.AngleFromTarget(target), angle);
        while (Mathf.Abs(m_AI.AngleFromTarget(target) - angle) > 10.0f)
        {
            // 他の敵とぶつからないように
            StepBack();
            // 減速しながら回転
            float rotateSpeed = rotate * (Mathf.Abs(Mathf.DeltaAngle(m_AI.AngleFromTarget(target), angle)) / 90.0f);
            transform.RotateAround(target, Vector3.up, rotateSpeed);
            yield return null;
        }
        animator.SetFloat("Speed", -1);
        StartStandBy();
    }

    void StepBack()
    {
        if (m_AI.IsRayHit(transform.right, 2, "Enemy", Color.green) ||
            m_AI.IsRayHit(-transform.right, 2, "Enemy", Color.green))
        {
            transform.position += -transform.forward / 10;
        }
    }

    void StartStandBy()
    {
        state = MeleeState.ATTACKREADY;
        StopAllCoroutines();
        StartCoroutine(StandBy());
    }
    IEnumerator StandBy()
    {
        while (true)
        {
            WaitForAttack();
            yield return null;
        }
    }

    void WaitForAttack()
    {
        if (m_AI.CanRayHitTarget(player.transform.position, 8, "Player", Color.blue)) return;
        StartCoolTime();
    }

    //このメソットを呼べば、攻撃開始
    public override void AttackEnemy()
    {
        if (m_AI.IsNearTarget(player.transform.position, 3.0f)) return;

        Attack();
    }

    public void GatherCalled()
    {
        StopAllCoroutines();
        Vector3 boss = GameObject.FindGameObjectWithTag("Boss").transform.position;
        StartCoroutine(Gather(boss));
    }

    IEnumerator Gather(Vector3 boss)
    {
        while (true)
        {
            Move(boss, "Boss", Color.yellow);
            yield return null;
        }
    }

    protected override void OnAttackReadyStart()
    {
        state = MeleeState.NORMAL;
        m_AI.MoveTowardsTarget(agent, player.transform.position, animator);
        if (player.isPlayerAttacking()) player.SetTarget(this.gameObject);
    }

    protected override void OnAttackReadyUpdate()
    {
        animator.SetTrigger("Attack");
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
        // 攻撃判定が武器のボーンについていけるように
        GameObject hit;
        hit = CreateWeapon(weapon, attackPoint.transform.position, attackPoint.transform.rotation);
        hit.transform.parent = attackPoint.transform;
    }
    #endregion
}
