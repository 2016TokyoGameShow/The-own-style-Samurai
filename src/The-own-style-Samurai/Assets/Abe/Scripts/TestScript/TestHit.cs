// ----- ----- ----- ----- -----
//
// TestHit
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
using System;

[AddComponentMenu("MyScript/TestHit")]
public class TestHit : MonoBehaviour, WeaponHitHandler
{
    
    #region 変数

    //[SerializeField, Tooltip("説明文")]

    #endregion


    #region プロパティ



    #endregion


    #region メソッド

    public void OnWeaponHit()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        SendPlayerDeadEvent();
    }

    void SendPlayerDeadEvent()
    {
        foreach(GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            ExecuteEvents.Execute<PlayerDeadHandler>(
                obj,
                null,
                (_object, _event) => { _object.OnPlayerDead(); }
            );
        }
    }
	#endregion
}