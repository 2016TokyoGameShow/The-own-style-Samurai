using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    public EnemyGeneratorBase enemyGenerator;

    public int count = 3;

	void Start () {
        StartCoroutine(Loop());
	}
	
    private IEnumerator Loop()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));

        while (true)
        {
            if (enemyGenerator.maxCount > enemyGenerator.enemyControllerF.GetSpawnedEnemy())
            {
                Instantiate(enemyGenerator.swordEnemy, transform.position, transform.rotation);
            }
            yield return new WaitForSeconds(count+Random.Range(0,2));
        }
    }
}
