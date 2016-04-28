using UnityEngine;
using System.Collections;

public class PlayerAvoidance : MonoBehaviour {

    [SerializeField, Header("回避スピード")]
    private float avoidanceSpeed;
    [SerializeField]
    private Player player;



    private Coroutine avoidanceAction;

	void Start () {
	
	}
	

	void Update () {
        //とりあえずよけるアクション
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((avoidanceAction == null) && (!player.nonMove))
            {
                avoidanceAction = StartCoroutine(AvoidanceAction());
            }
        }
	}

    //回避モードかどうか取得
    public bool GetAvoidance()
    {
        return avoidanceAction == null ? false : true;
    }

    //回避アクション
    private IEnumerator AvoidanceAction()
    {

        player.ChangeColor(Color.blue);
        player.GetAnimator().SetBool("bow", true);
        player.nonMove = true;
        float moveTime = 0.3f;

        //回避アクション中は向いている方向に一定数移動
        while (moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            player.CharacterMove(transform.forward, avoidanceSpeed);
            yield return new WaitForEndOfFrame();
        }

        player.ChangeColor(Color.white);
        player.GetAnimator().SetBool("bow", false);
        player.nonMove = false;
        avoidanceAction = null;
    }
}
