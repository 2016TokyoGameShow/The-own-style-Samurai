using UnityEngine;
using System.Collections;

public class MeleeMove : MonoBehaviour
{
    [SerializeField]
    private MeleeAI mAI;

    [SerializeField]
    private MeleeEnemy enemy;

    private float angle;
    private bool attackable = false;

    void Start()
    {
        angle = mAI.GetAngle(EnemyGenerator.singleton.Angle);
    }

    public IEnumerator Move(NavMeshAgent agent)
    {
        // プレイヤーが見えるまで移動、攻撃不能
        attackable = false;
        while (!mAI.CanRayHitTarget(
            enemy.playerObject.transform.position,
            6, "Player", Color.red))
        {
            if (IsRaysHit(transform.forward, 4, "Enemy", Color.black))
            {
                transform.position += -transform.right / 20;
            }
            mAI.MoveTowardsTarget(agent, enemy.playerObject.transform.position);
            yield return null;
        }
        agent.speed = 0;
        StartCoroutine(RotateLeft());
    }

    IEnumerator RotateAroundPlayer()
    {
        Vector3 target = enemy.playerObject.transform.position;
        float rotate = mAI.RotateDirection(mAI.AngleFromTarget(target), angle);
        while (Mathf.Abs(mAI.AngleFromTarget(target) - angle) > 10.0f)
        {
            // 減速しながら回転
            float rotateSpeed = rotate * (Mathf.Abs(Mathf.DeltaAngle(mAI.AngleFromTarget(target), angle)) / 90.0f);
            transform.RotateAround(target, Vector3.up, rotateSpeed);
            yield return null;
        }
    }

    IEnumerator RotateLeft()
    {
        while (IsRaysHit(transform.right, 5, "Enemy", Color.black))
        {
            transform.RotateAround(
                enemy.playerObject.transform.position,
                Vector3.up, 1.0f);
            yield return null;
        }
        StartCoroutine(RotateRight());
    }

    IEnumerator RotateRight()
    {
        while (IsRaysHit(-transform.right, 5, "Enemy", Color.black))
        {
            transform.RotateAround(
                enemy.playerObject.transform.position,
                Vector3.up, -1.0f);
            yield return null;
        }
        StartCoroutine(WaitForAttack());
    }

    IEnumerator WaitForAttack()
    {
        attackable = true;
        while (!mAI.CanRayHitTarget(
                enemy.playerObject.transform.position,
                3, "Player", Color.grey))
        {
            yield return null;
        }
        enemy.StartMove();
    }

    public bool IsAttackable()
    {
        return attackable;
    }

    private bool IsRaysHit(Vector3 direction, float maxDistance, string tag, Color color)
    {
        for (float i = -90; i < 90; i += 0.1f)
        {
            if (mAI.IsRayHit(Quaternion.Euler(0, i - direction.y, 0) * direction, maxDistance, tag, color))
            {
                return true;
            }
        }

        return false;
    }
}
