using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[AddComponentMenu("Enemy/Enemygenerator")]
public class EnemyGenerator : MonoBehaviour {

    [SerializeField, Tooltip("敵の種類")]
    List<Enemy> enemies;

    [SerializeField,Tooltip("敵生成位置")]
    List<GameObject> portal;

    void Start()
    {
        Reset();
    }

    IEnumerator GenerateEnemy()
    {
        yield return new WaitForSeconds(2);
        if (EnemyController.singleton.enemyNumber < 5)
        {
            //敵の種類(3:1)(近距離：遠距離)で生成
            int enemyType = Random.Range(0, 4);
            if (enemyType != 1) enemyType = 0;
            //生成位置
            int initialPos = Random.Range(0, portal.Count);
            Instantiate(
                enemies[enemyType],
                portal[initialPos].transform.position,
                portal[initialPos].transform.rotation
                );
        }
            
        Reset();
    }

    void Reset()
    {
        StopAllCoroutines();
        StartCoroutine(GenerateEnemy());
    }
}
