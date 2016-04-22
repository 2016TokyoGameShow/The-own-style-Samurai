// ----- ----- ----- ----- -----
//
// IWeapon
//
// 作成日：2016/04/21
// 作成者：阿部
//
// <概要>
// 武器の基底クラスです
//
// ----- ----- ----- ----- -----

using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Enemy/Weapon/IWeapon")]
public class IWeapon : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        CollisionEnter(collision);
    }

    protected virtual void CollisionEnter(Collision collision)
    {
        SendHit(collision.gameObject);

        Destroy(gameObject);
    }

    protected void SendHit(GameObject obj)
    {
        ExecuteEvents.Execute<WeaponHitHandler>(
            obj,
            null,
            (_object, _event) => { _object.OnWeaponHit(); }
        );
    }
}