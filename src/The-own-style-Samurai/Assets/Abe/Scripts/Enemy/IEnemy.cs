// ----- ----- ----- ----- -----
//
// IEnemy
//
// 作成日：2016/04/19
// 作成者：阿部
//
// <概要>
// 敵のインターフェイスです
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Enemy/IEnemy")]
public abstract class IEnemy : MonoBehaviour, WeaponHitHandler, PlayerDeadHandler
{
    [SerializeField, Tooltip("攻撃の準備から実際に攻撃するまでの時間")]
    protected float attackWaitTime;

    [SerializeField, Tooltip("次に攻撃するまでの時間")]
    protected float attackCoolTime;

    bool isAttack;

    protected virtual  void OnStart() { }
    protected abstract void OnAttack();
    protected virtual  void OnAttackReadyStart() { }
    protected virtual  void OnAttackReadyUpdate(){ }
    protected abstract void _OnMove(); //Unity標準にOnMoveというイベントがあるため

    void Awake()
    {

    }

    void Start()
    {
        OnStart();
        StartCoroutine(OnUpdate());
    }

    IEnumerator OnUpdate()
    {
        while(true)
        {
            _OnMove();
            yield return null;
        }
    }

    protected void Attack()
    {
        //2重に攻撃のコルーチンを実行しないように
        if(isAttack == true) return;

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
        for(float time = 0; time <= attackWaitTime; time += Time.deltaTime)
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
        Destroy(gameObject);
    }

    protected virtual void PlayerDead()
    {
        
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
}
