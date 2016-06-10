using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

    [SerializeField]
    private Player mPlayer;
    [SerializeField]
    private UIController mUiController;
    [SerializeField]
    private ParticleController mParticleController;
    [SerializeField]
    private EnemyController mEnemyController;
    [SerializeField]
    private int border;

    public Player player { get { return mPlayer; }}
    public UIController uiController { get { return mUiController;}}
    public ParticleController particleController { get { return mParticleController; } }

    public EnemyController enemycontroller {get { return mEnemyController; } }

    void Update()
    {
        if(mEnemyController.enemyDeathCount >= border)
        {
            GameObject.Find("Boss").GetComponent<Boss>().BossStart();
            border = 99999999;
        }
    }
}
