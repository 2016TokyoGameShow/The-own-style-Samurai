using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    static string gotoSceneName;
    static TweenAlpha tween;
    static List<UITweener> tweens;

    public List<UITweener> enableTweens;

    void Awake()
    {
        tweens = new List<UITweener>(enableTweens);
    }
    
    public static void FadeStart(string sceneName)
    {
        if(tweens[0].enabled)
        {
            return;
        }

        gotoSceneName = sceneName;

        foreach(UITweener tween in tweens)
        {
            tween.enabled = true;
            tween.ResetToBeginning();
            tween.PlayForward();
        }
    }

    public void GotoScene()
    {
        SceneManager.LoadScene(gotoSceneName);
    }
}