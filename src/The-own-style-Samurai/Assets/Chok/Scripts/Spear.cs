using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Spear : IMeleeWeapon
{

    protected override void OnTriggerEnter(Collider other)
    {
        ExecuteEvents.Execute<WeaponHitHandler>(
            other.gameObject,
            null,
            (_object, _event) => { _object.OnWeaponHit(); }
        );
    }
}
