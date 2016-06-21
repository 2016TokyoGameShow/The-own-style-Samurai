using UnityEngine;
using System.Collections;

public class EnemyGeneratorBase : MonoBehaviour {

    [SerializeField]
    private Generator[] wave0;
    [SerializeField]
    private Generator[] wave1;
    [SerializeField]
    private Generator[] wave2;

    [SerializeField]
    private Boss boss;
   

    public int maxCount;
    public int maxArrowCount;

    public GameObject swordEnemy;
    public GameObject shootEnemy;

    public EnemyControllerF enemyControllerF;


    public void SetWave(int value)
    {
        print("NowWave" + value);
        
        for (int i = 0; i < wave0.Length; i++) { wave0[i].online = false; }
        for (int i = 0; i < wave1.Length; i++) { wave1[i].online = false; } 
        for (int i = 0; i < wave2.Length; i++) { wave2[i].online = false; }
        
        switch (value)
        {
            case 0:
                print("Zero");
                foreach (var w in wave0) { w.online = true; }
                break;
            case 1:
                print("One");
                foreach (var w in wave1) { w.online = true; }
                break;
            case 2:
                foreach (var w in wave2) { w.online = true; }
                StartCoroutine(boss.Launch());
                break;

        }
    }
}
