using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour,WeaponHitHandler {


    [SerializeField]
    private NavMeshAgent myNavMeshAgetnt;
    [SerializeField]
    private Animator myAnimator;
    [SerializeField]
    private EnemyController enemeyController;
    [SerializeField]
    private GameObject bossAttackArea;
    [SerializeField]
    private GameObject summonGameObject;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject cameraZoomObject;

    private Vector3 saveSpeedVelocity;

    private bool saveSummonAction;//連続で召喚を行わないためのフラグ

    private int hp;



	// Use this for initialization
	void Start () {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        StartCoroutine(Launch());
        
    }
	
    public void BossStart()
    {
        /*player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        StartCoroutine(WaitNextAction(2));*/
    }
    private IEnumerator Launch()
    {
        yield return new WaitForSeconds(1);
        myAnimator.SetBool("launch", true);
        StartCoroutine(CameraMove());

    }

    //カメラ演出
    private IEnumerator CameraMove()
    {
        float counter = 0;
        float speed = 2;

        player.nonMove = true;

        Vector3 savePosition= player.GetPlayerAttack().mainCamera.transform.position;
        Quaternion saveRotation = player.GetPlayerAttack().mainCamera.transform.rotation;

        while (counter < 1)
        {
            counter += Time.deltaTime * speed;
            player.GetPlayerAttack().mainCamera.transform.position = Vector3.Lerp(savePosition, cameraZoomObject.transform.position, counter);
            player.GetPlayerAttack().mainCamera.transform.rotation = Quaternion.Lerp(saveRotation, cameraZoomObject.transform.rotation, counter);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(4f);

        while (counter > 0)
        {
            counter -= Time.deltaTime * speed;
            player.GetPlayerAttack().mainCamera.transform.position = Vector3.Lerp(savePosition, cameraZoomObject.transform.position, counter);
            player.GetPlayerAttack().mainCamera.transform.rotation = Quaternion.Lerp(saveRotation, cameraZoomObject.transform.rotation, counter);
            yield return new WaitForEndOfFrame();
        }
        player.nonMove = false;

        StartCoroutine(WaitNextAction(2));
    }

    //======================================================================================================突撃攻撃
    private IEnumerator Assault()
    {
        //プレイヤーのほうに向く
        while (Vector3.Angle(player.transform.position - transform.position, transform.forward) > 5)
        {

            float angle = Vector3.Angle(player.transform.position - transform.position, transform.forward);

            var relativePos = player.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(relativePos);

            transform.rotation =
              Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);


            yield return new WaitForEndOfFrame();
        }

        myAnimator.SetBool("Assult", true);
        //抜刀アニメーション待機
        yield return new WaitForSeconds(1);

        GameObject attackArea = (GameObject)Instantiate(bossAttackArea, transform.position + transform.forward * 2, transform.rotation);
        attackArea.transform.parent = transform;
        attackArea.GetComponent<BossAttackArea>().Initialize(0);

        while (true)
        {
            myNavMeshAgetnt.Move(transform.forward*Time.deltaTime*5);

            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            RaycastHit hit;

            //壁にぶつかったら終了
            if (Physics.Raycast(transform.position+transform.up, transform.position+transform.up + transform.forward,out hit,3))
            {
                if (hit.collider.tag != "Enemy")
                {
                    attackArea.GetComponent<BossAttackArea>().DestroyObject();
                    break;
                }
            }

                yield return new WaitForEndOfFrame();
        }

        AttackAnimatorEvent();
        myAnimator.SetBool("Assult", false);
        saveSummonAction = false;
    }

    //======================================================================================================追いかけ攻撃
    private IEnumerator Attack()
    {
        myNavMeshAgetnt.Resume();
        myAnimator.SetBool("Walk", true);
        while (Vector3.Distance(transform.position, player.transform.position) > myNavMeshAgetnt.stoppingDistance + 0.5f)
        {
            myNavMeshAgetnt.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.5f);
            print("Attack Loop");
        }

        myAnimator.SetBool("Attack", true);
        myNavMeshAgetnt.Stop();
        myAnimator.SetBool("Walk", false);
    }

    //======================================================================================================ダメージを受ける
    public void OnWeaponHit(int damege, GameObject attackObject)
    {
        hp -= damege;
        if (hp <= 0)
        {
            myAnimator.SetBool("die", true);
        }
    }

    //======================================================================================================攻撃処理
    public void AttackAnimatorEvent()
    {
        myAnimator.SetBool("Attack", false);
        GameObject attackArea = (GameObject)Instantiate(bossAttackArea, transform.position + transform.forward * 3, transform.rotation);
        attackArea.GetComponent<BossAttackArea>().Initialize(1);

        StartCoroutine(WaitNextAction(2));

    }

    //======================================================================================================弓兵を召喚処理
    public void CallEnemyAnimatorEvent()
    {
        //3体償還
        for (int i = 0; i < 3; i++)
        {
            Instantiate(summonGameObject, transform.position + transform.up * 2 + transform.forward * 3 + (transform.right * (i - 1) * 2), transform.rotation);
        }

        myAnimator.SetBool("call", false);
        StartCoroutine(WaitNextAction(2));
    }

    //======================================================================================================弓兵を償還
    private IEnumerator Summon()
    {
        saveSummonAction = true;
        //プレイヤーのほうに向く
        while (Vector3.Angle(player.transform.position - transform.position, transform.forward) > 5)
        {

            float angle = Vector3.Angle(player.transform.position - transform.position, transform.forward);

            var relativePos = player.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(relativePos);

            transform.rotation =
              Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);


            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        myAnimator.SetBool("call", true);

    }

    //======================================================================================================次のアクションを決定
    private IEnumerator WaitNextAction(float time)
    {
        yield return new WaitForSeconds(time);

        //距離が一定数離れていたら突撃攻撃
        if (Vector3.Distance(transform.position, player.transform.position) > 7)
        {
            StartCoroutine(Assault());
            /*
            if ((Random.Range(0, 2) == 0)&&(!saveSummonAction))
            {
                StartCoroutine(Summon());
            }
            else
            {
                StartCoroutine(Assault());
            }*/
        }
        else
        {
            StartCoroutine(Attack());
        }
    }
}
