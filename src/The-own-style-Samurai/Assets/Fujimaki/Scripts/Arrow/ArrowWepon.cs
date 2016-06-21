using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class ArrowWepon : MonoBehaviour
{
    [SerializeField]
    private int speed;

    void Start () {
	
	}
	

	void Update () {
        transform.Translate(transform.forward*Time.deltaTime*speed);
	}

    void OnTriggerEnter(Collider other)
    {

            ExecuteEvents.Execute<WeaponHitHandler>(
                other.gameObject,
                null,
                (_object, _event) => { _object.OnWeaponHit(1, this.gameObject); }
                );

        print(other.name);

     Destroy(this.gameObject,0.1f);
    }
}
