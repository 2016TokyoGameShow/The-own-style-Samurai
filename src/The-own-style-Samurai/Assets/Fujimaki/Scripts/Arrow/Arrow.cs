using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    private int distance;
    [SerializeField]
    private ArrowLine arrowLine;
    [SerializeField]
    private GameObject arrowWepon;
    [SerializeField]
    private GameObject fallEmitter;

    private Player player;
    private EnemyControllerF enemyControllerF;
    private CapsuleCollider myCapsule;

    private Animator animator;
    private NavMeshAgent navAgent;

    private int randomAngle;
    private int fallCount;
    private bool once;

    void Start()
    {
        myCapsule = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").GetComponent<Player>();
        enemyControllerF = GameObject.Find("EnemyControllerF").GetComponent<EnemyControllerF>();

        enemyControllerF.AddArrow(this);

    }


    void Update()
    {

    }

    public void FallArrive()
    {
        Instantiate(fallEmitter, transform.position, Quaternion.identity);
    }

    public void FallCompleat()
    {
        Instantiate(fallEmitter, transform.position, Quaternion.identity);
        StartCoroutine(Move());
        fallCount++;
        if (fallCount == 2)
        {
            Destroy(this.gameObject, 2);
        }
    }

    private IEnumerator Move()
    {
        randomAngle = Random.Range(-20, 20);
        while (true)
        {
            //スタンバイポジションへ移動
            Vector3 targetPosition = Quaternion.AngleAxis(randomAngle, Vector3.up) * (Vector3.forward * distance);
            targetPosition += player.transform.position;


            if (Vector3.Distance(targetPosition, transform.position) > 0.5f)
            {
                navAgent.Resume();
                navAgent.SetDestination(targetPosition);

                animator.SetBool("run", true);
            }
            else
            {

                StartCoroutine(Attack());
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Attack()
    {
        navAgent.Stop();

        if (!once)
        {
            once = true;


            while (Vector3.Angle(player.transform.position - transform.position, transform.forward) > 1f)
            {
                //スタンバイポジションについたらプレイヤーのほうを向く
                var relativePos = player.transform.position - transform.position;
                var rotation = Quaternion.LookRotation(relativePos);

                transform.rotation =
                       Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 3);
                print(Vector3.Angle(player.transform.position - transform.position, transform.forward));
                yield return new WaitForEndOfFrame();
            }
            StartCoroutine(arrowLine.animateLineRenderer());
            animator.SetBool("run", false);

            animator.SetBool("shoot", true);
            yield return new WaitForSeconds(1.5f);

            Instantiate(arrowWepon, transform.position + transform.forward * 2 + transform.up * 1.5f, transform.rotation);
            animator.SetBool("shoot", false);
            yield return new WaitForSeconds(0.5f);


            StartCoroutine(arrowLine.endLine());
            animator.SetBool("getout", true);
            enemyControllerF.RemoveArrow(this);
        }
    }
}
