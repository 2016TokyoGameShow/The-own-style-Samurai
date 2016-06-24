using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControllerF : MonoBehaviour {


    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private EndManager endManager;
    [SerializeField]
    private int endCountNum;

    private List<SwordEnemy> enemys;
    private List<Arrow> arrows;

    private float orderCounter;

    private bool end;

    private int spawnedEnemyCount;
    private int spawnedArrowCount;

	void Awake () {
        enemys = new List<SwordEnemy>();
        arrows = new List<Arrow>();
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

    public int GetSpawnArrowEnemy()
    {
        return spawnedArrowCount;
    }

    public void AddEnemy(SwordEnemy e)
    {
        enemys.Add(e);
        spawnedEnemyCount++;
    }

    public void AddArrow(Arrow e)
    {
        arrows.Add(e);
        spawnedArrowCount++;
    }

    public void RemoveEnemy(SwordEnemy e)
    {
        enemys.Remove(e);
        spawnedEnemyCount--;
        enemyController.enemyDeathCount++;
        uiController.SetEnemyCount(enemyController.enemyDeathCount);

        if (enemys.Count == 0)
        {
            end = true;
        }

        if (enemyController.enemyDeathCount >= endCountNum)
        {
            endManager.End();
        }
    }

    public void RemoveArrow(Arrow e)
    {
        arrows.Remove(e);
        spawnedArrowCount--;
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
