using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    [SerializeField]
    private Player player;
    [SerializeField]
    private Camera mCamera;
    [SerializeField]
    private GameObject target;
    private Vector3 cameraDistance;
    private Vector3 cameraThirdParson;
    private Vector3 rotateVelocity = new Vector3(0,3,0);
    private float setDistance;
	// Use this for initialization
	void Start () {
        cameraThirdParson = new Vector3(45.0f, 0.0f, 0.0f);
        setDistance = 5.0f;
        target.transform.forward = player.transform.forward;

    }
	// Update is called once per frame
	void Update () {
        target.transform.position = player.transform.position;
        cameraDistance = target.transform.position + target.transform.forward * setDistance + new Vector3(0.0f, -setDistance, 0.0f);

        if (Input.GetKey(KeyCode.E))
            target.transform.eulerAngles += rotateVelocity;
        if (Input.GetKey(KeyCode.Q))
            target.transform.eulerAngles -= rotateVelocity;
        if (Input.GetKeyDown(KeyCode.R))
            target.transform.eulerAngles = player.transform.forward;

        mCamera.transform.position = target.transform.position + target.transform.forward * -setDistance + new Vector3(0.0f, setDistance, 0.0f);
        mCamera.transform.eulerAngles = target.transform.eulerAngles + cameraThirdParson;
	}
}
