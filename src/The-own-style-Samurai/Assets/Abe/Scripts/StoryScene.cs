using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryScene : MonoBehaviour
{
	[SerializeField]
	string bgmName;

	[SerializeField]
	string gotoSceneName;

    [SerializeField, Tooltip("説明文")]
    GameObject[] slide;

    [SerializeField, Tooltip("説明文")]
    GameObject fadeOut;

    [SerializeField]
    UITweener tween;

    int count = 0;
	bool isLock = true;
    
	IEnumerator Start()
	{
		AudioManager.PlayBGM(bgmName, 0.8f);

        yield return null;

        tween.enabled = true;
        tween.ResetToBeginning();
	    tween.PlayForward();
	}

    void Update()
    {
		if(isLock) return;

        if(Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                slide[count].GetComponent<TweenPosition>().enabled = true;
                count++;
				AudioManager.PlaySE("nextSE");
            }
            catch //Listの範囲超えたら
            {
                fadeOut.SetActive(true);
            }

			isLock = true;
        }
    }

	public void UnLock()
	{
		isLock = false;
	}

    public void GotoScene()
    {
        SceneManager.LoadScene(gotoSceneName);
    }
}