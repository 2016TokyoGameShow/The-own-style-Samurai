using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Title : MonoBehaviour
{
    [SerializeField]
    GameObject title, menu;
    

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
        }
    }

    public void GotoStageScene()
    {
        SceneManager.LoadScene("Stage01");
    }
}