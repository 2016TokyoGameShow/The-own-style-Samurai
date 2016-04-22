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
public class IShootWeapon : IWeapon
{
    [SerializeField, Tooltip("飛道具の速さ")]
    float speed;

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
        WeaponStart();
        StartCoroutine(WeaponUpdate());
    }

    protected override IEnumerator WeaponUpdate()
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