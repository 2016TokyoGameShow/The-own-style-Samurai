using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject title, menu, start, start2, tutorial;
	GameObject hoveredObject;

	void Awake()
	{
		UICamera.currentScheme = UICamera.ControlScheme.Controller;
	}

	void Start()
	{
		hoveredObject = UICamera.hoveredObject;

        StartCoroutine(TitleToMovie());
	}

    IEnumerator TitleToMovie()
    {
        for(float t = 0; t <= 30; t += Time.deltaTime)
        {
            if(!title.activeSelf)
            {
                yield break;
            }
            yield return null;
        }

        SceneChanger.FadeStart("Movie");
    }

    void Update()
    {
		if(!menu.activeSelf || hoveredObject == UICamera.hoveredObject)
		{
            return;
        }

        if(hoveredObject != null)
        {
            AudioManager.PlaySE("menuSE");
            hoveredObject.GetComponent<TweenScale>().enabled = false;
        }
        hoveredObject = UICamera.hoveredObject;
        if(hoveredObject != null)
        {
            TweenScale scale = hoveredObject.GetComponent<TweenScale>();
            scale.enabled = true;
            scale.ResetToBeginning();
            scale.PlayForward();
        }
	}

	public void PlayStartSE()
	{
		AudioManager.PlaySE("startSE");
	}

	public void GotoMenu()
	{
		title.SetActive(false);
		menu.SetActive(true);
	}

	public void ShowTutorial()
	{
		AudioManager.PlaySE("selectSE");
		menu.SetActive(false);
		tutorial.SetActive(true);
	}

	public void HideTutorial()
	{
		tutorial.SetActive(false);
		menu.SetActive(true);
	}

    public void FadeOut()
    {
        if(TitleAudio.Instance == null) return;

        start2.GetComponent<TweenAlpha>().enabled = true;
        TweenScale scale = start2.GetComponent<TweenScale>();
        scale.from = start2.transform.localScale;
        scale.to   = start2.transform.localScale + new Vector3(0.5f, 0.5f, 0.5f);
        scale.enabled = true;
        
        Vector3 startScale = start.transform.localScale;
        start.GetComponent<TweenScale>().enabled = false;
        start.transform.localScale = startScale;

        TitleAudio.Instance.StopBGM();
        Destroy(TitleAudio.Instance.gameObject);

        
        AudioManager.PlaySE("selectSE");
        SceneChanger.FadeStart("Opening");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}