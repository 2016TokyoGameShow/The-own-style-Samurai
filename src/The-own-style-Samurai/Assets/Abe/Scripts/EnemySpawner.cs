using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
    float spawnInterval;

	[SerializeField]
	List<GameObject> enemySpawnList;

    void Awake()
    {
        
    }
    
    void Start()
    {
        StartCoroutine(Spawn());
    }
    
    void Update()
    {
        
    }

	IEnumerator Spawn()
	{
		yield return new WaitForSeconds(spawnInterval);

		int randomSpawn = Random.Range(0, enemySpawnList.Count);
		GameObject spawnObject = enemySpawnList[randomSpawn];
		
		//EnemyController.singleton.enemyInfo
		Instantiate(spawnObject, transform.position, Quaternion.identity);
	}
}