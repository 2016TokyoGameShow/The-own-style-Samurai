using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SwordAttackArea : MonoBehaviour {


    public int attackPoint;
	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 0.2f);
	}
	

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            ExecuteEvents.Execute<WeaponHitHandler>(
                other.gameObject,
                null,
                (_object, _event) => { _object.OnWeaponHit(attackPoint, this.gameObject); }
                );
                Destroy(this.gameObject);
        }
    }
}
