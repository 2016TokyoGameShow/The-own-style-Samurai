using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
    public float time;
	// Use this for initialization
	void Start () {
        //消すカウント起動
        if (time != 0) StartCoroutine(Dead());
	}
	
	// Update is called once per frame
	void Update () {

    }

    private IEnumerator Dead()
    {
        //消すタイミング
        yield return new WaitForSeconds(time);
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーに当たったら消す(暫定）
        if (other.tag == "player") Destroy(gameObject);
    }
}
