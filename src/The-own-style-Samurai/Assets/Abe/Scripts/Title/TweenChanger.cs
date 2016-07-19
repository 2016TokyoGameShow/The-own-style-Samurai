using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenChanger : MonoBehaviour
{
	[SerializeField, Tooltip("説明文")]
	List<UITweener> activeTween, deactiveTween;

	[SerializeField]
	List<EventDelegate>	pushed;

    bool isLock = false;

	void Update()
	{
        if(isLock)
        {
            return;
        }

		if(Input.GetKeyUp(KeyCode.Return)||
           Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			Change();
		}
	}

    public void Change()
    {
        foreach(UITweener tween in activeTween)
		{
			tween.enabled = true;
			tween.ResetToBeginning();
			tween.PlayForward();
		}

		foreach(UITweener tween in deactiveTween)
		{
			tween.enabled = false;
		}

		if (pushed != null)
		{
			List<EventDelegate> mTemp = pushed;
			pushed = new List<EventDelegate>();

			// Notify the listener delegates
			EventDelegate.Execute(mTemp);

			// Re-add the previous persistent delegates
			for (int i = 0; i < mTemp.Count; ++i)
			{
				EventDelegate ed = mTemp[i];
				if (ed != null && !ed.oneShot) EventDelegate.Add(pushed, ed, ed.oneShot);
			}
			mTemp = null;
		}

        isLock = true;
    }

	public void Reset()
	{
        isLock = false;
		foreach(UITweener tween in activeTween)
		{
			tween.enabled = false;
		}

		foreach(UITweener tween in deactiveTween)
		{
			tween.enabled = true;
			tween.ResetToBeginning();
			tween.PlayForward();
		}

		Debug.Log("Reset");
	}
}