using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Enemy/Enemygenerator")]
public class EnemyGenerator : MonoBehaviour
{
    public static EnemyGenerator singleton;

    [SerializeField, Tooltip("敵の種類")]
    List<Enemy> enemies;

    [SerializeField, Tooltip("敵生成位置")]
    List<GameObject> portal;

    [SerializeField, Tooltip("敵生成時間")]
    List<int> addEnemyInterval;

    private float time;

    int angle;

    public int Angle
    {
        get
        {
            angle += 2;angle %= 5;
            return angle;
        }
    }

    void Start()
    {
        time = 0;
        angle = 0;
    }

    public void OnEnable()
    {
        singleton = this;
    }

    void Update()
    {
        GenerateEnemy();
    }

    void GenerateEnemy()
    {
        int eNumber = EnemyController.singleton.enemyNumber;
        int eMax = EnemyController.singleton.enemyMaxNumber;
        if (eNumber > eMax) return;

        time += Time.deltaTime;
        if (time < addEnemyInterval[eNumber]) return;

        //敵の種類(3:1)(近距離：遠距離)で生成
        int enemyType = Random.Range(0, 4);
        if (enemyType != 0) enemyType = 1;
        //生成位置
        int initialPos = Random.Range(0, portal.Count);
        Instantiate(
            enemies[enemyType],
            portal[initialPos].transform.position,
            portal[initialPos].transform.rotation
            );
    }
}
