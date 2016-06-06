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

    private UIController uiController;
    private Vector3 saveMoveVelocity;
    private Vector3 targetVelocity;
    public bool nonMove;

    private float finisherGage;



    [SerializeField]
	private Renderer myMaterial;

	void Start () {
        uiController = stageController.uiController;
        UpFinisherGage(0);
        hp = maxHP;
	}

	void Update () {

        if (hp > 0)
        {
            Vector3 moveVelocity = Vector3.zero;

            //入力から移動ベクトルを計算して移動
            if (!nonMove)
            {
                moveVelocity = cameraRig.transform.forward * Input.GetAxis("Vertical");
                moveVelocity += cameraRig.transform.right * Input.GetAxis("Horizontal");

                animator.SetBool("walk", moveVelocity != Vector3.zero ? true : false);

                CharacterMove(moveVelocity, speed);
            }
        }
	}
    //最大HPを取得
    public int GetMaxHP(){ return maxHP; }
    //現在のHPを取得
    public int GetHP() { return hp; }

    //プレイヤーが流し中かどうか返す
    public bool GetPlayerAttacking()
    {
        if (playerAttack.playerAttackingOnce)
        {
            playerAttack.playerAttackingOnce = false;
            return true;
        }
        return false;
    }

    //現在攻撃を仕掛けている敵(エネミーターゲット)を取得
    public GameObject GetEnemyTarget()
    {
        return playerAttack.GetEnemyTarget();
    }

    //ヒット通知
    public void OnWeaponHit(int damage,GameObject enemy)
    {
        playerAttack.Hit(damage);
        
        hp -= damage;
        uiController.SetHPGage(maxHP, hp);

        if (hp <= 0)
        {
            animator.SetBool("die", true);
        }
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

    //必殺ゲージを上昇
    public void UpFinisherGage(float value)
    {
        finisherGage += value;
        uiController.SetFinisherGage(finisherGage);
    }
}
