using UnityEngine;
using System.Collections;

public class testParticlePlay : MonoBehaviour {
    public ParticleSystem particle;


    [SerializeField]
    private string particleName;
	// Use this for initialization
	void Start () {
        ParticleController.AddParticle(particleName, particle);
    }
	
	// Update is called once per frame
	void Update () {
       
        if (Input.GetKeyDown(KeyCode.P))
        {
            
            ParticleController.PlayOnParticle(particleName, transform.position);
        }
	}
}
