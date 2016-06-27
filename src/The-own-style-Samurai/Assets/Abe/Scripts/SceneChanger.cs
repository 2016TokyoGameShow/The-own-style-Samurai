using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class SceneChanger : MonoBehaviour
{
    static string gotoSceneName;
    static TweenAlpha tween;
    static List<UITweener> tweens;
    static List<UITweener> disable;

    public List<UITweener> enableTweens;
    public List<UITweener> disableTweens;

    void Awake()
    {
        tweens = new List<UITweener>(enableTweens);
        disable = new List<UITweener>(disableTweens);

        enableTweens.Clear();
        disableTweens.Clear();
    }
    
    public static void FadeStart(string sceneName)
    {
        if(tweens[0].enabled)
        {
            return;
        }

        gotoSceneName = sceneName;

        foreach(UITweener tween in disable)
        {
            tween.enabled = false;
        }

        foreach(UITweener tween in tweens)
        {
            tween.enabled = true;
            tween.ResetToBeginning();
            tween.PlayForward();
        }
    }

    public void GotoScene()
    {
        GC.Collect();
        SceneManager.LoadScene(gotoSceneName);
    }
}