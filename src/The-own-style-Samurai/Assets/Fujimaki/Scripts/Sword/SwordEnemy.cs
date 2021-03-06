﻿using UnityEngine;
using System.Collections;

public class SwordEnemy : MonoBehaviour ,WeaponHitHandler {


    [SerializeField]
    private int distance;
    [SerializeField]
    private GameObject attackArea;
    [SerializeField]
    private GameObject hitEmitter;
    [SerializeField]
    private GameObject hitLocation;
    [SerializeField]
    private GameObject dieEmitter;
    [SerializeField]
    private GameObject dieSmokeEmitter;
    [SerializeField]
    private GameObject fallEmitter;
    [SerializeField]
    private GameObject legEmitter;

    private Renderer myRenderer;

    private Animator animator;
    private NavMeshAgent navAgent;

    private Player player;
    private EnemyControllerF enemyControllerF;
    private CapsuleCollider myCapsule;

    private bool starting;
    private bool stenby;
    private bool removeMode;

    private bool die;

    private Vector3 savePostion;
    private float fleezeTime;

    public float myAngle;
    public bool arrivedStenbyPosition;

    void Start()
    {
        myCapsule = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").GetComponent<Player>();
        enemyControllerF = GameObject.Find("EnemyControllerF").GetComponent<EnemyControllerF>();

        enemyControllerF.AddEnemy(this);

        stenby = true;
        distance += Random.Range(-2, 2);

        myAngle = Random.Range(0, 360);

    }

    public void FallArrive()
    {
        Instantiate(fallEmitter, transform.position, Quaternion.identity);
        AudioManager.PlaySE("dropSE",1 - Vector3.Distance(player.transform.position, transform.position) / 30.0f);
    }

    public void FallCompleat()
    {
        starting = true;
    }
	
	void Update () {

        if (starting)
        {
            if (!die)
            {
                //===============================================================================待機中
                if (stenby)
                {
                    //スタンバイポジションへ移動
                    Vector3 targetPosition = Quaternion.AngleAxis(myAngle, Vector3.up) * (Vector3.forward * distance);
                    targetPosition += player.transform.position;
                    print("moving");

                    if (Vector3.Distance(targetPosition, transform.position) > 0.5f)
                    {
                        arrivedStenbyPosition = false;
                        if (navAgent != null)
                        {
                            navAgent.Resume();
                            navAgent.SetDestination(targetPosition);
                        }

                        animator.SetBool("run", true);
                    }
                    else
                    {
                        print("stemby");
                        //スタンバイポジションについたらプレイヤーのほうを向く
                        var relativePos = player.transform.position - transform.position;
                        var rotation = Quaternion.LookRotation(relativePos);

                        transform.rotation =
                               Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);

                        if (Vector3.Angle(player.transform.position - transform.position, transform.forward) < 10)
                        {
                            arrivedStenbyPosition = true;
                            animator.SetBool("run", false);
                        }
                    }
                }
                else
                {
                    //============================================================================攻撃中
                    if (!removeMode)
                    {

                        //プレイヤーのちょっと手前で止まるようにする
                        Vector3 targetPosition = (player.transform.position - transform.position).normalized;
                        navAgent.SetDestination(player.transform.position - (targetPosition * 2.5f));
                        animator.SetBool("run", true);
                        if (navAgent != null)
                        {
                            if (Vector3.Distance(transform.position, player.transform.position) < 3f)
                            {
                                navAgent.Stop();
                                animator.SetBool("attack", true);
                            }
                            else
                            {
                                navAgent.Resume();
                            }
                        }
                    }else
                    {
                    }
                }
            }
        }
    }
    //=============================攻撃開始
    public void AttackStartAnimatorEvent()
    {
        player.SetTarget(this.gameObject);

        AudioManager.PlaySE("swordAttackSE", 1);
    }

    //=============================攻撃する瞬間
    public void AttackAnimatorEvent()
    {
        animator.SetBool("attack", false);

        if (player.isAttackingVelocity()!=Vector3.zero)
        {
            PlayReciveAnimation(player.isAttackingVelocity().x > 0 ? true : false);
        }
        else
        {
            StartCoroutine(OutMove());
            Instantiate(attackArea, transform.position + transform.up + transform.forward*3, transform.rotation);
        }
    }

    //=========================受け流される
    public void PlayReciveAnimation(bool right)
    {
        Instantiate(hitEmitter, hitLocation.transform.position, Quaternion.identity);

        savePostion = transform.position;
        myCapsule.enabled = false;
        animator.SetBool(right? "reciveRight": "reciveLeft", true);
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
    }

    public void ReciveEnd()
    {
        print("recive");
        animator.SetBool("attack", false);
        animator.SetBool("reciveRight", false);
        animator.SetBool("reciveLeft", false);

        enemyControllerF.RemoveEnemy(this);

        myCapsule.enabled = false;
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }
        die = true;
        StartCoroutine(DieCounter());
    }

    private IEnumerator DieCounter()
    {
        yield return new WaitForSeconds(2);
        Instantiate(dieEmitter, hitLocation.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    //=====================================スタンバイポジションを更新
    public void SetStenbyPosition(float angle)
    {
        myAngle = angle;
    }

    public bool GetStenby()
    {
        return stenby;
    }

    public void SetStenby(bool set)
    {
        stenby = set;
    }

    public void GetOut()
    {
        myAngle += Random.Range(-10, 10) > 0 ? 30 : -30;
    }

    //=================================元の場所に戻る
    private IEnumerator OutMove()
    {
        float timer = 0;
        if (!die)
        {
            removeMode = true;

            myAngle = Random.Range(0, 360);

            Vector3 targetPosition = Quaternion.AngleAxis(myAngle, Vector3.up) * (Vector3.forward * distance);
            targetPosition += player.transform.position;

            yield return new WaitForSeconds(1);

            if (navAgent != null)
            {
                navAgent.Resume();
                navAgent.SetDestination(targetPosition);
            }

            animator.SetBool("run", true);


            while (Vector3.Distance(targetPosition, transform.position) > 0.5f)
            {
                timer += Time.deltaTime;
                if (timer > 5)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            stenby = true;
            removeMode = false;
        }
    }

    public void OnWeaponHit(int damege, GameObject attackObject)
    {
        AudioManager.PlaySE("ArrowHitEnemySE", 1 - Vector3.Distance(player.transform.position, transform.position) / 30.0f);
        enemyControllerF.RemoveEnemy(this);
        Instantiate(hitEmitter, hitLocation.transform.position, Quaternion.identity);
        animator.SetBool("dead", true);
        die = true;
        myCapsule.enabled = false;
        navAgent.enabled = false;
        navAgent = null;
        StartCoroutine(DieCounter());
    }

    public void WalkEvent()
    {

        float scale = 1.5f - Vector3.Distance(player.transform.position, transform.position) / 30.0f;
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

        }

        Instantiate(legEmitter, transform.position, Quaternion.identity);
    }
}
