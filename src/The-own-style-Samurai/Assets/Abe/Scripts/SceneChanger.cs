using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneChanger : MonoBehaviour
{
    static string gotoSceneName;
    static TweenAlpha tween;

    void Awake()
    {
        tween = gameObject.GetComponent<TweenAlpha>();
    }
    
    public static void FadeStart(string sceneName)
    {
        if(tween.enabled)
        {
            return;
        }

        gotoSceneName = sceneName;
        tween.enabled = true;
        tween.ResetToBeginning();
        tween.PlayForward();
    }

    public void GotoScene()
    {
        SceneManager.LoadScene(gotoSceneName);
    }
}