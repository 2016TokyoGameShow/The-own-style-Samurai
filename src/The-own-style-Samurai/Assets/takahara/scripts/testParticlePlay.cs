using UnityEngine;
using System.Collections;

public class testParticlePlay : MonoBehaviour {

    [SerializeField]
    private StageController stageController;

    private ParticleController particleController;
    [SerializeField]
    private string particleName;
	// Use this for initialization
	void Start () {
        particleController = stageController.particleController;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            particleController.PlayOnParticle(particleName, transform.position);
        }
	}
}
