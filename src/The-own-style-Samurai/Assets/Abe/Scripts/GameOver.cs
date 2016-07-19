using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
    GameObject continueUI, returnTitleUI;

    [SerializeField]
    new AudioSource audio;

    bool frag = true;
    string sceneName = "";

    IEnumerator Start()
    {
        audio.time = 2f;
        AudioManager.PlayBGM("gameOver", 1);

        yield return new WaitForSeconds(0.7f);
        frag = false;
    }

    void Update()
    {
        if(frag)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            continueUI.GetComponent<TweenAlpha>().enabled = true;
            sceneName = "Stage01";
            AudioManager.PlaySE("selectSE");
            frag = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            returnTitleUI.GetComponent<TweenAlpha>().enabled = true;
            sceneName = "Title";
            AudioManager.PlaySE("selectSE");
            frag = true;
        }
    }

    public void FadeOut()
    {
        SceneChanger.FadeStart(sceneName);
    }
}