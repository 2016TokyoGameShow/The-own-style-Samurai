using UnityEngine;
using System.Collections;

public class WarrningSplash : MonoBehaviour {


    private GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position;
    }

    public void Initialize(float time,GameObject target)
    {
        this.target = target;
        Destroy(this.gameObject, time);

    }
}
