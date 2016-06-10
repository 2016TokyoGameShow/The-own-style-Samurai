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

    [SerializeField, Range(0, 3), Tooltip("攻撃の準備から実際に攻撃するまでの時間")]
    private float attackWaitTime;

    [SerializeField, Range(0, 3), Tooltip("受け流すが可能のタイミング　開始")]
    private float flowStart;

    [SerializeField, Range(0, 3), Tooltip("受け流すが可能のタイミング　終了")]
    private float flowEnd;

    [SerializeField, Range(0, 3), Tooltip("攻撃距離")]
    private float stopDistance;

    public bool flow;

    public IEnumerator Attack(NavMeshAgent agent)
    {
        // 目標に接近
        while (!mAI.IsNearTarget(
            enemy.playerObject.transform.position,
            stopDistance))
        {
            mAI.MoveTowardsTarget(agent, enemy.playerObject.transform.position, 1.5f);
            yield return new WaitForEndOfFrame();
        }
        agent.speed = 0;                                // 止める
        yield return new WaitForSeconds(0.3f);          // ちょっと待つ
        enemy.GetAnimator.SetTrigger("Attack");         // 攻撃アニメーション開始
        
        // プレイヤーに自分を送る
        if (enemy.playerObject.isPlayerAttacking())
            enemy.playerObject.SetTarget(this.gameObject);

        // 攻撃開始
        float time = 0;
        flow = false;
        while (time < attackWaitTime)
        {
            time += Time.deltaTime;
            // プレイヤーが受け流す中かつターゲットは自分
            if (enemy.playerObject.GetEnemyTarget() == gameObject &&
                enemy.playerObject.GetPlayerAttacking() &&
                (time > flowStart && time < flowEnd))
            {
                //流すをtrue
                flow = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        if (flow == true)
        {
            Flow();
        }
        else
        {
            // 判定生成、攻撃終了通知、攻撃アニメーション終了、ターゲットを消す
            InstantiateWeapon();
            EnemyController.singleton.AttackEnd(gameObject, enemy.Kind);
            enemy.GetAnimator.SetTrigger("AttackEnd");
            enemy.playerObject.SetTarget(null);
            StartCoroutine(enemy.CoolTime(2));
        }
    }

    private void InstantiateWeapon()
    {
        // 攻撃判定が武器のボーンについていけるように
        IWeapon hit;
        hit = Instantiate(weapon, attackPoint.transform.position, attackPoint.transform.rotation) as IWeapon;
        hit.transform.parent = attackPoint.transform;
    }

    private void Flow()
    {
        // 受け流すアニメーション
        transform.rotation = Quaternion.Euler(transform.rotation.x, -10.0f, transform.rotation.z);
        enemy.GetAnimator.SetTrigger("StartFlow");
        // 全部の処理を終了
        enemy.StopAll();
        enemy.Dead(3);
    }
}