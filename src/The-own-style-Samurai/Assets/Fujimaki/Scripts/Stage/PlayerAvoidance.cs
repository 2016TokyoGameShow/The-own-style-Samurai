using UnityEngine;
using System.Collections;

public class PlayerAvoidance : MonoBehaviour {

    [SerializeField, Header("回避スピード")]
    private float avoidanceSpeed;
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject avoidanceEmitter;



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

        player.GetAnimator().SetBool("avoidance", true);

        //player.ChangeColor(Color.blue);
       // player.GetAnimator().SetBool("bow", true);
        player.nonMove = true;
        float moveTime = 1f;


        bool effected = false;

        //回避アクション中は向いている方向に一定数移動
        while (moveTime > 0)
        {
            GameObject g = null;
            if ((moveTime > 0.35f)&&(!effected))
            {
                //g = (GameObject)Instantiate(avoidanceEmitter, transform.position, transform.rotation);
                effected = true;
            }

            moveTime -= Time.deltaTime;
            player.CharacterMove(transform.forward, avoidanceSpeed*Time.deltaTime*30);


            if (g != null)
            {
                g.transform.position = transform.position;
            }
            yield return new WaitForEndOfFrame();
        }

        player.ChangeColor(Color.white);

        player.nonMove = false;
        avoidanceAction = null;

        player.GetAnimator().SetBool("avoidance", false);
    }
}
