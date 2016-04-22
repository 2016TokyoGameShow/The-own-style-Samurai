// ----- ----- ----- ----- -----
//
// EnemyController
//
// 作成日：
// 作成者：
//
// <概要>
//
//
// ----- ----- ----- ----- -----

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MyScript/EnemyController")]
public class EnemyController : MonoBehaviour
{
    #region 変数

    public static EnemyController singleton;

    [SerializeField]
    List<GameObject> enemies;

    [SerializeField, Tooltip("敵がフィールドに最大でいられる数")]
    int enemyMaxNumber;

    int _enemyNumber;

    #endregion


    #region プロパティ

    public int enemyNumber
    {
        get
        {
            return _enemyNumber;
        }
    }

    public bool isEnemyExists
    {
        get
        {
            return _enemyNumber > 0;
        }
    }

    

    #endregion

    #region メソッド

    public void OnEnable()
    {
        if(singleton != null) return;
        singleton = this;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void EraseEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

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

    #endregion
}