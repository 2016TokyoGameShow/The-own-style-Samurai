using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTweenStart : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
	List<UITweener> randomTweenStart, activeTween;


	[SerializeField]
	float minRandom, maxRandom;

    void Awake()
    {
        
    }
    
    void Start()
    {
        foreach(UITweener tween in randomTweenStart)
		{
			tween.delay   = Random.Range(minRandom, maxRandom);
			tween.enabled = true;
		}

		foreach(UITweener tween in activeTween)
		{
			tween.enabled = true;
		}
    }
    
    void Update()
    {
        
    }
}