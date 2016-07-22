using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageControllerF : MonoBehaviour
{
    [SerializeField]
    private EnemyGeneratorBase enemyGeneratorBase;

    [SerializeField]
    Player mPlayer;

    bool isGameOver = false;

    private int nowWave;

    void Start()
    {
        enemyGeneratorBase.SetWave(0);
    }


    void Update()
    {
        if(mPlayer.GetHP() <= 0 && !isGameOver)
        {
            isGameOver = true;
            
            SceneChanger.FadeStart("GameOver");
        }
    }

    public void AddWave()
    {
        nowWave++;
        enemyGeneratorBase.SetWave(nowWave);
    }
}
