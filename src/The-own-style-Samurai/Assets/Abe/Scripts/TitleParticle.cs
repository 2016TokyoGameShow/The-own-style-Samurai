using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleParticle : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particle;

    [SerializeField]
    Vector3 from;

    [SerializeField]
    Vector3 to;

    [SerializeField]
    UITexture texture;
    
    [SerializeField]
    float deleyTime;

    [SerializeField, Tooltip("説明文")]
    float time;

    [SerializeField]
    MonoBehaviour[] enableObject;

    [SerializeField]
    TweenAlpha activeObject;

    bool previousKey = false;

    void Awake()
    {
         
    }
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(deleyTime);
        particle.time = 0;
        particle.Simulate(10.0f);
        particle.Play();
        
        for(float t = 0; t <= time; t += Time.deltaTime)
        {
            float amount = t / time;
            texture.fillAmount = amount;
            particle.transform.localPosition = Vector3.Lerp(from, to, amount);
            yield return null;
        }

        StartCoroutine(Delete());
    }
    
    void LateUpdate()
    {
        if(!Input.anyKey && previousKey) //anyKeyUpが無いため
        {
            StopAllCoroutines();
            texture.fillAmount = 1;
            particle.Stop();
            StartCoroutine(DelayEnable());
            StartCoroutine(Delete());
        }

        previousKey = Input.anyKey;
    }

    IEnumerator DelayEnable()
    {
        yield return null;
        foreach(MonoBehaviour obj in enableObject)
        {
            obj.enabled = true;
        }
        activeObject.enabled = true;
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(particle.startLifetime + 0.01f);

        Destroy(particle.gameObject);

        foreach(MonoBehaviour obj in enableObject)
        {
            obj.enabled = true;
        }
        activeObject.enabled = true;
        Destroy(this);
    }
}