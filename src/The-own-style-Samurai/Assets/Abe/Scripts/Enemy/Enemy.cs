// ----- ----- ----- ----- -----
//
// Enemy
//
// 作成日：2016/04/19
// 作成者：阿部
//
// <概要>
// 
//
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[SelectionBase]
[AddComponentMenu("Enemy/Enemy")]
public abstract class Enemy : MonoBehaviour, WeaponHitHandler, PlayerDeadHandler
{
    [SerializeField, Range(0,  10), Tooltip("攻撃の準備から実際に攻撃するまでの時間")]
    protected float attackWaitTime;

    [SerializeField, Range(0,  30), Tooltip("次に攻撃するまでの時間")]
    protected float attackCoolTime;

    [SerializeField, Range(0, 100), Tooltip("敵の攻撃範囲(プレイヤーを検知する範囲)")]
    protected float maxDistance;

    [SerializeField, Range(0,   3), Tooltip("移動のスピード")]
    protected float moveSpeed;

    [SerializeField, Tooltip("武器")]
    protected IWeapon weapon;

    [SerializeField, Tooltip("攻撃の始点")]
    protected GameObject attackPoint;

    protected Player player;

    bool isAttack;
    Rigidbody rig;

    protected virtual void OnStart() { }
    protected abstract void OnAttack();
    protected virtual void OnAttackReadyStart() { }
    protected virtual void OnAttackReadyUpdate() { }
    protected abstract void _OnMove(); //Unity標準にOnMoveというイベントがあるため

    void Awake()
    {
        
    }

    void Start()
    {
        Debug.Assert(EnemyController.singleton != null, "EnemyControllerがありません");

        player = EnemyController.singleton.player;
        EnemyController.singleton.AddEnemy(gameObject);
        rig = GetComponent<Rigidbody>();

        OnStart();
        StartCoroutine(OnUpdate());
    }

    IEnumerator OnUpdate()
    {
        while (true)
        {
            _OnMove();
            yield return null;
        }
    }

    protected void StartUpdate()
    {
        StopAllCoroutines();
        StartCoroutine(OnUpdate());
    }

    protected void Attack()
    {
        //2重に攻撃のコルーチンを実行しないように
        if (isAttack == true)
            return;

        isAttack = true;

        //攻撃準備中に敵を動かす場合はOnAttackReadyUpdateを使う
        StopAllCoroutines();
        StartCoroutine(AttackReady());
    }

    protected void AttackCancel()
    {
        //攻撃をキャンセル
        isAttack = false;
        StartUpdate();
    }

    IEnumerator AttackReady()
    {
        EnemyController.singleton.AddAttackCount();
        OnAttackReadyStart();
        for (float time = 0; time <= attackWaitTime; time += Time.deltaTime)
        {
            //準備期間の間毎回実行される
            OnAttackReadyUpdate();
            yield return null;
        }
        OnAttack();
        EnemyController.singleton.EraseAttackCount();
        //クールタイムが終わるまで移動も攻撃ができない
        StartCoolTime();
    }

    IEnumerator CoolTime()
    {
        //クールタイム開始
        yield return new WaitForSeconds(attackCoolTime);

        //クールタイム終了
        //攻撃準備完了
        isAttack = false;
        StartUpdate();
    }

    protected void StartCoolTime()
    {
        StopAllCoroutines();
        StartCoroutine(CoolTime());
    }

    protected virtual void Dead()
    {
        //敵を吹っ飛ばしてから消去のほうがいいか
        EnemyController.singleton.EraseEnemy(gameObject);
        EnemyController.singleton.AddDeathCount();
        Destroy(gameObject);
    }

    public virtual void OnWeaponHit(int damage)
    {
        Dead();
    }

    public void OnPlayerDead()
    {
        StopAllCoroutines();
        PlayerDead();
    }

    protected virtual void PlayerDead()
    {

    }

    delegate bool RayHit(Ray ray, out RaycastHit hitInfo);

    protected bool IsRayHitPlayer(float maxDistance)
    {
        return _IsRayHitPlayer((Ray ray, out RaycastHit hitInfo) =>
        {
            return Physics.Raycast(ray, out hitInfo, maxDistance);
        });
    }

    protected bool IsRayHitPlayer(float maxDistance, int layerMask)
    {
        return _IsRayHitPlayer((Ray ray, out RaycastHit hitInfo) => 
        {
            return Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
        });
    }

    protected bool IsRayHitPlayer(float maxDistance, Vector3 offset)
    {
        return _IsRayHitPlayer((Ray ray, out RaycastHit hitInfo) =>
        {
            ray.origin += offset;
            return Physics.Raycast(ray, out hitInfo, maxDistance);
        });
    }

    protected bool IsRayHitPlayer(float maxDistance, int layerMask, Vector3 offset)
    {
        return _IsRayHitPlayer((Ray ray, out RaycastHit hitInfo) =>
        {
            ray.origin += offset;
            return Physics.Raycast(ray, out hitInfo, maxDistance);
        });
    }

    private bool _IsRayHitPlayer(RayHit rayCast)
    {
        Ray ray = new Ray();
        ray.origin    = transform.position;
        ray.direction = transform.forward;

        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        if (!rayCast(ray, out hitInfo))                  return false;
        if (hitInfo.collider.gameObject.tag != "Player") return false;
        return true;
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        //滑っていかないように
        rig.velocity = Vector3.zero;
    }
}
