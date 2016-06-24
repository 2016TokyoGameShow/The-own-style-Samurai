using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageControllerF : MonoBehaviour
{
    [SerializeField]
    private EnemyGeneratorBase enemyGeneratorBase;

    private int nowWave;

    void Start()
    {
        enemyGeneratorBase.SetWave(0);
    }


    void Update()
    {

    }

    public void AddWave()
    {
        nowWave++;
        enemyGeneratorBase.SetWave(nowWave);
    }

}
