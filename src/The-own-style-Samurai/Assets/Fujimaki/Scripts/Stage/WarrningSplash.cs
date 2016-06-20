using UnityEngine;
using System.Collections;

public class WarrningSplash : MonoBehaviour {


    private GameObject target;


	// Use this for initialization
	void Start () {
        transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = target.transform.position + new Vector3(0, 3.5f, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize(float time,GameObject target)
    {
        this.target = target;
        Destroy(this.gameObject, 1);

    }
}
