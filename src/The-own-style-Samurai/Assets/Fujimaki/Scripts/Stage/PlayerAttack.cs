using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField]
    private Player player;

    private Enemy enemyTarget;

    public bool playerAttacking;

	void Start () {
	
	}
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Attack(player.GetCameraRig().transform.forward));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Attack(-player.GetCameraRig().transform.forward));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Attack(player.GetCameraRig().transform.right));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Attack(-player.GetCameraRig().transform.right));
        }
	}

    //流し攻撃
    private IEnumerator Attack(Vector3 velocity){


        if ((!playerAttacking) && (enemyTarget != null)) 
        {
            player.GetAnimator().SetInteger("katana", 1);

            player.ChangeColor(Color.red);
            player.nonMove = true;
            playerAttacking = true;


            while (Vector3.Angle(player.transform.forward, enemyTarget.transform.position - player.transform.position) != 0)
            {
                Quaternion rotation = Quaternion.LookRotation(enemyTarget.transform.position - player.transform.position);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 0.5f);
                yield return new WaitForEndOfFrame();
            }

            //流す方向に向く
            while (Vector3.Angle(player.transform.forward, velocity) != 0)
            {
                Quaternion rotation = Quaternion.LookRotation(velocity);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 0.2f);
                yield return new WaitForEndOfFrame();
            }

            Attack();

            playerAttacking = false;
            player.ChangeColor(Color.white);
            player.nonMove = false;

            player.GetAnimator().SetInteger("katana", 0);
        }
    }

    //ダメージを受ける
    public void Hit(int damage,Enemy enemy)
    {
        enemyTarget = enemy;
        print("PlayerDamage");
    }

    private void Attack()
    {

       if (Physics.Raycast(player.transform.position, player.transform.forward * 10, 10))
       {
           print("hit");

           ExecuteEvents.Execute<WeaponHitHandler>(
            enemyTarget.gameObject,
            null,
            (_object, _event) => { _object.OnWeaponHit(1); }
            );
       }
    }
}
