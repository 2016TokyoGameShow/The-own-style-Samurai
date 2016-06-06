using UnityEngine;
using System.Collections;

public class MeleeMove : MonoBehaviour {
    [SerializeField]
    private MeleeAI m_AI;

    [SerializeField]
    private MeleeEnemy enemy;

    public IEnumerator Move(NavMeshAgent agent)
    {
        while (!m_AI.CanRayHitTarget(enemy.playerObject.transform.position, 6, "Player", Color.red))
        {
            m_AI.MoveTowardsTarget(agent, enemy.playerObject.transform.position);
            yield return null;
        }
    }

    //void StartRotate(Vector3 origin)
    //{
    //    if (m_AI.IsNearTarget(origin, 2.0f))
    //    {
    //        transform.position += -transform.forward / 50;
    //        return;
    //    }
    //    StopAllCoroutines();
    //    StartCoroutine(RotateAroundTarget(origin, m_AI.GetAngle(EnemyGenerator.singleton.Angle)));
    //}

    //// プレイヤーを囲む
    //IEnumerator RotateAroundTarget(Vector3 target, float angle)
    //{
    //    float rotate = m_AI.RotateDirection(m_AI.AngleFromTarget(target), angle);
    //    while (Mathf.Abs(m_AI.AngleFromTarget(target) - angle) > 10.0f)
    //    {
    //        // 他の敵とぶつからないように
    //        StepBack();
    //        // 減速しながら回転
    //        float rotateSpeed = rotate * (Mathf.Abs(Mathf.DeltaAngle(m_AI.AngleFromTarget(target), angle)) / 90.0f);
    //        transform.RotateAround(target, Vector3.up, rotateSpeed);
    //        yield return null;
    //    }
    //    //StartStandBy();
    //}

    //void StepBack()
    //{
    //    if (m_AI.IsRayHit(transform.right, 2, "Enemy", Color.green) ||
    //        m_AI.IsRayHit(-transform.right, 2, "Enemy", Color.green))
    //    {
    //        transform.position += -transform.forward / 30;
    //    }
    //}
}
