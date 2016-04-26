using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour,WeaponHitHandler {
    
	[SerializeField,Header("移動スピード")]
	private float speed;
    [SerializeField, Header("回避スピード")]
    private float avoidanceSpeed;
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

    private Coroutine avoidanceAction;

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

        //入力から移動ベクトルを計算して移動
        if (avoidanceAction == null)
        {
            moveVelocity = cameraRig.transform.forward * Input.GetAxis("Vertical");
            moveVelocity += cameraRig.transform.right * Input.GetAxis("Horizontal");
            CharacterMove(moveVelocity, speed);
        }



        //とりあえずよけるアクション
		if (Input.GetKeyDown (KeyCode.UpArrow)){
			myMaterial.material.color = Color.red;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
            if (avoidanceAction == null)
            {
                avoidanceAction = StartCoroutine(AvoidanceAction());
            }
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


    //ダメージを受ける
    public void OnWeaponHit(int damage)
    {
        hp -= damage;
    }

    //キャラクター移動
    private void CharacterMove(Vector3 moveVelocity,float speed)
    {
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
    }

    //回避アクション
    private IEnumerator AvoidanceAction()
    {

        myMaterial.material.color = Color.blue;

        float moveTime = 0.3f;

        //回避アクション中は向いている方向に一定数移動
        while (moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            CharacterMove(transform.forward, avoidanceSpeed);
            yield return new WaitForEndOfFrame();
        }

        myMaterial.material.color = Color.white;

        avoidanceAction = null;
    }
}
