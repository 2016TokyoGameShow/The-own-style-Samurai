using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[AddComponentMenu("Enemy/Weapon/IMeleeWeapon")]
public class IMeleeWeapon : MonoBehaviour
{

    [SerializeField, Tooltip("生存秒数")]
    float objectLifeTime;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(Dead());
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        ExecuteEvents.Execute<WeaponHitHandler>(
            other.gameObject,
            null,
            (_object, _event) => { _object.OnWeaponHit(); }
        );

        Destroy(gameObject);

    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(objectLifeTime);
        Destroy(gameObject);
    }
}
