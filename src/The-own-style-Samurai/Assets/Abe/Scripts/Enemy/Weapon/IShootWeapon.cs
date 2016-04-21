// ----- ----- ----- ----- -----
//
// IWeapon
//
// 作成日：
// 作成者：
//
// <概要>
//
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Enemy/Weapon/IWeapon")]
public class IShootWeapon : MonoBehaviour
{
    [SerializeField, Tooltip("飛道具の速さ")]
    float speed;

    [SerializeField, Tooltip("オブジェクトの生存時間")]
    float objectLifeTime;

    protected virtual void Awake()
    {
        //RequireComponentにColliderを指定することができないため
        Debug.Assert(GetComponent<Collider>() != null, "当たり判定が追加されていません");
    }

    void Start()
    {
        Shoot();
    }

    protected virtual IEnumerator ShootWaitUpdate()
    {
        yield break;
    }

    public void ShootStop()
    {
        StopAllCoroutines();
    }
    

    public void Shoot()
    {
        ShootStart();
        StartCoroutine(ShootUpdate());
    }

    protected virtual void ShootStart()
    {

    }

    protected virtual IEnumerator ShootUpdate()
    {
        //撃った方向に飛ぶ
        for(float time = 0; time <= objectLifeTime; time += Time.deltaTime)
        {
            transform.position += transform.forward * speed;
            yield return null;
        }
        Destroy(gameObject);
    }
}