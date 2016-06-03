using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject enemyTarget;

    public bool playerAttacking;

    void Start() {

    }

    void Update() {

      /*  if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Attack(player.GetCameraRig().transform.forward,Vector3.up));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Attack(-player.GetCameraRig().transform.forward,-Vector3.up));
        }*/


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Attack(player.GetCameraRig().transform.right,Vector3.right));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Attack(-player.GetCameraRig().transform.right,-Vector3.right));
        }
    }

    //流し攻撃
    private IEnumerator Attack(Vector3 velocity,Vector3 localVelocity) {



        if ((!playerAttacking) && (enemyTarget != null))
        {
            //player.GetAnimator().SetInteger("katana", 1);
            player.UpFinisherGage(0.1f);

            player.ChangeColor(Color.red);
            player.nonMove = true;
            playerAttacking = true;

            Vector3 enemyTargetPositon = enemyTarget.transform.position;

            while (Vector3.Angle(player.transform.forward, enemyTargetPositon - player.transform.position) != 0)
            {

                Quaternion rotation = Quaternion.LookRotation(enemyTargetPositon - player.transform.position);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 0.5f);
                //player.transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
                yield return new WaitForEndOfFrame();
            }


            //player.GetAnimator().SetBool("Katana",localVelocity.x)
            //流す方向に向く
            while (Vector3.Angle(player.transform.forward, velocity) > 0.1f)
            {
                Quaternion rotation = Quaternion.LookRotation(velocity);

                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 0.2f);
                yield return new WaitForEndOfFrame();
            }

            if(enemyTarget != null)Attack();

            yield return new WaitForSeconds(0.5f);

            playerAttacking = false;
            player.ChangeColor(Color.white);
            player.nonMove = false;

           // player.GetAnimator().SetInteger("katana", 0);
        }
    }

    //ダメージを受ける
    public void Hit(int damage)
    {
        print("PlayerDamage");
    }

    //攻撃してくるターゲットを指定
    public void SetEnemyTarget(GameObject g)
    {
        enemyTarget = g;
    }

    public GameObject GetEnemyTarget()
    {
        return enemyTarget;
    }


    public bool getEnemyTarget(){
        return enemyTarget == null;
    }

    private void Attack()
    {
        RaycastHit hit;

        if (Physics.Raycast(player.transform.position, player.transform.forward * 10, out hit))
       {
           print("hit");
           if (hit.collider.gameObject != enemyTarget)
           {
               ExecuteEvents.Execute<WeaponHitHandler>(
                enemyTarget.gameObject,
                null,
                (_object, _event) => { _object.OnWeaponHit(1, this.gameObject); }
                );
           }
       }

       enemyTarget = null;
    }
}
