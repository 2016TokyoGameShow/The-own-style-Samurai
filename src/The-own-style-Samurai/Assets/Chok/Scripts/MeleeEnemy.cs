using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : Enemy
{
    public enum State
    {
        Chase,
        Attack,
        StandBy,
        RotateAround
    }

    #region 変数

    [SerializeField, Tooltip("攻撃前の信号")]
    GameObject sign;

    [SerializeField, Tooltip("信号を出す位置")]
    GameObject aboveHead;

    [SerializeField, Tooltip("Nav Mesh Agentのコンポーネント")]
    NavMeshAgent agent;

    private State state;

    #endregion

    #region メソッド
    protected override void _OnMove()
    {
        if (IsRayHitPlayer(maxDistance))
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
            transform.RotateAround(rotateOrigin, Vector3.up, 1.0f);
            yield return 0;
        }
        StartUpdate();
    }

    #endregion
}
