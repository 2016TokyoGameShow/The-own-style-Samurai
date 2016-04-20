// ----- ----- ----- ----- -----
//
// TestPlayer
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

[AddComponentMenu("Test/TestPlayer")]
public class TestPlayer : MonoBehaviour, WeaponHitHandler
{
    #region 変数

    //[SerializeField, Tooltip("説明文")]

    #endregion


    #region プロパティ



    #endregion


    #region メソッド

    // 初期化処理
    void Awake()
    {

    }

    // 更新前処理
    void Start()
    {
        
    }

    // 更新処理
    void Update()
    {
        
    }

    public void OnWeaponHit()
    {
        foreach(GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            ExecuteEvents.Execute<PlayerDeadHandler>(
                obj,
                null,
                (_object, _event) => { _object.OnPlayerDead(); }
            );
        }
        Destroy(gameObject);
    }
	#endregion
}