using UnityEngine;
using System.Collections;

public class MeleeGather : MonoBehaviour {
    [SerializeField]
    private MeleeAI mAI;

    public IEnumerator Gather(Transform boss,NavMeshAgent agent)
    {
        // 目標まで移動
        while(!mAI.IsRayHit(transform.forward, 3, "Player", Color.yellow))
        {
            mAI.MoveTowardsTarget(agent, boss.position);
            yield return null;
        }
        // 止める
        agent.speed = 0;
        // 回転角度取得、回転開始
        StartCoroutine(Rotate(boss.position, mAI.GetAngle(MeleeAIController.singleton.Angle)));
    }

    private IEnumerator Rotate(Vector3 boss,float angle)
    {
        float rotate = mAI.RotateDirection(mAI.AngleFromTarget(boss), angle);
        while (Mathf.Abs(mAI.AngleFromTarget(boss) - angle) > 10.0f)
        {
            // 減速しながら回転
            float rotateSpeed = rotate * (Mathf.Abs(Mathf.DeltaAngle(mAI.AngleFromTarget(boss), angle)) / 90.0f);
            transform.RotateAround(boss, Vector3.up, rotateSpeed);
            yield return null;
        }
        StartCoroutine(StandBy());
    }

    private IEnumerator StandBy()
    {
        while (true)
        {
            yield return null;
        }
    }
}
