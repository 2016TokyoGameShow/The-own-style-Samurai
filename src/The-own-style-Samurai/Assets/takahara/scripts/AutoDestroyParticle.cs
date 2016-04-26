using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour {

    //private ParticleSystem ps;


    public void Start()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.startLifetime);
    }

    public void Update()
    {
    }
}
