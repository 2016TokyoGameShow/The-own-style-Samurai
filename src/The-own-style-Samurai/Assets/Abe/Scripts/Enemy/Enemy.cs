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
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Enemy/Enemy")]
public abstract class Enemy : MonoBehaviour, WeaponHitHandler, PlayerDeadHandler
{
    [SerializeField, Tooltip("攻撃の準備から実際に攻撃するまでの時間")]
    protected float attackWaitTime;

    [SerializeField, Tooltip("次に攻撃するまでの時間")]
    protected float attackCoolTime;

    [SerializeField, Tooltip("敵の攻撃範囲(プレイヤーを検知する範囲)")]
    protected float maxDistance;

    bool isAttack;

    Rigidbody rig;

    protected virtual void OnStart() { }
    protected abstract void OnAttack();
    protected virtual void OnAttackReadyStart() { }
    protected virtual void OnAttackReadyUpdate() { }
    protected abstract void _OnMove(); //Unity標準にOnMoveというイベントがあるため

    void Awake()
    {
        EnemyController.singleton.AddEnemy(gameObject);
        rig = GetComponent<Rigidbody>();
    }

    void Start()
    {
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
        StopAllCoroutines();
        StartCoroutine(OnUpdate());
    }

    IEnumerator AttackReady()
    {
        OnAttackReadyStart();
        for (float time = 0; time <= attackWaitTime; time += Time.deltaTime)
        {
            //準備期間の間毎回実行される
            OnAttackReadyUpdate();
            yield return null;
        }
        OnAttack();

        //攻撃したあとにすぐに動けるようにする
        //ただしクールタイムが終わるまで次の攻撃ができない
        StartCoroutine(OnUpdate());
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        //クールタイム開始
        yield return new WaitForSeconds(attackCoolTime);

        //クールタイム終了
        //攻撃準備完了
        isAttack = false;
    }

    protected virtual void Dead()
    {
        //敵を吹っ飛ばしてから消去のほうがいいか
        EnemyController.singleton.EraseEnemy(gameObject);
        Destroy(gameObject);
    }

    public void OnWeaponHit()
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

    protected bool IsRayHitPlayer(float maxDistance)
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = transform.forward;

        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, transform.forward * maxDistance);

        if (Physics.Raycast(ray, out hitInfo, maxDistance) == false)
            return false;
        if (hitInfo.collider.gameObject.tag != "Player")
            return false;
        return true;
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        rig.velocity = Vector3.zero;
    }
}
