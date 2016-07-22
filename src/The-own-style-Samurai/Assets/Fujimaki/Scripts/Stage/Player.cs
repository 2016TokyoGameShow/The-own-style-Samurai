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
    [SerializeField]
    private GameObject hitEmitter;
    [SerializeField]
    private GameObject fallEmitter;
    [SerializeField]
    private GameObject legEmitter;

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

        AudioManager.PlayBGM("nomalBgm",0.4f);
    }

    public PlayerAttack GetPlayerAttack()
    {
        return playerAttack;
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

                CharacterMove(moveVelocity, speed*Time.deltaTime*60);

                transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
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
        AudioManager.PlaySE("DamageSE");
        playerAttack.Hit(damage);
        animator.SetBool("damage", true);
        Instantiate(hitEmitter, transform.position+Vector3.up*1.5f, Quaternion.identity);
        
        hp -= damage;
        uiController.SetHPGage(maxHP, hp);

        if (hp <= 0)
        {
            animator.SetBool("die", true);
        }
    }

    public void GetDamage()
    {
        animator.SetBool("damage", false);
        print("false");
    }

    //攻撃してくる敵をセット
    public void SetTarget(GameObject g)
    {
        uiController.SpawnWarringSlphas(g);
        playerAttack.SetEnemyTarget(g);
    }
    //プレイヤーが攻撃しているか
    public bool isPlayerAttacking()
    {
        return playerAttack.getEnemyTarget();
    }

    //プレイヤーの流している向きを取得
    public Vector3 isAttackingVelocity()
    {
        return playerAttack.playerAttackingVelocity;
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

    //歩行音
    public void WalkFoot()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                AudioManager.PlaySE("walk01");
                break;
            case 1:
                AudioManager.PlaySE("walk02");
                break;
            case 2:
                AudioManager.PlaySE("walk03");
                break;

        }

        Instantiate(legEmitter, transform.position, Quaternion.identity);

    }

    public void FallEvent()
    {
        Instantiate(fallEmitter, transform.position, Quaternion.identity);
        AudioManager.PlaySE("dropSE");
    }
}
