using UnityEngine;
using System.Collections;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : MonoBehaviour, WeaponHitHandler, PlayerDeadHandler
{
    #region 変数
    [SerializeField, Tooltip("敵の種類")]
    EnemyController.EnemyKind kind;

    [SerializeField, HideInInspector]
    NavMeshAgent agent;

    [SerializeField, HideInInspector]
    Animator animator;

    [SerializeField, Range(0, 3), Tooltip("移動のスピード")]
    protected float moveSpeed;

    [SerializeField, Tooltip("武器")]
    protected IWeapon weapon;

    [SerializeField, Tooltip("武器生成位置")]
    Transform attackPoint;

    [SerializeField, HideInInspector]
    Player player;

    [SerializeField]
    MeleeAI m_AI;

    private MeleeState state;       //攻撃可能かどうかの状態
    private Vector3 target;
    #endregion
    public EnemyController.EnemyKind Kind
    {
        get { return kind; }
    }

    #region メソッド
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        state = MeleeState.NORMAL;              // 普通状態　攻撃不能
        player = EnemyController.singleton.player;
        EnemyController.singleton.AddEnemy(gameObject, kind);
        //animator.SetTrigger("StartFlow");
        AttackEnemy();
    }

    void Update()
    {

    }

    void ChasePlayer()
    {
        // レイがプレイヤーを探知出来るまでプレイヤーに向かって移動
        if (m_AI.CanRayHitTarget(player.transform.position, 5, "Player", Color.red))
        {
            agent.speed = 0;
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

    public virtual void OnWeaponHit(int damage, GameObject attackObject)
    {
        EnemyController.singleton.EraseEnemy(gameObject, kind);
        EnemyController.singleton.AttackEnd(gameObject, kind);
        EnemyController.singleton.AddDeathCount();
        Destroy(gameObject);
    }

    public void OnPlayerDead()
    {
        StopAllCoroutines();
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
            transform.position += -transform.forward / 30;
        }
    }

    void StartStandBy()
    {
        state = MeleeState.ATTACKREADY;
        //transform.rotation = Quaternion.Euler(player.transform.forward);
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
        //if (m_AI.CanRayHitTarget(player.transform.position, 8, "Player", Color.blue)) return;
        //StartCoolTime();
        AttackEnemy();
    }

    //このメソットを呼べば、攻撃開始
    public void AttackEnemy()
    {
        // プレイヤーと離れすぎだったら攻撃不能
        if (m_AI.IsNearTarget(player.transform.position, 5.0f)) return;
        if (!EnemyController.singleton.Attack(gameObject, kind)) return;
        StopAllCoroutines();
        target = player.transform.position;
        m_AI.MoveTowardsTarget(agent, target, animator);
        StartCoroutine(AttackReady(target));
    }

    IEnumerator AttackReady(Vector3 target)
    {
        while (!m_AI.IsNearTarget(target, 2.0f))
        {
            yield return null;
        }
        animator.SetTrigger("Attack");
        if (player.isPlayerAttacking()) player.SetTarget(this.gameObject);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (!player.GetPlayerAttacking())
        {
            yield return null;
        }
        if (player.GetEnemyTarget() != gameObject) yield return null;
        //if (player.GetPlayerAttacking())
        //{
        Debug.Log("!hbsfkjdsjldfjsdfjl");
        StopAllCoroutines();
        agent.Stop();
        Flow();
        yield return null;
        //}
        //yield return new WaitForSeconds(1);
        //InstantiateWeapon();
        //animator.SetTrigger("AttackEnd");
        //player.SetTarget(null);
    }

    public void GatherCalled()
    {
        StopAllCoroutines();
        Vector3 boss = GameObject.FindGameObjectWithTag("Boss").transform.position;
    }

    protected void OnAttack()
    {
        InstantiateWeapon();
        player.SetTarget(null);
    }

    //判定生成メソット、生成判定、生成位置
    void InstantiateWeapon()
    {
        // 攻撃判定が武器のボーンについていけるように
        IWeapon hit;
        hit = Instantiate(weapon, attackPoint.position, attackPoint.rotation) as IWeapon;
        hit.transform.parent = attackPoint.transform;
    }

    void Flow()
    {
        animator.SetTrigger("StartFlow");
        transform.rotation = Quaternion.Euler(player.transform.forward);
        //transform.position = player.transform.position+transform.right;
    }
    #endregion
}
