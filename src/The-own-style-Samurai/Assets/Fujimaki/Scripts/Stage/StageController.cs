using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {

    [SerializeField]
    private Player mPlayer;
    [SerializeField]
    private UIController mUiController;
    [SerializeField]
    private EnemyController mEnemyController;
    [SerializeField]
    private int border;
    private bool isGameOver = false;

    public Player player { get { return mPlayer; }}
    public UIController uiController { get { return mUiController;}}

    public EnemyController enemycontroller {get { return mEnemyController; } }

    void Update()
    {
        if(mPlayer.GetHP() == 0)
        {
            isGameOver = true;
        }
        if(mEnemyController != null && mEnemyController.enemyDeathCount >= border)
        {
            GameObject.Find("Boss").GetComponent<Boss>().BossStart();
            border = 99999999;
        }
    }
}
