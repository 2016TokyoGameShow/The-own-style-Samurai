using UnityEngine;
using System.Collections;

public class Enemy_Distance_Short : MonoBehaviour {

    NavMeshAgent agent;
    public Transform player;        //プレイヤー
    public GameObject attack;       //攻撃判定（刀にくっつける）
    public GameObject katana;       //刀のボーン


    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Prepare();
        Attack();
    }

    private void Move()
    {
        //プレイヤーに向くようにする
        //キャラクターコントローラを使うなら消してもいい
        float angle = Mathf.Atan2(player.position.z - transform.position.z, player.position.x - transform.position.x);
        angle *= 180.0f / Mathf.PI;
        Quaternion.Euler(0, angle, 0);

        //プレイヤーに向かって移動
        agent.SetDestination(player.position);

        //プレイヤーのまわりに止まって、攻撃するか円周運動するか判断する
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            //攻撃合図を出す(未完成）
            //GameObject warn = Instantiate(sign);
            //warn.transform.position = transform.position+new Vector3(0.0f,1.0f,0.0f);
            transform.RotateAround(player.position, Vector3.up, 1.0f);
        }
    }

    private void Attack()
    {
        //攻撃をする
        AttackInstantiate(attack, katana);
    }

    private void Prepare()
    {
    }

    //判定生成メソット、生成判定、生成位置
    protected void AttackInstantiate(GameObject hitObject, GameObject hitOffset)
    {
        GameObject hit;

        hit = Instantiate(hitObject, hitOffset.transform.position, transform.rotation) as GameObject;
        hit.transform.parent = hitOffset.transform;
    }
}
