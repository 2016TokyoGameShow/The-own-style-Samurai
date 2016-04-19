// ----- ----- ----- ----- -----
//
// Archer
//
// 作成日：2016/04/19
// 作成者：阿部
//
// <概要>
// 遠距離攻撃をする敵です
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Enemy/Archer")]
public class ShootEnemy : IEnemy
{
    #region 変数

    [SerializeField, Tooltip("説明文")]
    IWeapon weapon;

    #endregion


    #region プロパティ



    #endregion


    #region メソッド

    public override void Attack()
    {
        IWeapon obj = (IWeapon)Instantiate(weapon, transform.position, Quaternion.identity);
        
    }

    public override void AttackReady()
    {
        
    }

    public override void Move()
    {
        
    }
	#endregion
}