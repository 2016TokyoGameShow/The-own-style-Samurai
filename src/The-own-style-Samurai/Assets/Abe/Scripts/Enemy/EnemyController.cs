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

    public enum EnemyKind
    {
        Sword = 0,
        Spear = 1,
        Arrow = 2,
    }

    [System.Serializable]
    class Info
    {
        public EnemyKind kind;

        public List<GameObject> attackCount;

        public int attackMaxCount;

        public int maxNumber;

        public int number;

        public Info(EnemyKind kind)
        {
            this.kind   = kind;
            attackCount = new List<GameObject>();
        }
    }

    [SerializeField]
    Info[] enemyInfo = new Info[] 
    {
        new Info(EnemyKind.Sword),
        new Info(EnemyKind.Spear),
        new Info(EnemyKind.Arrow),
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

    public void AddEnemy(GameObject enemy, EnemyKind kind)
    {
        
        enemies.Add(enemy);
        enemyInfo[(int)kind].number++;
    }

    public void EraseEnemy(GameObject enemy, EnemyKind kind)
    {
        if(enemies.Remove(enemy))
        {
            enemyInfo[(int)kind].number--;
        }
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

    public void AttackRandom(EnemyKind kind, int attackNumber)
    {
        Info info  = enemyInfo[(int)kind];
        int  count = info.attackMaxCount - info.attackCount.Count - attackNumber;
        
        //制限
        if(count < 0)
        {
            attackNumber -= count;
        }

        int rand = 0;
        for(int i = 0; i < attackNumber; i++)
        {
            rand = Random.Range(0, enemies.Count - 1);
            enemies[rand].GetComponent<Enemy>().AttackEnemy();
        }
    }

    public bool Attack(GameObject enemy, EnemyKind kind)
    {
        //攻撃している数が多ければ何もしない
        if(IsAttackMax(kind))
        {
            return false;
        }

        enemyInfo[(int)kind].attackCount.Add(enemy);
        return true;
    }

    public void AttackEnd(GameObject enemy, EnemyKind kind)
    {
        enemyInfo[(int)kind].attackCount.Remove(enemy);
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
        Info info = enemyInfo[(int)kind];
        return info.attackCount.Count >= info.attackMaxCount;
    }

    bool IsMaxNumber(EnemyKind kind)
    {
         return enemyInfo[(int)kind].number >= enemyInfo[(int)kind].maxNumber;
    }

    public void EnemyChangeWeapon(Enemy enemy)
    {
        EnemyKind kind = enemy.Kind;
        GameObject enemyObject = enemy.gameObject;

        Destroy(enemy);

        enemyObject.AddComponent<AssaultEnemy>();
    }
    #endregion
}
