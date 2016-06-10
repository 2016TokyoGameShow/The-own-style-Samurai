using UnityEngine;
using System.Collections;

public class MeleeMove : MonoBehaviour
{
    [SerializeField]
    private MeleeAI mAI;

    [SerializeField]
    private MeleeEnemy enemy;

    private float angle = 0;

    public IEnumerator Move(NavMeshAgent agent)
    {
        // プレイヤーと近すぎだったら後退
        while (mAI.IsNearTarget(enemy.playerObject.transform.position, 5.0f))
        {
            agent.speed = 0.01f;
            transform.position -= transform.forward / 20;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.5f);

        // プレイヤーが見えるまで移動、攻撃不能
        while (!mAI.CanRayHitTarget(
            enemy.playerObject.transform.position,
            6, "Player", Color.red))
        {
            mAI.MoveTowardsTarget(agent, enemy.playerObject.transform.position);
            yield return new WaitForEndOfFrame();
        }
        agent.speed = 0;
        enemy.SetIsAttackable(true);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RotateAroundPlayer());
    }

    IEnumerator RotateAroundPlayer()
    {
        // プレイヤーを中心に回転、もしプレイヤーを見失ったらループに戻る
        bool lostPlayer = false;
        if (angle == 0) angle = mAI.GetAngle(MeleeAIController.singleton.Angle);
        Vector3 target = enemy.playerObject.transform.position;
        float rotate = mAI.RotateDirection(mAI.AngleFromTarget(target), angle);
        while (Mathf.Abs(mAI.AngleFromTarget(target) - angle) > 10.0f)
        {
            if (!mAI.CanRayHitTarget(enemy.playerObject.transform.position,
                8, "Player", Color.red))
            {
                lostPlayer = true;
                break;
            }
            // 減速しながら回転
            float rotateSpeed = rotate * (Mathf.Abs(Mathf.DeltaAngle(mAI.AngleFromTarget(target), angle)) / 90.0f);
            transform.RotateAround(target, Vector3.up, rotateSpeed);
            enemy.GetAnimator.SetFloat("Rotate", rotateSpeed);
            yield return null;
        }
        enemy.GetAnimator.SetFloat("Rotate", 0);
        if (lostPlayer == true)
        {
            StartCoroutine(enemy.CoolTime(2));
        }
        else
        {
            StartCoroutine(WaitForAttack());
        }
    }

    IEnumerator WaitForAttack()
    {
        while (mAI.CanRayHitTarget(
                enemy.playerObject.transform.position,
                5, "Player", Color.grey))
        {
            yield return null;
        }
        enemy.CoolTime(2);
    }
}
