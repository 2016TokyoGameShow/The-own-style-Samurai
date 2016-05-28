﻿using UnityEngine;
using System.Collections;

public class MeleeAI : MonoBehaviour
{
    [SerializeField]
    MeleeEnemy enemy;

    [SerializeField, Tooltip("見る角度")]
    float m_Angle;

    [SerializeField]
    float[] targetAngle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //移動する角度を取得
    public float GetAngle(int num)
    {
        return targetAngle[num];
    }

    //目標まで移動
    public void MoveTowardsTarget(NavMeshAgent agent,Vector3 target,float moveSpeed,Animator animator)
    {
        agent.destination = target;
        agent.speed = moveSpeed;
        animator.SetFloat("Speed", agent.speed);
    }

    public bool IsRayHitEnemy(Vector3 position, Vector3 direction)
    {
        Ray ray = new Ray(position, direction);
        RaycastHit hitInfo;
        Debug.DrawRay(position, direction * 2);

        bool hit = Physics.Raycast(ray, out hitInfo, 2);
        return (hit && hitInfo.collider.tag == "Enemy");
    }

    //プレイヤーが視角内か？
    public bool IsPlayerInViewingAngle(Vector3 player)
    {
        Vector3 directionToPlayer = player - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        return (Mathf.Abs(angleToPlayer) <= m_Angle);
    }

    //レイがプレイやに当たったか？
    public bool IsRayHitPlayer(Vector3 player, float maxDistance)
    {
        Vector3 directionToPlayer = player - transform.position;
        directionToPlayer.Normalize();
        Debug.DrawRay(transform.position, directionToPlayer * maxDistance, Color.green);
        return IsRayHit(transform.position, directionToPlayer, maxDistance, "Player");
    }

    public bool CanRayHitPlayer(Vector3 player,float maxDistance)
    {
        if (!IsPlayerInViewingAngle(player)) return false;
        if (!IsRayHitPlayer(player, maxDistance)) return false;
        return true;
    }

    public bool IsNearPlayer(Vector3 position, Vector3 player, float distance)
    {
        return (Vector3.Distance(position, player) <= distance);
    }

    private bool IsRayHit(Vector3 position, Vector3 direction, float maxDistance, string tag)
    {
        Ray ray = new Ray(position, direction);
        RaycastHit hitInfo;
        Debug.DrawRay(position, direction * maxDistance);

        bool hit = Physics.Raycast(ray, out hitInfo, maxDistance);
        return (hit && hitInfo.collider.tag == tag);
    }

    //目標からの角度
    public float AngleFromTarget(Vector3 target)
    {
        float directionX = target.x - transform.position.x;
        float directionZ = target.z - transform.position.z;
        float angle = Mathf.Atan2(directionZ, directionX);
        angle = angle * 180.0f / Mathf.PI;
        if (angle < 0) angle += 360.0f;
        Debug.Log(angle);
        return angle;
    }

    //左か右か回転方向選択
    public float RotateLeftOrRight(float selfAngle,float targetAngle)
    {
        if (selfAngle - targetAngle > 0 ||
            selfAngle - targetAngle < -180.0f)
            return 1.0f;
        else return -1.0f;

    }
}