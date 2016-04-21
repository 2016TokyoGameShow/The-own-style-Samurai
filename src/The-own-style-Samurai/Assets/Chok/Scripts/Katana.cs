using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[AddComponentMenu("Enemy/Weapon/Katana")]
public class Katana : IWeapon
{

    #region メソッド
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("hit arrow");
        ExecuteEvents.Execute<WeaponHitHandler>(
            collision.gameObject,
            null,
            (_object, _event) => { _object.OnWeaponHit(); }
        );

        Destroy(gameObject);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player") Destroy(other.gameObject);
    //}

    #endregion

}
