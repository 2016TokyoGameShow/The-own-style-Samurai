using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Enemy/MeleeEnemy")]
public class MeleeEnemy : Enemy
{
    #region 変数

    [SerializeField, Tooltip("武器")]
    GameObject weapon;

    [SerializeField, Tooltip("攻撃判定を出す位置")]
    GameObject weaponHitOffset;

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

    [SerializeField, Tooltip("攻撃距離")]
    float maxDistance;

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
        //Instantiate(weapon, katana.transform.position, transform.rotation);
        AttackInstantiate(weapon, weaponHitOffset);
    }

    //判定生成メソット、生成判定、生成位置
    protected void AttackInstantiate(GameObject hitObject, GameObject hitOffset)
    {
        GameObject hit;

        hit = Instantiate(hitObject, hitOffset.transform.position, hitOffset.transform.rotation) as GameObject;
        hit.transform.parent = hitOffset.transform;
    }

    #endregion
}
