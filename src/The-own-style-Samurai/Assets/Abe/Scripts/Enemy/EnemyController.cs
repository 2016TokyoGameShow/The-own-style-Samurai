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

    [Serializable]
    struct info
    {
        public int max;
        public float time;
    }


    [SerializeField]
    Player _player;

    [SerializeField]
    List<GameObject> enemies;

    public enum EnemyKind
    {
        Sword = 0,
        Spear = 1,
        Arrow = 2,
    }

    [Serializable]
    class AttackInfo
    {
        //[HideInInspector]
        public EnemyKind kind;

        //[HideInInspector]
        public List<GameObject> count;

        [SerializeField]
        public string str;

        [SerializeField]
        public int maxCount;

        public AttackInfo(EnemyKind kind, string str)
        {
            this.kind = kind;
            this.str  = str;
            count     = new List<GameObject>();
        }
    }

    [SerializeField]
    AttackInfo[] attackInfo = new AttackInfo[] 
    {
        new AttackInfo(EnemyKind.Sword, EnemyKind.Sword.ToString()),
        new AttackInfo(EnemyKind.Spear, EnemyKind.Spear.ToString()),
        new AttackInfo(EnemyKind.Arrow, EnemyKind.Arrow.ToString()),
    };

    [SerializeField, Tooltip("敵がフィールドに最大でいられる数")]
    int _enemyMaxNumber;
    
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

    public int enemyMaxNumber
    {
        get { return _enemyMaxNumber; }
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

    public bool Attack(GameObject enemy, EnemyKind kind)
    {
        //攻撃している数が多ければ何もしない
        if(IsAttackMax(kind))
        {
            return false;
        }

        attackInfo[(int)kind].count.Add(enemy);
        return true;
    }

    public void AttackEnd(GameObject enemy, EnemyKind kind)
    {
        attackInfo[(int)kind].count.Remove(enemy);
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
        //foreach(AttackInfo i in attackInfo)
        //{
        //    foreach(GameObject enemy in i.count)
        //    {
        //        if(enemy == null)
        //        {
        //            enemies.Remove(enemy);
        //        }
        //    }
        //}
    }

    bool IsAttackMax(EnemyKind kind)
    {
        AttackInfo info = attackInfo[(int)kind];
        return info.count.Count >= info.maxCount;
    }

    #endregion
}