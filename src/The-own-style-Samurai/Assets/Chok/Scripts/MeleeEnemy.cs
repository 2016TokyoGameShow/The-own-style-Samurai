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

    [SerializeField, Tooltip("アニメーションのコンポーネント")]
    Animator animator;

    #endregion

    #region メソッド
    protected override void _OnMove()
    {
        if (IsRayHitPlayer(maxDistance))
        {
            agent.speed = 0;
            animator.SetFloat("Speed", agent.speed);
            ActionStart();
            return;
        }
        agent.destination = player.transform.position;
        agent.speed = moveSpeed;
        animator.SetFloat("Speed", agent.speed);
    }

    protected override void OnAttackReadyStart()
    {
        animator.SetTrigger("Attack");
        Instantiate(sign, aboveHead.transform.position, transform.rotation);
        if(player.isPlayerAttacking())player.SetTarget(this.gameObject);
    }

    protected override void OnAttackReadyUpdate()
    {

    }

    protected override void OnAttack()
    {
        AttackInstantiate(weapon, attackPoint);
        animator.SetTrigger("AttackEnd");
        if (!player.isPlayerAttacking()) player.SetTarget(null);
    }

    //判定生成メソット、生成判定、生成位置
    protected void AttackInstantiate(IWeapon hitObject, GameObject hitOffset)
    {
        GameObject hit;

        hit = CreateWeapon(weapon,attackPoint.transform.position,attackPoint.transform.rotation) as GameObject;
        hit.transform.parent = hitOffset.transform;
    }

    private void ActionStart()
    {
        StopAllCoroutines();
        StartCoroutine(StartChoose());
    }

    private IEnumerator StartChoose()
    {
        yield return new WaitForSeconds(0.5f);
        int choose = Random.Range(0, 102);
        ActionChoose((EnemyState)(choose % 3));
    }

    private void ActionChoose(EnemyState state)
    {
        Debug.Log(state.ToString());
        switch (state)
        {
            case EnemyState.StandBy:
                StartStandBy();
                break;
            case EnemyState.RotateAround:
                StartRotate();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void StartStandBy()
    {
        StartCoroutine(StandBy());
    }

    private IEnumerator StandBy()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));
        Attack();
    }

    private void StartRotate()
    {
        float time = Random.Range(1, 3);                     //回転周期を決める
        float angle = Random.Range(-2, 3);                  //回転角度を決める
        if (angle == 0) angle = 1;                          //止めさせない
        Vector3 rotateOrigin = player.transform.position;   //最初に回転中心を決める
        StopAllCoroutines();
        StartCoroutine(RotateAroundPlayer(rotateOrigin, time, angle));
    }

    IEnumerator RotateAroundPlayer(Vector3 rotateOrigin, float time, float angle)
    {
        for (float rotateTime = 0; rotateTime <= time; rotateTime += Time.deltaTime)
        {
            transform.RotateAround(rotateOrigin, Vector3.up, angle);
            yield return 0;
        }
        Attack();
    }

    protected override void OnCollisionExit(Collision collision)
    {
    }

    #endregion
}
