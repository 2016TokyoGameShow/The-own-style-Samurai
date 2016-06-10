using UnityEngine;
using System.Collections;

public class MeleeMove : MonoBehaviour
{
    [SerializeField]
    private MeleeAI mAI;

    [SerializeField]
    private MeleeEnemy enemy;

    private float angle = 1;

    public IEnumerator Move(NavMeshAgent agent)
    {
        // プレイヤーが見えるまで移動、攻撃不能
        while (!mAI.CanRayHitTarget(
            enemy.playerObject.transform.position,
            6, "Player", Color.red))
        {
            mAI.MoveTowardsTarget(agent, enemy.playerObject.transform.position);
            yield return null;
        }
        agent.speed = 0;
        StartCoroutine(RotateAroundPlayer());
    }

    IEnumerator RotateAroundPlayer()
    {
        bool lostPlayer = false;
        if (angle == 1) angle = mAI.GetAngle(MeleeAIController.singleton.Angle);
        Vector3 target = enemy.playerObject.transform.position;
        float rotate = mAI.RotateDirection(mAI.AngleFromTarget(target), angle);
        while (Mathf.Abs(mAI.AngleFromTarget(target) - angle) > 10.0f)
        {
            if(!mAI.CanRayHitTarget(enemy.playerObject.transform.position,
                6, "Player", Color.red))
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
        if(lostPlayer== true)
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
                8, "Player", Color.grey))
        {
            yield return null;
        }
        enemy.StartMove();
    }
}
