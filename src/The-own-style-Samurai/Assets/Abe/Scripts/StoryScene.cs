using UnityEngine;
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

    int count = 0;
	bool isLock = true;
    
	void Start()
	{
		AudioManager.PlayBGM(bgmName);
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