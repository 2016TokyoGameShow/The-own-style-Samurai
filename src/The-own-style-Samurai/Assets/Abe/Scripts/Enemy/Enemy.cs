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

[SelectionBase]
[AddComponentMenu("Enemy/Enemy")]
public abstract class Enemy : MonoBehaviour, WeaponHitHandler, PlayerDeadHandler
{
    [SerializeField, Tooltip("敵の種類")]
    protected EnemyController.EnemyKind kind;

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

    public Player playerObject
    {
        get { return player; }
    }

    public EnemyController.EnemyKind Kind
    {
        get { return kind; }
    }

    protected virtual  void OnStart() { }
    protected abstract void OnAttack();
    protected virtual  void OnAttackReadyStart() { }
    protected virtual  void OnAttackReadyUpdate() { }
    protected virtual  void OnAttackCancel() { }
    protected abstract void _OnMove(); //Unity標準にOnMoveというイベントがあるため
    protected virtual  void _OnMoveEnd() { }

    void Awake()
    {
        
    }

    void Start()
    {
        Debug.Assert(EnemyController.singleton != null, "EnemyControllerがありません");

        player = EnemyController.singleton.player;
        EnemyController.singleton.AddEnemy(gameObject, kind);

        OnStart();
        StartCoroutine(OnUpdate());
    }

    IEnumerator OnUpdate()
    {
        yield return null;
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

	public virtual void AttackEnemy()
    {
        //2重に攻撃のコルーチンを実行しないように
        if(isAttack)
        {
            //攻撃失敗
            return;
        }

        //maxより少ないか
        Debug.Assert(EnemyController.singleton.Attack(gameObject, kind));

        isAttack = true;
        _OnMoveEnd();

        StopAllCoroutines();
        StartCoroutine(AttackReady());
    }

    protected bool Attack()
    {
        //2重に攻撃のコルーチンを実行しないように
        if (isAttack == true)
        {
            //攻撃失敗
            return false;
        }

        //攻撃している人数が少なければ攻撃可能
        if(!EnemyController.singleton.Attack(gameObject, kind))
        {
            //攻撃失敗
            return false;
        }

        isAttack = true;

        _OnMoveEnd();

        //攻撃準備中に敵を動かす場合はOnAttackReadyUpdateを使う
        StopAllCoroutines();
        StartCoroutine(AttackReady());

        //攻撃成功
        return true;
    }

    protected void AttackCancel()
    {
        //攻撃をキャンセル
        OnAttackCancel();
        isAttack = false;
        EnemyController.singleton.AttackEnd(gameObject, kind);
        EnemyController.singleton.EraseAttackCount();
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
        Debug.Log(gameObject.name);
        OnAttack();
        EnemyController.singleton.EraseAttackCount();
        EnemyController.singleton.AttackEnd(gameObject, kind);

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
        EnemyController.singleton.EraseEnemy(gameObject, kind);
        EnemyController.singleton.AttackEnd(gameObject, kind);
        EnemyController.singleton.AddDeathCount();
        Destroy(gameObject);
    }

    protected virtual void Dead(float time)
    {
        //敵を吹っ飛ばしてから消去のほうがいいか
        EnemyController.singleton.EraseEnemy(gameObject, kind);
        EnemyController.singleton.AttackEnd(gameObject, kind);
        EnemyController.singleton.AddDeathCount();
        Destroy(gameObject, time);
    }

    public virtual void OnWeaponHit(int damage, GameObject attackObject)
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

    protected void ChasePlayer(NavMeshAgent agent, float moveSpeed)
    {
        agent.destination = player.transform.position;
        agent.speed       = moveSpeed;
    }

    delegate bool RayHit(Ray ray, out RaycastHit hitInfo);

    protected bool IsRayHitPlayer(float maxDistance)
    {
        return _IsRayHitPlayer((Ray ray, out RaycastHit hitInfo) =>
        {
            return Physics.Raycast(ray, out hitInfo, maxDistance);
        });
    }

#if UNITY_EDITOR
    protected new Object Instantiate(Object original, Vector3 position, Quaternion rotation)
    {
        //IWeaponをInstantiateで生成させないように

        bool isWeapon = original is IWeapon;
        bool isAttachWeapon;
        try
        {
            isAttachWeapon = ((Component)original).GetComponent<IWeapon>() != null;
        }
        catch
        {
            //コンポーネントではない -> IWeaponではない
            isAttachWeapon = false;
        }
        Debug.Assert(!(isWeapon) && !(isAttachWeapon), "武器を生成する場合はCreateWeaponを使用してください");

        return Object.Instantiate(original, position, rotation);
    }
#endif

    protected GameObject CreateWeapon(IWeapon weapon, Vector3 position, Quaternion rotation)
    {
        weapon.attackEnemy = gameObject;
        return Object.Instantiate(weapon.gameObject, position, rotation) as GameObject;
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
            return Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
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
}
