using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject title, menu, start, tutorial;
	GameObject hoveredObject;

	[SerializeField]
	AudioSource source;

	void Awake()
	{
		UICamera.currentScheme = UICamera.ControlScheme.Controller;
	}

	void Start()
	{
		hoveredObject = UICamera.hoveredObject;
        AudioManager.PlayBGM("titleBGM");
	}

    void Update()
    {
		if(menu.activeSelf && hoveredObject != UICamera.hoveredObject)
		{
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
        SceneChanger.FadeStart("Opening");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}