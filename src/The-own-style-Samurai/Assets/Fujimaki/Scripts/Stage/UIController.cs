using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

    [SerializeField]
    private UITexture hpBer;
    [SerializeField]
    private UITexture finisherGage;
    [SerializeField]
    private GameObject warrningSplash;
    [SerializeField]
    private UILabel enemyCountLabel;

	void Start () {
	
	}
	
	void Update () {
	
	}

    public void SetEnemyCount(int count)
    {
        enemyCountLabel.text = "" + count;
    }

    //HPの表示をセット
    public void SetHPGage(int maxHp, int nowHp)
    {
        hpBer.fillAmount = (float)nowHp / (float)maxHp;
    }

    //必殺技ゲージをセット
    public void SetFinisherGage(float value)
    {
        //finisherGage.fillAmount = value;
    }

    public void SpawnWarringSlphas(GameObject targetObject)
    {
        GameObject g = (GameObject)Instantiate(warrningSplash, Vector3.zero, Quaternion.identity);
        g.GetComponent<WarrningSplash>().Initialize(4, targetObject);
    }
}
