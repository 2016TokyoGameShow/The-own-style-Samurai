using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Enemy/Enemygenerator")]
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("敵の種類")]
    List<GameObject> enemies;

    [SerializeField, Tooltip("敵生成位置")]
    List<GameObject> portal;

    [SerializeField, Tooltip("敵生成時間")]
    List<int> addEnemyInterval;

    private float time;


    void Start()
    {
        time = 0;
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
