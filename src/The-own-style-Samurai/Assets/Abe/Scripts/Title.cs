using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject title, menu, start, UI, fadeOutObject;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            title.SetActive(false);
            menu.SetActive(true);
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