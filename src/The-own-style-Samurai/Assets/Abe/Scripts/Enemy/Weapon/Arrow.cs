﻿// ----- ----- ----- ----- -----
//
// Arrow
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
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
[AddComponentMenu("Enemy/Weapon/Arrow")]
public class Arrow : IWeapon
{
    #region 変数

    #endregion


    #region プロパティ



    #endregion


    #region メソッド

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit arrow");
        ExecuteEvents.Execute<WeaponHitHandler>(
            collision.gameObject,
            null,
            (_object, _event) => {_object.OnWeaponHit(); }
        );

        Destroy(gameObject);
    }

    #endregion
}