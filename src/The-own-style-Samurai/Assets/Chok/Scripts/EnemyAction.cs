using UnityEngine;
using System.Collections;

public class EnemyAction : MonoBehaviour
{

    public enum EnemyState      //敵の状態
    {
        StandBy,                //待機
        RotateAround,          //プレイヤーを中心に回る
        Attack,                 //攻撃
        StepBack
    }

    [SerializeField, HideInInspector]
    MeleeEnemy enemy;

    void Start()
    {
        enemy = GetComponent<MeleeEnemy>();
    }

    public void ActionStart()
    {
        enemy.StopAllCoroutines();
        enemy.StartCoroutine(StartChoose());
    }

    public IEnumerator StartChoose()
    {
        yield return new WaitForSeconds(0.5f);
        int choose = Random.Range(0, 102);
        ActionChoose((choose % 4));
    }

    public void ActionChoose(int state)
    {
        EnemyState eState = (EnemyState)state;
        Debug.Log(state.ToString());
        switch (eState)
        {
            case EnemyState.StandBy:
                enemy.GetAnimator.SetFloat("Speed", -1);
                StartCoroutine(StandBy());
                break;
            case EnemyState.RotateAround:
                StartRotate();
                break;
            case EnemyState.Attack:
                StartCoroutine(enemy.StartAttack());
                break;
            case EnemyState.StepBack:
                StartCoroutine(StepBack());
                break;
        }
    }

    private IEnumerator StandBy()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));
        StartCoroutine(enemy.StartAttack());
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
        for (float rotateTime = 0; rotateTime <= 0.06; rotateTime += Time.deltaTime)
        {
            transform.RotateAround(rotateOrigin, Vector3.up, 15.0f);
            enemy.GetAnimator.SetFloat("Speed", 1);
            yield return 0;
        }
        StartCoroutine(enemy.StartAttack());
    }

    IEnumerator StepBack()
    {
        for (float i = 0; i < 0.06; i += Time.deltaTime)
        {
            for (int j = 0; j < 5; ++j)
            {
                transform.position += -transform.forward / 10;
            }
            yield return 0;
        }
        StartCoroutine(enemy.StartAttack());
    }

}
