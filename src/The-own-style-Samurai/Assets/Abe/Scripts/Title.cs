using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject title, menu, start, UI, fadeOutObject;
    

    void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            title.SetActive(false);
            menu.SetActive(true);
			start.GetComponent<UIButton>().isEnabled = true;
			UICamera.hoveredObject = start;
        }
    }

    public void FadeOut()
    {
        fadeOutObject.SetActive(true);
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