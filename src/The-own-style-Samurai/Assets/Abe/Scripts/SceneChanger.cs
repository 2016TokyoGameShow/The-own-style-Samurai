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

    [SerializeField]
    List<EventDelegate>	finalize;

    static List<EventDelegate>	delegates;


    void Awake()
    {
        tweens = new List<UITweener>(enableTweens);
        disable = new List<UITweener>(disableTweens);
        delegates = new List<EventDelegate>(finalize);
        enableTweens.Clear();
        disableTweens.Clear();
        finalize.Clear();
    }
    
    public static void FadeStart(string sceneName)
    {
        if(tweens[0].enabled)
        {
            return;
        }

        End();

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

    static void End()
    {
        if (delegates == null)
		{
            return;
        }
		List<EventDelegate> mTemp = delegates;
		delegates = new List<EventDelegate>();

		// Notify the listener delegates
		EventDelegate.Execute(mTemp);

		// Re-add the previous persistent delegates
		for (int i = 0; i < mTemp.Count; ++i)
		{
			EventDelegate ed = mTemp[i];
			if (ed != null && !ed.oneShot) EventDelegate.Add(delegates, ed, ed.oneShot);
		}
		mTemp = null;
    }
}