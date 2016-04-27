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
    Player _player;

    [SerializeField]
    List<GameObject> enemies;

    [SerializeField, Tooltip("敵がフィールドに最大でいられる数")]
    int enemyMaxNumber;

    int _enemyNumber;

    int _enemyDeathCount;

    int _enemyAttackCount;

    #endregion


    #region プロパティ

    public Player player
    {
        get { return _player; }
    }

    public int enemyNumber
    {
        get { return enemies.Count; }
    }

    public bool isEnemyExists
    {
        get { return enemies.Count <= 0; }
    }

    public int enemyDeathCount
    {
        get { return _enemyDeathCount; }
    }

    public int enemyAttackCount
    {
        get { return _enemyAttackCount; }
    }
    
    #endregion

    #region メソッド

    public void OnEnable()
    {
        //Findを無駄に使わないようにするため
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

    public void AddDeathCount()
    {
        _enemyDeathCount++;
    }

    public void ResetDeathCount()
    {
        _enemyDeathCount = 0;
    }

    public void AddAttackCount()
    {
        _enemyAttackCount++;
    }

    public void EraseAttackCount()
    {
        _enemyAttackCount--;
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