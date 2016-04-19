using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
	[SerializeField,Header("移動スピード")]
	private float speed;
    [SerializeField, Header("最大HP")]
    private int maxHP;
    [SerializeField]
	private CharacterController myController;
    [SerializeField]
    private GameObject cameraRig;

    private int hp;

    private Vector3 saveMoveVelocity;
	private Renderer myMaterial;

	void Start () {
		myMaterial = GetComponent<Renderer> ();
        hp = maxHP;
	}

	void Update () {


        Vector3 moveVelocity = Vector3.zero;

        moveVelocity = cameraRig.transform.forward * Input.GetAxis("Vertical") * speed;
        moveVelocity += cameraRig.transform.right * Input.GetAxis("Horizontal") * speed;


        transform.LookAt(transform.position + moveVelocity);
        
        
        print(moveVelocity*100);

        myController.Move(moveVelocity);


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
    //最大HPを取得
    public int GetMaxHP(){ return maxHP; }
    //現在のHPを取得
    public int GetHP() { return hp; }
}
