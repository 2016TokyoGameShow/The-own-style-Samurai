using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {

    [SerializeField]
    private Player player;

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




        if (!playerAttacking)
        {
            player.GetAnimator().SetInteger("katana", 1);

            player.ChangeColor(Color.red);
            player.nonMove = true;
            playerAttacking = true;

            //流す方向に向く
            while (Vector3.Angle(player.transform.forward, velocity) != 0)
            {
                Quaternion rotation = Quaternion.LookRotation(velocity);
                player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rotation, 0.2f);
                yield return new WaitForEndOfFrame();
            }

            playerAttacking = false;
            player.ChangeColor(Color.white);
            player.nonMove = false;

            player.GetAnimator().SetInteger("katana", 0);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (playerAttacking)
        {
            ExecuteEvents.Execute<WeaponHitHandler>(
                other.gameObject,
                null,
                (_object, _event) => { _object.OnWeaponHit(1); }
                );
        }
    }
}
