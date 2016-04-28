// ----- ----- ----- ----- -----
//
// WeaponHitHandler
//
// 作成日：
// 作成者：
//
// <概要>
//
//
// ----- ----- ----- ----- -----

using UnityEngine;
using UnityEngine.EventSystems;

public interface WeaponHitHandler : IEventSystemHandler
{
    void OnWeaponHit(int damege, GameObject attackObject);
}
