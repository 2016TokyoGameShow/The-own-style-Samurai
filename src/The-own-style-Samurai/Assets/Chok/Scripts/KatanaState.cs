﻿using UnityEngine;
using System.Collections;

public class KatanaState : IMeleeState
{
    protected override void SpecialMove()
    {
        float time = Random.Range(1, 3);                                //回転周期を決める
        float angle = Random.Range(-2, 3);                              //回転角度を決める
        if (angle == 0) angle = 1;                                      //止めさせない
        Vector3 rotateOrigin = enemy.GetPlayer().transform.position;    //最初に回転中心を決める
        StopAllCoroutines();
        StartCoroutine(RotateAroundPlayer(rotateOrigin, time, angle));
    }

    IEnumerator RotateAroundPlayer(Vector3 rotateOrigin, float time, float angle)
    {
        for (float rotateTime = 0; rotateTime <= time; rotateTime += Time.deltaTime)
        {
            transform.RotateAround(rotateOrigin, Vector3.up, angle);
            enemy.GetAnimator.SetFloat("Speed", 1);
            yield return 0;
        }
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.StartAttack();
    }
}