using UnityEngine;
using System.Collections;

public class EndManager : MonoBehaviour {

    [SerializeField]
    private EndBoss[] targets;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject cameraObject;

    private bool end;
    private float movement;

	void Start () {
	
	}
	
    public void End()
    {

        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        player.nonMove = true;
        player.GetPlayerAttack().end = true;

        yield return new WaitForSeconds(1);

        foreach (var t in targets)
        {
            StartCoroutine(t.Run());
        }

        player.GetPlayerAttack().mainCamera.transform.position = cameraObject.transform.position;
        player.GetPlayerAttack().mainCamera.transform.rotation = cameraObject.transform.rotation;

        yield return new WaitForSeconds(2);

        end = true;
    }

	void Update () {

        if (end)
        {
            movement += Time.deltaTime*1.5f;
            player.GetPlayerAttack().mainCamera.transform.position = cameraObject.transform.position-new Vector3(0,0,movement);
            player.GetPlayerAttack().mainCamera.transform.rotation = cameraObject.transform.rotation;

            if (movement > 5)
            {
                TrueEnd();
            }
        }
    }

    private void TrueEnd()
    {
        end = false;
    }
}
