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
        StartCoroutine(BGMFade());
    }

    private IEnumerator BGMFade()
    {
        float timer = 1;
        while (AudioManager.GetVolume() > 0)
        {
            AudioManager.ChangeVolume(AudioManager.GetVolume() - Time.deltaTime/40.0f);
            yield return new WaitForEndOfFrame();
        }
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

    public void WalkEvent()
    {
        float scale = 1;

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
    }
}
