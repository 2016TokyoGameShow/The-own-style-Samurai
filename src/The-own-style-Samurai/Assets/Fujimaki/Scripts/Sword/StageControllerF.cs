using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageControllerF : MonoBehaviour
{
    [SerializeField]
    private EnemyGeneratorBase enemyGeneratorBase;

    private int nowWave;
    // Use this for initialization
    void Start()
    {
        enemyGeneratorBase.SetWave(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddWave()
    {
        nowWave++;
        enemyGeneratorBase.SetWave(nowWave);
    }

}
