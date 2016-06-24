using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
    GameObject fadeOut, continueUI, returnTitleUI;

	[SerializeField]
	new AudioSource audio;

	bool frag = true;
	string sceneName = "";

	IEnumerator Start()
	{
		audio.time = 2f;
		AudioManager.PlayBGM("gameOver" , 1);

		yield return new WaitForSeconds(2.8f);
		frag = false;
	}

    void Update()
    {
		if(frag)
		{
			return;
		}

        if(Input.GetKeyDown(KeyCode.Return))
		{
			continueUI.GetComponent<TweenAlpha>().enabled = true;
			sceneName = "Stage01";
			AudioManager.PlaySE("selectSE");
			frag = true;
		}
		else if(Input.GetKeyDown(KeyCode.Space))
		{
			returnTitleUI.GetComponent<TweenAlpha>().enabled = true;
			sceneName = "Title";
			AudioManager.PlaySE("selectSE");
			frag = true;
		}
    }

	public void FadeOut()
	{
		//SceneChanger.FadeStart(sceneName);
	}
}