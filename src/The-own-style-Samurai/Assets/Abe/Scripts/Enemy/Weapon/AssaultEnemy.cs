using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("MyScript/AssaultEnemy")]
public class AssaultEnemy: IWeapon
{
    #region 変数

    //[SerializeField, Tooltip("説明文")]

    float speed = 3.0f;
    
    Transform player;
    Vector3   dir;

    #endregion

    #region プロパティ

    #endregion

    #region メソッド

    protected override void WeaponStart()
    {
        GetComponent<Collider>().isTrigger = true;
        Rigidbody rig = gameObject.AddComponent<Rigidbody>();
        rig.useGravity = false;
        rig.constraints = RigidbodyConstraints.FreezeAll;

        player = EnemyController.singleton.player.transform;
        dir = player.position - transform.position;
        dir.y = 0;
        dir.Normalize();
    }

    protected override IEnumerator WeaponUpdate()
    {
        while(true)
        {
            Quaternion playerAngle = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, playerAngle, 120 * Time.deltaTime);
            transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }
    }

    protected override void TriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            bool isAvoidance = other.GetComponent<PlayerAvoidance>().GetAvoidance();
            if(isAvoidance)
            {
                return;
            }
        }
        else if(other.tag == "Wall")
        {
            Destroy(gameObject);
        }
        SendHit(other.gameObject);
    }
    #endregion
}
