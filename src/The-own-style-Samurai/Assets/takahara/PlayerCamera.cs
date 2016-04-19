using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
    private GameObject player;
    private Vector3 cameraDistance;
    private Vector3 cameraRotate;
    private float setDistance;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraRotate = new Vector3(45.0f, 0.0f, 0.0f);
        setDistance = 5.0f;
	}
	　
	// Update is called once per frame
	void Update () {
        cameraDistance = player.transform.forward * setDistance + new Vector3(0.0f, -setDistance, 0.0f);



        gameObject.transform.position = player.transform.position - cameraDistance;
        gameObject.transform.eulerAngles = player.transform.eulerAngles + cameraRotate;
	}
}
