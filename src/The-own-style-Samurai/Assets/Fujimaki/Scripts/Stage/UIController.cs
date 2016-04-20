using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

    [SerializeField]
    private UITexture hpBer;
    [SerializeField]
    private UITexture finisherGage;

	void Start () {
	
	}
	
	void Update () {
	
	}


    //HPの表示をセット
    public void SetHPGage(int maxHp, int nowHp)
    {
        hpBer.fillAmount = (float)nowHp / (float)maxHp;
    }

    //必殺技ゲージをセット
    public void SetFinisherGage(float value)
    {
        finisherGage.fillAmount = value;
    }
}
