using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    [SerializeField]
    private Player player;
    [SerializeField]
    private Camera mCamera;
    private Vector3 rotateVelocity = new Vector3(0,5,0);

	// Use this for initialization
	void Start () {
    }
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;

        if (Input.GetKey(KeyCode.E))
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + rotateVelocity, 5.0f * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles - rotateVelocity, 5.0f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.R))
            transform.eulerAngles = player.transform.eulerAngles;
    }
}
