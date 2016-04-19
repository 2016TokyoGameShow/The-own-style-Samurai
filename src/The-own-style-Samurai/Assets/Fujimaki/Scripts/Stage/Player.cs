using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
	[SerializeField,Header("移動スピード")]
	private float speed;
	[SerializeField]
	private CharacterController myController;


	private Renderer myMaterial;

	void Start () {
		myMaterial = GetComponent<Renderer> ();
	}

	void Update () {

		myController.Move (new Vector3 (Input.GetAxis ("Vertical")*speed, 0,Input.GetAxis ("Horizontal")*-speed));


		if (Input.GetKeyDown (KeyCode.UpArrow)){
			myMaterial.material.color = Color.red;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			myMaterial.material.color = Color.blue;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			myMaterial.material.color = Color.yellow;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			myMaterial.material.color = Color.green;
		}
	}
}
