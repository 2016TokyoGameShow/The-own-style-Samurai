﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Boss : MonoBehaviour,WeaponHitHandler {

    [SerializeField]
    private bool debug;
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
    [SerializeField]
    private GameObject AssultEmitter;
    [SerializeField]
    private GameObject attackEmitter;

    private Vector3 saveSpeedVelocity;

    private bool saveSummonAction;//連続で召喚を行わないためのフラグ

    [SerializeField]
    private int hp;



	// Use this for initialization
	void Start () {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if(debug)
        StartCoroutine(Launch());
        
    }

    public void GetDamage()
    {
        myAnimator.SetBool("damage", false);
        print("false");
    }

    public void BossStart()
    {
        /*player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        StartCoroutine(WaitNextAction(2));*/
    }
    public IEnumerator Launch()
    {
        yield return new WaitForSeconds(3);
        myAnimator.SetBool("launch", true);
        AudioManager.PlaySE("appearBossSE", 1);
        AudioManager.PlayBGM("");
        AudioManager.PlayBGM("bossBgm", 0.5f);

        StartCoroutine(WaitNextAction(2));
        //StartCoroutine(CameraMove());

    }

    //カメラ演出
    private IEnumerator CameraMove()
    {
        float counter = 0;
        float speed = 2;

        player.nonMove = true;

        Vector3 savePosition= player.GetPlayerAttack().mainCamera.transform.position;
        Quaternion saveRotation = player.GetPlayerAttack().mainCamera.transform.rotation;

        player.GetPlayerAttack().mainCamera.GetComponent<DepthOfField>().focalTransform = this.gameObject.transform;

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

        player.GetPlayerAttack().mainCamera.GetComponent<DepthOfField>().focalTransform = null;

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
        }

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

        myAnimator.SetBool("Attack", true);
        myNavMeshAgetnt.Stop();
        myAnimator.SetBool("Walk", false);
    }

    //======================================================================================================ダメージを受ける
    public void OnWeaponHit(int damege, GameObject attackObject)
    {
        hp -= damege;
        myAnimator.SetBool("damage",true);
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

        GameObject ag = (GameObject)Instantiate(attackEmitter, transform.position + transform.forward*4, transform.rotation);
        AudioManager.PlaySE("attackBossSE2", 2);

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
              Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 4);


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

    public void AssultStart()
    {
        StartCoroutine(AssultAttacking());
    }

    private IEnumerator AssultAttacking()
    {

        GameObject attackArea = (GameObject)Instantiate(bossAttackArea, transform.position + transform.forward * 2, transform.rotation);
        attackArea.transform.parent = transform;
        attackArea.GetComponent<BossAttackArea>().Initialize(1);

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        float timer = 0.0f;
        GameObject ag = (GameObject)Instantiate(AssultEmitter, transform.position + Vector3.up * 3, transform.rotation);

        while (timer < 5)
        {
            timer += Time.deltaTime;
            myNavMeshAgetnt.Move(transform.forward * Time.deltaTime * 5);
            ag.transform.position = transform.position + Vector3.up * 2 + transform.forward * 4;
            yield return new WaitForEndOfFrame();
        }

        AttackAnimatorEvent();
        myAnimator.SetBool("Assult", false);
        saveSummonAction = false;
    }

    public void WalkEvent()
    {
        float scale = 3f - Vector3.Distance(player.transform.position, transform.position) / 30.0f;

        AudioManager.PlaySE("BossWalking", scale);
        /*
        switch (Random.Range(0, 3))
        {
            case 0:
                AudioManager.PlaySE("WalkEnemySE01", scale);
                break;
            case 1:
                AudioManager.PlaySE("WalkEnemySE02", scale);
                break;
            case 2:
                AudioManager.PlaySE("WalkEnemySE03", scale);
                break;

        }*/
    }
}
