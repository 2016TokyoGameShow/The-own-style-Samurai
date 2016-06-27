using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelayFadeIn : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
    List<UITweener> tweens;

    void Awake()
    {
        
    }
    
    IEnumerator Start()
    {
        yield return null;
        foreach(UITweener tween in tweens)
        {
            tween.enabled = true;
            tween.ResetToBeginning();
	        tween.PlayForward();
        }
    }
    
    void Update()
    {
        
    }
}