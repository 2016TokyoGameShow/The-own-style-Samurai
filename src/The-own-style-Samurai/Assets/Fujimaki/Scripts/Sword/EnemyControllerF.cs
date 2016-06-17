using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControllerF : MonoBehaviour {

    private List<SwordEnemy> enemys;

    private float orderCounter;

    private bool end;

    private int spawnedEnemyCount;

	void Awake () {
        enemys = new List<SwordEnemy>();
	}
	

	void Update () {

        if (!end)
        {
            orderCounter += Time.deltaTime;
        }

        if (orderCounter > 3)
        {

            CheckOrder();
            orderCounter = 0;
        }
	}

    public int GetSpawnedEnemy()
    {
        return spawnedEnemyCount;
    }

    public void AddEnemy(SwordEnemy e)
    {
        enemys.Add(e);
        spawnedEnemyCount++;
    }

    public void RemoveEnemy(SwordEnemy e)
    {
        enemys.Remove(e);
        spawnedEnemyCount--;

        if (enemys.Count == 0)
        {
            end = true;
        }
    }

    private void CheckOrder()
    {
        bool allStenby = true;

        if (enemys.Count > 0)
        {
            foreach (var e in enemys)
            {
                //たまにランダムに移動
                if (Random.Range(0, 3) == 0)
                {
                    e.GetOut();
                }

                //全員スタンバイなら攻撃アクションを開始
                if (!e.GetStenby())
                {
                    allStenby = false;
                }
            }

            if (allStenby)
            {
                //一体目を選定
                int num = Random.Range(0, enemys.Count);

                enemys[num].SetStenby(false);

                //二体目を選定
                for (int i = 0; i < 100; i++)
                {
                    int num2 = Random.Range(0, enemys.Count);
                    if (num2 != num)
                    {
                        enemys[num2].SetStenby(false);
                        break;
                    }
                }

                foreach (var e in enemys)
                {
                    //たまにランダムに移動
                    if (Random.Range(0, 4) == 0)
                    {
                        e.GetOut();
                    }
                }
            }
        }
    }
}
