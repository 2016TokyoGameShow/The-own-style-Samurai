using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BossAttackArea : MonoBehaviour {

    [SerializeField]
    private int attackPoint;
    private float destroyTime;

    public void Initialize(float destroyTime)
    {
        this.destroyTime = destroyTime;
        if (destroyTime != 0)
        {
            StartCoroutine(DieCount());
        }
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
            if (destroyTime != 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator DieCount()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
