using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour,WeaponHitHandler {


    [SerializeField]
    private NavMeshAgent myNavMeshAgetnt;
    [SerializeField]
    private Animator myAnimator;
    [SerializeField]
    private EnemyController enemeyController;

    private Player player;

    private Vector3 saveSpeedVelocity;



	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(Assault());
    }
	
	// Update is called once per frame
	void Update () {


    }
    private IEnumerator Assault()
    {
        while (Vector3.Angle(player.transform.position - transform.position, transform.forward) > 5)
        {

            float angle = Vector3.Angle(player.transform.position - transform.position, transform.forward);

            var relativePos = player.transform.position - transform.position;
            var rotation = Quaternion.LookRotation(relativePos);

            transform.rotation =
              Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);


            yield return new WaitForEndOfFrame();
        }

        myAnimator.SetBool("Assult",true);
        while (true)
        {

            myNavMeshAgetnt.Move(transform.forward*Time.deltaTime*5);

            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position+transform.up, transform.position+transform.up + transform.forward * 5, 1))
            {
                AttackAnimatorEvent();
                break;
            }

                yield return new WaitForEndOfFrame();
        }

        myAnimator.SetBool("Assult", false);
    }

    private IEnumerator Attack()
    {
        myAnimator.SetBool("Walk", true);
        while (Vector3.Distance(transform.position, player.transform.position) > myNavMeshAgetnt.stoppingDistance + 0.5f)
        {
            myNavMeshAgetnt.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.5f);
        }

        myAnimator.SetBool("Attack", true);

        print("stop");
        myAnimator.SetBool("Walk", false);
    }

    public void OnWeaponHit(int damege, GameObject attackObject)
    {

    }

    public void AttackAnimatorEvent()
    {
        myAnimator.SetBool("Attack", false);

        print(Vector3.Distance(transform.position, player.transform.position));

            //距離が一定数離れていたら突撃攻撃
            if (Vector3.Distance(transform.position, player.transform.position) > 5)
        {
            StartCoroutine(WaitStartAssult());
        }
        else
        {
            StartCoroutine(WaitStartTrackPlayer());
        }
    }

    private void Order()
    {
        if (Random.Range(0, 2) == 0)
        {
            //収集命令
        }
        else
        {
            //突撃命令
        }
    }

    private IEnumerator WaitStartAssult()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Assault());
    }

    private IEnumerator WaitStartTrackPlayer()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(Attack());
    }


}
