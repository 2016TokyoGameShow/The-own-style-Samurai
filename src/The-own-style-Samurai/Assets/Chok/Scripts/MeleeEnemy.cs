using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : MonoBehaviour, WeaponHitHandler, PlayerDeadHandler
{
    #region 変数
    [SerializeField, Tooltip("敵の種類")]
    protected EnemyController.EnemyKind kind;

    [SerializeField, HideInInspector]
    private NavMeshAgent agent;

    [SerializeField, HideInInspector]
    private Animator animator;

    [SerializeField]
    private MeleeAI mAI;

    [SerializeField]
    private MeleeMove move;

    [SerializeField]
    private MeleeGather gather;

    [SerializeField]
    private MeleeAttack attack;

    private Player player;
    private MeleeState state;
    #endregion

    public Player playerObject { get { return player; } }

    public EnemyController.EnemyKind Kind { get { return kind; } }

    public Animator GetAnimator { get { return animator; } }

    #region メソッド
    public void Start()
    {
        state = MeleeState.NORMAL;
        player = EnemyController.singleton.player;
        EnemyController.singleton.AddEnemy(gameObject, kind);
        AttackEnemy();
    }

    public void Update()
    {
        //Move(player.transform.position, "Player", Color.red);
        animator.SetBool("Move", agent.speed != 0 ? true : false);
        //Attack();
    }

    // 移動関数
    public void StartMove()
    {
        move.Move(agent);
    }

    // 攻撃関数
    public void AttackEnemy()
    {
        Vector3 target = player.transform.position;
        // プレイヤーと離れすぎだったら攻撃不能
        if (mAI.IsNearTarget(target, 5.0f)) return;
        if (!EnemyController.singleton.Attack(gameObject, kind)) return;
        StopAll();
        StartCoroutine(attack.StartAttack(target, agent)); 
    }

    // 招集関数
    public void GatherCalled()
    {
        // 流されていれば return
        if (attack.GetFlow()) return;
        StopAll();
        Transform boss = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(gather.Gather(boss, agent));
    }

    public void Dead(float time = 0)
    {
        EnemyController.singleton.EraseEnemy(gameObject, kind);
        EnemyController.singleton.AttackEnd(gameObject, kind);
        EnemyController.singleton.AddDeathCount();
        Destroy(gameObject, time);
    }

    public virtual void OnWeaponHit(int damage, GameObject attackObject)
    {
        animator.SetTrigger("Dead");
        Dead(3);
    }

    public void OnPlayerDead()
    {
        StopAll();
        animator.SetTrigger("PlayerDefeat");
    }

    private void StopAll()
    {
        StopAllCoroutines();
        move.StopAllCoroutines();
        attack.StopAllCoroutines();
        gather.StopAllCoroutines();
    }
    #endregion
}
