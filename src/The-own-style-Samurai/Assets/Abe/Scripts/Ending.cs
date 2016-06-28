using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ending : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]
    
    public void BGMFade()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for(float t = 0; t <= 3; t += Time.deltaTime)
        {
            AudioManager.ChangeVolume(1 - (t / 3));
            yield return null;
        }
    }
}