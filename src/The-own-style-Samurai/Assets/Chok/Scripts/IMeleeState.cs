using UnityEngine;
using System.Collections;

public class IMeleeState : MonoBehaviour
{
    public enum MeleeState
    {
        StandBy,                //待機
        SpecialMove,            //敵の特殊行動
        Attack,                 //攻撃
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
        ActionChoose((choose % 3));
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
                StartCoroutine(enemy.StartAttack());
                break;
        }
    }

    private IEnumerator StandBy()
    {
        //待機が終わったら攻撃
        yield return new WaitForSeconds(Random.Range(0, 3));
        StartCoroutine(enemy.StartAttack());
    }

    protected virtual void SpecialMove() { }
}
