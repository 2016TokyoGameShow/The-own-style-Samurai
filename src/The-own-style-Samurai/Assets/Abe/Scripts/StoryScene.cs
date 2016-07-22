using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryScene : MonoBehaviour
{
    [SerializeField]
    string bgmName;
    
    [SerializeField]
    string gotoSceneName;

    [SerializeField]
    bool useSkip = true;

    [SerializeField, Tooltip("説明文")]
    GameObject[] slide;

    int count = 0;
    bool isLock = true;
    
    void Start()
    {
        AudioManager.PlayBGM(bgmName, 0.8f);
    }

    void Update()
    {
        if(isLock) return;

        if(Input.GetKeyDown(KeyCode.Return) ||
           Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            try
            {
                slide[count].GetComponent<TweenPosition>().enabled = true;
                count++;
                AudioManager.PlaySE("nextSE");
            }
            catch //Listの範囲超えたら
            {
                SceneChanger.FadeStart(gotoSceneName);
            }
            
            isLock = true;
        }
        else if(Input.GetKeyDown(KeyCode.JoystickButton7) ||
                Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(!useSkip)
            {
                return;
            }

            //スキップ
            if(count >= slide.Length)
            {
                return;
            }

            TweenPosition pos = slide[count].GetComponent<TweenPosition>();
            pos.duration = pos.duration / 2;
            pos.enabled = true;

            for(int i = count + 1; i < slide.Length; i++)
            {
                slide[i].SetActive(false);
            }
            count = slide.Length;

            AudioManager.PlaySE("nextSE");
        }
    }

    public void UnLock()
    {
        isLock = false;
    }
}