using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField, Tooltip("AI")]
    MeleeAI mAI;

    [SerializeField]
    MeleeEnemy enemy;

    [SerializeField, Tooltip("武器")]
    private IWeapon weapon;

    [SerializeField, Tooltip("攻撃の始点")]
    private GameObject attackPoint;

    [SerializeField, Range(0, 10), Tooltip("攻撃の準備から実際に攻撃するまでの時間")]
    private float attackWaitTime;

    [SerializeField, Range(0, 10), Tooltip("受け流すが可能のタイミング")]
    private float flowTime;

    private bool flow = false;

    public IEnumerator StartAttack(Vector3 target, NavMeshAgent agent)
    {
        // 目標に接近
        while (!mAI.IsNearTarget(target, 2.0f))
        {
            mAI.MoveTowardsTarget(agent, target);
            yield return null;
        }
        // 止める
        agent.speed = 0;
        // ちょっと待つ
        yield return new WaitForSeconds(0.3f);
        // 攻撃アニメーション開始
        enemy.GetAnimator.SetTrigger("Attack");
        // プレイヤーに自分を送る
        if (enemy.playerObject.isPlayerAttacking())
            enemy.playerObject.SetTarget(this.gameObject);
        // 攻撃判定生成開始
        StartCoroutine(Attack(agent));
    }

    private IEnumerator Attack(NavMeshAgent agent)
    {
        // 攻撃開始、判定までの時間、受け流すfalse(流されていない)
        float time = 0;
        while (time < attackWaitTime)
        {
            Debug.Log(time);
            time += Time.deltaTime;
            // プレイヤーが受け流す中かつターゲットは自分
            if (enemy.playerObject.GetEnemyTarget() == gameObject &&
                enemy.playerObject.GetPlayerAttacking()&&
                time > flowTime)
            {
                //流すをtrue
                transform.rotation = Quaternion.Euler(transform.rotation.x, 170.0f, transform.rotation.z);
                flow = true;
                break;
            }
            yield return null;
        }
        if (flow == true)
        {
            Flow(agent);
            yield return null;
        }
        // 判定生成、攻撃終了通知、攻撃アニメーション終了、ターゲットを消す
        InstantiateWeapon();
        EnemyController.singleton.AttackEnd(gameObject, enemy.Kind);
        enemy.GetAnimator.SetTrigger("AttackEnd");
        enemy.playerObject.SetTarget(null);
    }

    private void InstantiateWeapon()
    {
        // 攻撃判定が武器のボーンについていけるように
        IWeapon hit;
        hit = Instantiate(weapon, attackPoint.transform.position, attackPoint.transform.rotation) as IWeapon;
        hit.transform.parent = attackPoint.transform;
    }

    private void Flow(NavMeshAgent agent)
    {
        // 全部の処理を終了
        StopAllCoroutines();
        agent.Stop();
        enemy.Dead(3);
        enemy.GetAnimator.SetTrigger("StartFlow");
        //transform.position = player.transform.position+transform.right;
    }

    public bool GetFlow()
    {
        return flow;
    }
}
