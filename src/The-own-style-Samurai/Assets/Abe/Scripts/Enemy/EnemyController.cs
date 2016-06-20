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
        Melee = 0,
        Arrow = 1,
    }

    [System.Serializable]
    public class Info
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
    Info[] _enemyInfo = new Info[] 
    {
        new Info(EnemyKind.Melee),
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
        set { _enemyDeathCount = value; }
    }

    public int enemyAttackCount
    {
        get { return _enemyAttackCount; }
    }

    public int enemyMaxNumber
    {
        get { return _enemyMaxNumber; }
    }

	public Info[] enemyInfo
	{
		get { return _enemyInfo; }
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
        _enemyInfo[(int)kind].number++;
    }

    public void EraseEnemy(GameObject enemy, EnemyKind kind)
    {
        if(enemies.Remove(enemy))
        {
            _enemyInfo[(int)kind].number--;
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
        Info info  = _enemyInfo[(int)kind];
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

        _enemyInfo[(int)kind].attackCount.Add(enemy);
        return true;
    }

    public void AttackEnd(GameObject enemy, EnemyKind kind)
    {
        _enemyInfo[(int)kind].attackCount.Remove(enemy);
    }

    // 初期化処理
    void Awake()
    {
        
    }

    // 更新前処理
    void Start()
    {
        StartCoroutine(MeleeEnemyAttackUpdate());
    }

    // 更新処理
    void Update()
    {

    }

    IEnumerator MeleeEnemyAttackUpdate()
    {
        List<MeleeEnemy> meleeEnemy = new List<MeleeEnemy>();
        List<int> count = new List<int>();
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));

            meleeEnemy.Clear();
            foreach(GameObject enemy in enemies)
            {
                MeleeEnemy e = enemy.GetComponent<MeleeEnemy>();
                if(e != null)
                {
                    meleeEnemy.Add(e);
                }
            }
            if(meleeEnemy.Count == 0)
            {
                continue;
            }

            count.Clear();
            for(int i = 0; i < _enemyInfo[(int)EnemyKind.Melee].attackMaxCount; i++)
            {
                int rand = Random.Range(0, meleeEnemy.Count);
                meleeEnemy[rand].AttackEnemy();
            }
        }
    }

    bool IsEqualList(List<int> list, int num)
    {
        foreach(int c in list)
        {
            if(c == num)
            {
                return true;
            }
        }

        return false;
    }

    bool IsAttackMax(EnemyKind kind)
    {
        Info info = _enemyInfo[(int)kind];
        return info.attackCount.Count >= info.attackMaxCount;
    }

    bool IsMaxNumber(EnemyKind kind)
    {
         return _enemyInfo[(int)kind].number >= _enemyInfo[(int)kind].maxNumber;
    }

    public void EnemyAssault(Vector3 position, float radius)
    {
        foreach(GameObject enemy in enemies)
        {
            float dis = Vector3.Distance(position, enemy.transform.position);

            bool isRange = dis < radius;
            bool isNotArcher = enemy.GetComponent<ShootEnemy>() == null;

            if(isRange && isNotArcher)
            {
                EnemyChangeWeapon(enemy.GetComponent<Enemy>());
            }
        }
    }

    private void EnemyChangeWeapon(Enemy enemy)
    {
        EnemyKind kind = enemy.Kind;
        GameObject enemyObject = enemy.gameObject;

        Destroy(enemy);

 //     enemyObject.AddComponent<AssaultEnemy>();
    }

    public void EnemyCall(int callnum)
    {
        int num = 0;

        foreach(GameObject enemy in enemies)
        {
            MeleeEnemy e = enemy.GetComponent<MeleeEnemy>();

            if(e == null)
            {
                continue;
            }

            e.GatherCalled();
            num++;

            if(num >= callnum) break;
        }
        //GatherCalled
    }
    
    public void EnemySummon(int number, Enemy enemy, Vector3 center, Vector3 forward, float interval)
    {
        GameObject root = (GameObject)Instantiate(enemy.gameObject, center,   Quaternion.LookRotation(forward));

        for(int i = 0; i < number; i++)
        {
            Instantiate(enemy.gameObject, center + root.transform.right * interval, Quaternion.LookRotation(forward));
            Instantiate(enemy.gameObject, center - root.transform.right * interval, Quaternion.LookRotation(forward));
        }
    }
    #endregion
}
