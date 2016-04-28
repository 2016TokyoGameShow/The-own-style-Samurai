using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour,WeaponHitHandler {
    
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
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerAttack playerAttack;
    [SerializeField]
    private PlayerAvoidance playerAvoidance;

    private int hp;
    private float finisherGageValue;

    private UIController uiController;
    private Vector3 saveMoveVelocity;
    private Vector3 targetVelocity;
    public bool nonMove;



    [SerializeField]
	private Renderer myMaterial;

	void Start () {
        uiController = stageController.uiController;
        hp = maxHP;
	}

	void Update () {

        //枚フレームちょっとだけ必殺ゲージを貯める
        finisherGageValue += Time.deltaTime / 30;
        finisherGageValue = Mathf.Clamp(finisherGageValue, 0, 1);
        uiController.SetFinisherGage(finisherGageValue);


        Vector3 moveVelocity = Vector3.zero;

        //入力から移動ベクトルを計算して移動
        if (!nonMove)
        {
            moveVelocity = cameraRig.transform.forward * Input.GetAxis("Vertical");
            moveVelocity += cameraRig.transform.right * Input.GetAxis("Horizontal");
            CharacterMove(moveVelocity, speed);
        }
	}
    //最大HPを取得
    public int GetMaxHP(){ return maxHP; }
    //現在のHPを取得
    public int GetHP() { return hp; }


    //ヒット通知
    public void OnWeaponHit(int damage,GameObject enemy)
    {
        print("PlayerDamage");
        playerAttack.Hit(damage);
        
        hp -= damage;
        uiController.SetHPGage(maxHP, hp);
    }

    //攻撃してくる敵をセット
    public void SetTarget(GameObject g)
    {
        playerAttack.SetEnemyTarget(g);
    }
    //プレイヤーが攻撃しているか
    public bool isPlayerAttacking()
    {
        return playerAttack.getEnemyTarget();
    }

    //キャラクター移動
    public void CharacterMove(Vector3 moveVelocity,float speed)
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

    //メッシュの色変更
    public void ChangeColor(Color color)
    {
        myMaterial.material.color = color;
    }

    //カメラリグを取得
    public GameObject GetCameraRig()
    {
        return cameraRig;
    }

    //アニメーター取得
    public Animator GetAnimator()
    {
        return animator;
    }
}
