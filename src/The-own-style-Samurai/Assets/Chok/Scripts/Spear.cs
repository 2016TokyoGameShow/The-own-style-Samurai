using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Spear : IWeapon
{

    protected override void TriggerEnter(Collider other)
    {
        SendHit(other.gameObject);
    }
}
