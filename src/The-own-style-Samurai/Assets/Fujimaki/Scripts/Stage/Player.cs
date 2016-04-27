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

    [SerializeField]
	private Renderer myMaterial;

	void Start () {
        uiController = stageController.uiController;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (avoidanceAction == null)
            {
                avoidanceAction = StartCoroutine(AvoidanceAction());
            }
        }
	}
    //最大HPを取得
    public int GetMaxHP(){ return maxHP; }
    //現在のHPを取得
    public int GetHP() { return hp; }


    //ダメージを受ける
    public void OnWeaponHit(int damage)
    {
        print("PlayerDamage");
        hp -= damage;
        uiController.SetHPGage(maxHP, hp);
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

    public void ChangeColor(Color color)
    {
        myMaterial.material.color = color;
    }

    //回避アクション
    private IEnumerator AvoidanceAction()
    {

        ChangeColor(Color.blue);

        float moveTime = 0.3f;

        //回避アクション中は向いている方向に一定数移動
        while (moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            CharacterMove(transform.forward, avoidanceSpeed);
            yield return new WaitForEndOfFrame();
        }

        ChangeColor(Color.white);

        avoidanceAction = null;
    }

    //カメラリグを取得
    public GameObject GetCameraRig()
    {
        return cameraRig;
    }
}
