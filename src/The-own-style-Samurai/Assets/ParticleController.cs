using UnityEngine;
using System.Collections;

public static class ParticleController{

    
    public static ParticleSystem[] particle = new ParticleSystem[99];
    
    public static string[] particleName = new string[99];

    public static void AddParticle(string name,ParticleSystem addParticle)
    {
        for(int i = 0; i <= particle.Length; i++)
        {
            if(particle[i] == null)
            {
                particle[i] = addParticle;
                particleName[i] = name;
                Debug.Log("OK.number:" + i);
                break;
            }

        }
    }

    //任意のポジションにパーティクル呼び出し
    public static void PlayOnParticle(string name,Vector3 position)
    {
        Debug.Log(name);
        bool isPlayParticle = false;
        for(int i = 0; i < particleName.Length; i++)
        {
            Debug.Log(i);
            if (name == particleName[i])
            {
                
                GameObject.Instantiate(particle[i], position, Quaternion.identity);
                isPlayParticle = true;
                Debug.Log("Play");
                break;
            }
        }
        if (isPlayParticle == false)Debug.Log("Error! Not Set Particle or ParticleName"); else Debug.Log("Success!");
    }
}
