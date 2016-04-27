using UnityEngine;
using System.Collections;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : Enemy
{
    public enum EnemyState      //敵の状態
    {
        Attack,                 //攻撃
        StandBy,                //待機
        RotateAround            //プレイヤーを中心に回る
    }

    #region 変数

    [SerializeField, Tooltip("攻撃前の信号")]
    GameObject sign;

    [SerializeField, Tooltip("信号を出す位置")]
    GameObject aboveHead;

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    #endregion

    #region メソッド
    protected override void _OnMove()
    {
        if (IsRayHitPlayer(maxDistance))
        {
            agent.speed = 0;
            int choose = Random.Range(0,102);
            ActionChoose((EnemyState)(choose % 3));
        }
        else
        {
            agent.destination = player.transform.position;
            agent.speed = moveSpeed;
        }
    }

    protected override void OnAttackReadyStart()
    {
        Instantiate(sign, aboveHead.transform.position, transform.rotation);
    }

    protected override void OnAttack()
    {
        AttackInstantiate(weapon, attackPoint);
    }

    //判定生成メソット、生成判定、生成位置
    protected void AttackInstantiate(IWeapon hitObject, GameObject hitOffset)
    {
        IWeapon hit;

        hit = Instantiate(hitObject, hitOffset.transform.position, hitOffset.transform.rotation) as IWeapon;
        hit.transform.parent = hitOffset.transform;
    }

    private void ActionChoose(EnemyState state)
    {
        Debug.Log(state.ToString());
        StopAllCoroutines();
        switch (state)
        {
            case EnemyState.StandBy:
                StartCoolTime();
                break;
            case EnemyState.RotateAround:
                RotateAround();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }


    }

    private void RotateAround()
    {
        float time = 2;
        Vector3 rotateOrigin = player.transform.position;
        StopAllCoroutines();
        StartCoroutine(RotateAroundPlayer(rotateOrigin, time));
    }

    IEnumerator RotateAroundPlayer(Vector3 rotateOrigin, float time)
    {
        for (float rotateTime = 0; rotateTime <= time; rotateTime += Time.deltaTime)
        {
            transform.RotateAround(rotateOrigin, Vector3.up, 0.75f);
            yield return 0;
        }
        StartUpdate();
    }

    protected override void OnCollisionExit(Collision collision)
    {
    }

    #endregion
}
