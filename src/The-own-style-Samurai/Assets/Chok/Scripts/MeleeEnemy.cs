﻿using UnityEngine;
using System.Collections;

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

    //[SerializeField]
    //private MeleeGather gather;

    [SerializeField]
    private MeleeAttack attack;

    private Player player;
    private MeleeState state;
    private bool attackable = true;
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
        StartMove();
    }

    public void Update()
    {
        animator.SetBool("Move", agent.speed != 0 ? true : false);
    }

    // 移動関数
    public void StartMove()
    {
        StopAll();
        StartCoroutine(move.Move(agent));
    }

    // 攻撃関数
    public void AttackEnemy()
    {
        if (attackable == false) return;
        Vector3 target = player.transform.position;
        // プレイヤーと離れすぎだったら攻撃不能
        if (!mAI.IsNearTarget(target, 8.0f)) return;
        if (!EnemyController.singleton.Attack(gameObject, kind)) return;
        attackable = false;
        StopAll();
        StartCoroutine(attack.Attack(agent));
    }

    // 招集関数
    public void GatherCalled()
    {
        // 流されていれば return
        //if (attack.flow == true) return;
        //StopAll();
        //Transform boss = GameObject.FindGameObjectWithTag("Player").transform;
        //StartCoroutine(gather.Gather(boss, agent));
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
        //animator.SetTrigger("PlayerDefeat");
    }

    // 全部のループを止める
    public void StopAll()
    {
        StopAllCoroutines();
        move.StopAllCoroutines();
        attack.StopAllCoroutines();
        agent.speed = 0;
    }

    public IEnumerator CoolTime(float time)
    {
        yield return new WaitForSeconds(time);
        StartMove();
    }

    public void SetIsAttackable(bool set)
    {
        attackable = set;
    }
    #endregion
}
