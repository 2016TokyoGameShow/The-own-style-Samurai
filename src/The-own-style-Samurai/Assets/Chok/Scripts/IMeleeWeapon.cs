using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[AddComponentMenu("Enemy/Weapon/IMeleeWeapon")]
public class IMeleeWeapon : IWeapon
{


    // Use this for initialization
    void Start()
    {
        StartCoroutine(Dead());
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(objectLifeTime);
        Destroy(gameObject);
    }
}
