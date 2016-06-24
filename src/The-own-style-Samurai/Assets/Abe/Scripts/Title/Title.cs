using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject title, menu, tutorial, fadeOutObject;
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
			AudioManager.PlaySE("menuSE");
			hoveredObject = UICamera.hoveredObject;
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
        if(fadeOutObject.activeSelf)
        {
            return;
        }
        fadeOutObject.SetActive(true);
		AudioManager.PlayBGM("");
		AudioManager.PlaySE("selectSE");
    }

    public void GotoStageScene()
    {
        SceneManager.LoadScene("Opening");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}