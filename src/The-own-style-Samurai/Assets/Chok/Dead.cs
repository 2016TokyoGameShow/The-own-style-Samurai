using UnityEngine;
using System.Collections;

public class Dead : MonoBehaviour {

    public float time;      //何秒後に消す

	// Use this for initialization
	void Start () {
        //Deadメソットカウントダウン
        if(time!=0) StartCoroutine(Erase());
    }

    private IEnumerator Erase()
    {
        //消すタイミング
        yield return new WaitForSeconds(time);
        DestroyObject(gameObject);
    }
}
