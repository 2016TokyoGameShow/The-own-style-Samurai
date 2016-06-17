using UnityEngine;
using System.Collections;

public class ParticleDestroyer : MonoBehaviour {

    private ParticleSystem particleSystem;

	void Start () {
        particleSystem = GetComponent<ParticleSystem>();

        Destroy(this.gameObject,particleSystem.duration+3);
	}
	

	void Update () {
	
	}
}
