// ----- ----- ----- ----- -----
//
// IEnemy
//
// 作成日：2016/04/19
// 作成者：阿部
//
// <概要>
// 敵のインターフェイスです
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Enemy/IEnemy")]
[RequireComponent(typeof(Collider))]
public abstract class IEnemy : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public abstract void Attack();
    public abstract void AttackReady();
    public abstract void Move();

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    public virtual void Dead()
    {
        //敵を吹っ飛ばしてから消去のほうがいいか
        Destroy(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EnemyBullet")
        {
            Dead();
        }
    }
}