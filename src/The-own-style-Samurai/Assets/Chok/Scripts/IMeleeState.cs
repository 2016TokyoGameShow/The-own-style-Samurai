using UnityEngine;
using System.Collections;

public class IMeleeState : MonoBehaviour
{
    public enum MeleeState
    {
        StandBy,                //待機
        SpecialMove,            //敵の特殊行動
        Attack,                 //攻撃
        Rotate,                 //プレイヤーをちゅんしんに回転
    }

    [SerializeField, HideInInspector]
    protected MeleeEnemy enemy;

    void Awake()
    {
        enemy = GetComponent<MeleeEnemy>();
    }

    public void ActionStart()
    {
        enemy.GetAnimator.SetFloat("Speed", -1);
        enemy.StopAllCoroutines();
        StartCoroutine(StartChoose());
    }

    private IEnumerator StartChoose()
    {
        yield return new WaitForSeconds(0.5f);
        int choose = Random.Range(0, 102);
        ActionChoose((2));
    }

    private void ActionChoose(int state)
    {
        MeleeState eState = (MeleeState)state;
        switch (eState)
        {
            case MeleeState.StandBy:
                StartCoroutine(StandBy());
                break;
            case MeleeState.SpecialMove:
                SpecialMove();
                break;
            case MeleeState.Attack:
                enemy.StartAttack();
                break;
            case MeleeState.Rotate:
                Rotate();
                break;
        }
    }

    private IEnumerator StandBy()
    {
        //待機が終わったら攻撃
        yield return new WaitForSeconds(Random.Range(0, 3));
        enemy.StartAttack();
    }

    protected virtual void SpecialMove() { }

    private void Rotate()
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
        StartCoroutine(StopBeforeAttack());
    }

    private IEnumerator StopBeforeAttack()
    {
        yield return new WaitForSeconds(0.2f);
        enemy.StartAttack();
    }
}
