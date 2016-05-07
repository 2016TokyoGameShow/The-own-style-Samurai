using UnityEngine;
using System.Collections;

public class EnemyAction : MonoBehaviour
{

    public enum EnemyState      //敵の状態
    {
        Attack,                 //攻撃
        StandBy,                //待機
        RotateAround,          //プレイヤーを中心に回る
        StepBack
    }

    [SerializeField]
    MeleeEnemy enemy;

    public void ActionChoose(int state)
    {
        EnemyState eState = (EnemyState)state;
        Debug.Log(state.ToString());
        switch (eState)
        {
            case EnemyState.StandBy:
                StartCoroutine(StandBy());
                break;
            case EnemyState.RotateAround:
                StartRotate();
                break;
            case EnemyState.Attack:
                enemy.StartAttack();
                break;
            case EnemyState.StepBack:
                StartCoroutine(StepBack());
                break;
        }
    }

    private IEnumerator StandBy()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));
        enemy.StartAttack();
    }

    private void StartRotate()
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
            yield return 0;
        }
        enemy.StartAttack();
    }

    IEnumerator StepBack()
    {
        for (float i = 0; i < 2; i += Time.deltaTime)
        {
            transform.position += -transform.forward / 200;
            yield return 0;
        }
        enemy.StartAttack();
    }

}
