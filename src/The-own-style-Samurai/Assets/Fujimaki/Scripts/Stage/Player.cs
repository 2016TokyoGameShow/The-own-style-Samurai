using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
	[SerializeField,Header("移動スピード")]
	private float speed;
    [SerializeField, Header("最大HP")]
    private int maxHP;
    [SerializeField]
    private StageController stageController;
    [SerializeField]
	private CharacterController myController;
    [SerializeField]
    private GameObject cameraRig;

    private int hp;
    private float finisherGageValue;

    private UIController uiController;
    private Vector3 saveMoveVelocity;
    private Vector3 targetVelocity;
	private Renderer myMaterial;

	void Start () {
        uiController = stageController.uiController;
		myMaterial = GetComponent<Renderer> ();
        hp = maxHP;
	}

	void Update () {

        finisherGageValue += Time.deltaTime / 30;
        finisherGageValue = Mathf.Clamp(finisherGageValue, 0, 1);
        uiController.SetFinisherGage(finisherGageValue);


        Vector3 moveVelocity = Vector3.zero;

        //入力から移動ベクトルを計算
        moveVelocity = cameraRig.transform.forward * Input.GetAxis("Vertical");
        moveVelocity += cameraRig.transform.right * Input.GetAxis("Horizontal");

        //進行方向に向く
        if (moveVelocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
        }

        //スピードを適用
        moveVelocity *= speed;
        
        //移動を適用
        myController.Move(moveVelocity);


        //とりあえずよけるアクション
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
