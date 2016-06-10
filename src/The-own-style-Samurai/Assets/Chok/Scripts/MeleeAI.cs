using UnityEngine;
using System.Collections;

public enum MeleeState
{
    NORMAL,
    ROTATE,
    ATTACKREADY,
    ATTACKING,
    COOLDOWN
}

public class MeleeAI : MonoBehaviour
{
    [SerializeField, Tooltip("見る角度")]
    float m_ViewingAngle;

    [SerializeField]
    float[] targetAngle;

    private float speed;

    // Use this for initialization
    void Start()
    {
        speed = Random.Range(2, 4);     // 移動スピードをランダム
    }

    //移動する角度を取得
    public float GetAngle(int num)
    {
        return targetAngle[num];
    }

    //目標まで移動
    public void MoveTowardsTarget(NavMeshAgent agent, Vector3 target, float multiply = 1)
    {
        agent.destination = target;
        agent.speed = speed * multiply;
    }

    //目標が視角内か？
    public bool IsTargetInViewingAngle(Vector3 target)
    {
        float angleToTarget = Vector3.Angle(transform.forward, DirectionToTarget(target));
        return (Mathf.Abs(angleToTarget) <= m_ViewingAngle);
    }

    //レイが目標やに当たったか？
    public bool IsRayHit(Vector3 direction, float maxDistance, string tag, Color color)
    {
        direction.Normalize();
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hitInfo;
        Debug.DrawRay(transform.position, direction * maxDistance, color);

        bool hit = Physics.Raycast(ray, out hitInfo, maxDistance);
        return (hit && hitInfo.collider.tag == tag);
    }

    // レイが目標やに当たったか？ プラス 目標が視角内か？
    public bool CanRayHitTarget(Vector3 target, float maxDistance, string tag, Color color)
    {
        if (!IsTargetInViewingAngle(target)) return false;
        if (!IsRayHit(DirectionToTarget(target), maxDistance, tag, color)) return false;
        return true;
    }

    public bool IsNearTarget(Vector3 target, float distance)
    {
        return (Vector3.Distance(transform.position, target) <= distance);
    }

    //目標からの角度
    public float AngleFromTarget(Vector3 target)
    {
        float directionX = target.x - transform.position.x;
        float directionZ = target.z - transform.position.z;
        float angle = Mathf.Atan2(directionZ, directionX);
        angle = angle * 180.0f / Mathf.PI;
        if (angle < 0) angle += 360.0f;
        //Debug.Log(angle);
        return angle;
    }

    //左か右か回転方向選択
    public float RotateDirection(float current, float target)
    {
        float dir = current - target > 0 || current - target < -180.0f ?
            dir = 1.0f :
            dir = -1.0f;
        return dir;
    }

    public Vector3 DirectionToTarget(Vector3 target)
    {
        return target - transform.position;
    }

    public void RotateAroundTarget(Vector3 target, float angle, float rotate)
    {
        while (Mathf.Abs(AngleFromTarget(target) - angle) > 10.0f)
        {
            // 減速しながら回転
            float rotateSpeed = rotate * (Mathf.Abs(Mathf.DeltaAngle(AngleFromTarget(target), angle)) / 90.0f);
            transform.RotateAround(target, Vector3.up, rotateSpeed);
        }
    }
}
