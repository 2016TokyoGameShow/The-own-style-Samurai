// ----- ----- ----- ----- -----
//
// IWeapon
//
// 作成日：2016/04/21
// 作成者：阿部
//
// <概要>
// 武器の基底クラスです
//
// ----- ----- ----- ----- -----

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Enemy/Weapon/IWeapon")]
public class IWeapon : MonoBehaviour
{
    [SerializeField]
    protected int damage;

    [SerializeField, Tooltip("オブジェクトの生存時間")]
    protected float objectLifeTime;

    void Start()
    {
        Attack();
    }

    public void OnTriggerEnter(Collider other)
    {
        TriggerEnter(other);
    }

    protected virtual void TriggerEnter(Collider other)
    {
        SendHit(other.gameObject);

        Destroy(gameObject);
    }

    void Attack()
    {
        WeaponStart();
        StartCoroutine(WeaponUpdate());
    }

    protected void SendHit(GameObject obj)
    {
        ExecuteEvents.Execute<WeaponHitHandler>(
            obj,
            null,
            (_object, _event) => { _object.OnWeaponHit(damage); }
        );
    }

    protected virtual void WeaponStart()
    {

    }

    protected virtual IEnumerator WeaponUpdate()
    {
        yield return new WaitForSeconds(objectLifeTime);
        Destroy(gameObject);
    }
}