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
public abstract class IWeapon : MonoBehaviour
{
    float speed;
    float friction;
    float time;

    public virtual void Initialize()
    {

    }
}