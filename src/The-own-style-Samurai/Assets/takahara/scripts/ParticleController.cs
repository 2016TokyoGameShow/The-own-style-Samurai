using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

    [SerializeField]
    private ParticleSystem[] particle;
    [SerializeField]
    private string[] particleName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    //任意のポジションにパーティクル呼び出し
    public void PlayOnParticle(string name,Vector3 position)
    {
        Debug.Log(name);
        bool isPlayParticle = false;
        for(int i = 0; i < particleName.Length; i++)
        {
            Debug.Log(i);
            if (name == particleName[i])
            {
                Instantiate(particle[i], position, Quaternion.identity);
                isPlayParticle = true;
                break;
            }
        }
        if (isPlayParticle == false)Debug.Log("Error! Not Set Particle or ParticleName"); else Debug.Log("Success!");
    }
}
