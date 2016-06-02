using UnityEngine;
using System.Collections;

public class NextDoorOpen : MonoBehaviour {
    [SerializeField]
    private StageController stageController;
    [SerializeField]
    private int border;

    private EnemyController enemycontroller;
    // Use this for initialization
    void Start () {
        enemycontroller = stageController.enemycontroller;
    }
	
	// Update is called once per frame
	void Update () {
        if (border <= enemycontroller.enemyDeathCount)
        {
            GameObject.Destroy(gameObject);
        }
	}
}
