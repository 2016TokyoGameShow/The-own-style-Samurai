using UnityEngine;
using System.Collections;

public class EndBoss : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private bool boss;

    private NavMeshAgent navAgent;

	void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        animator.SetBool("launch", true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Run()
    {
        if (boss)
        {
            animator.SetBool("damage", true);
            yield return new WaitForSeconds(0.2f);
            animator.SetBool("damage", false);
        }

        yield return new WaitForSeconds(2);

        navAgent.SetDestination(transform.position - Vector3.forward * 15);

        if (boss)
        {
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("run", true);
        }


        yield return new WaitForSeconds(1);
    }

    public void FallCompleat()
    {

    }

    public void GetDamage()
    {

    }

    public void FallArrive()
    {

    }
}
