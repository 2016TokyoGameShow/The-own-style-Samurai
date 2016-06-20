using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

    public EnemyGeneratorBase enemyGenerator;
    public bool online;

    public int count = 3;

	void Start () {
        StartCoroutine(Loop());
	}
	
    private IEnumerator Loop()
    {
        yield return new WaitForSeconds(Random.Range(0, 3));

            while (true)
            {
            if (online)
            {
                if (enemyGenerator.maxCount > enemyGenerator.enemyControllerF.GetSpawnedEnemy())
                {
                    if (Random.Range(0, 1) == 0)
                    {
                        Instantiate(enemyGenerator.swordEnemy, transform.position, transform.rotation);
                    }
                    else
                    {
                       // Instantiate(enemyGenerator.shootEnemy, transform.position, transform.rotation);
                    }
                }
            }
            yield return new WaitForSeconds(count + Random.Range(0, 2));
        }
    }
}
