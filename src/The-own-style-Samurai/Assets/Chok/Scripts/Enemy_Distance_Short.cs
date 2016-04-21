using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Enemy/Enemy_Distance_Short")]
public class Enemy_Distance_Short : IEnemy
{
    #region 変数

    [SerializeField, Tooltip("武器")]
    GameObject weapon;

    [SerializeField, Tooltip("攻撃判定を出す位置")]
    GameObject katana;

    [SerializeField, Tooltip("プレイヤー")]
    GameObject player;

    [SerializeField, Tooltip("攻撃前の信号")]
    GameObject sign;

    [SerializeField, Tooltip("信号を出す位置")]
    GameObject aboveHead;

    [SerializeField, Tooltip("移動のスピード")]
    float moveSpeed;

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    #endregion

    #region メソッド
    protected override void _OnMove()
    {
        if (IsHitPlayer())
        {
            agent.speed = 0;
            Attack();
        }
        else
        {
            agent.destination = player.transform.position;
            agent.speed = moveSpeed;
        }
    }

    bool IsHitPlayer()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = transform.forward;


        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hitInfo, 2) == false) return false;
        if (hitInfo.collider.gameObject.tag != "Player") return false;
        return true;
    }

    protected override void OnAttackReadyStart()
    {
        Instantiate(sign, aboveHead.transform.position, transform.rotation);
    }

    protected override void OnAttack()
    {
        //Instantiate(weapon, katana.transform.position, transform.rotation);
        AttackInstantiate(weapon, katana);
    }

    //判定生成メソット、生成判定、生成位置
    protected void AttackInstantiate(GameObject hitObject, GameObject hitOffset)
    {
        GameObject hit;

        hit = Instantiate(hitObject, hitOffset.transform.position, hitOffset.transform.rotation) as GameObject;
        hit.transform.parent = hitOffset.transform;
    }

    #endregion

    //public enum EnemyState
    //{
    //    Move,
    //    RotateAround,
    //    Prepare,
    //    Attack
    //}


    //NavMeshAgent agent;
    //public Transform player;        //プレイヤー
    //public GameObject attack;       //攻撃判定（刀にくっつける）
    //public GameObject katana;       //刀のボーン
    //public GameObject warnSign;     //攻撃する前の合図
    //public GameObject head;         //合図をレンダリング位置

    //private EnemyState state;

    //// Use this for initialization
    //void Start()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //    agent.SetDestination(player.position);
    //    state = EnemyState.Move;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Debug1();
    //    Sign();
    //    Move();
    //    Attack();
    //    MoveAround();
    //}

    //private void Debug1()
    //{
    //    Debug.Log(agent.remainingDistance);
    //    Debug.Log(agent.stoppingDistance);
    //    Debug.Log(state.ToString());
    //}

    //private void Move()
    //{
    //    if (state != EnemyState.Move) return;
    //    //プレイヤーに向くようにする
    //    //キャラクターコントローラを使うなら消してもいい
    //    float angle = Mathf.Atan2(player.position.z - transform.position.z, player.position.x - transform.position.x);
    //    angle *= 180.0f / Mathf.PI;
    //    Quaternion.Euler(0, angle, 0);

    //    //プレイヤーに向かって移動
    //    agent.SetDestination(player.position);
    //    //プレイヤーのまわりに止まって、攻撃するか円周運動するか判断する
    //    if (agent.remainingDistance <= agent.stoppingDistance)
    //    {
    //        //攻撃をするキャラクターの選び方はまだきまってないから
    //        //接近したら直接合図を出して、攻撃をする
    //        state = EnemyState.Prepare;
    //    }
    //}

    //private void RotateTowardPlayer()
    //{
    //    //プレイヤーとの距離から角度を計算して
    //    float dirX = player.position.x - transform.position.x;
    //    float dirZ = player.position.z - transform.position.z;
    //    float direction = Mathf.Atan2(dirX, dirZ);
    //    //degree変換
    //    direction *= 180.0f / Mathf.PI;
    //    //プレイヤーに向くように
    //    Quaternion.Euler(0, direction, 0);
    //}

    //private IEnumerator Attack()
    //{
    //    //攻撃まで何秒待つ
    //    yield return new WaitForSeconds(3);
    //    //攻撃をする
    //    AttackInstantiate(attack, gameObject);
    //    state = EnemyState.Move;
    //}


    //private void Sign()
    //{
    //    if (state != EnemyState.Prepare) return;
    //    //攻撃の合図を出す
    //    AttackInstantiate(warnSign, head);
    //    //攻撃へのカウントダウン
    //    StartCoroutine(Attack());
    //    state = EnemyState.Attack;
    //}

    //private void MoveAround()
    //{
    //    if (state != EnemyState.RotateAround) return;
    //    //state = EnemyState.Move;
    //    agent.Stop();
    //    //transform.RotateAround(player.position, Vector3.up, 1.0f);
    //}
}
