using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
    GameObject[] slide;

    [SerializeField, Tooltip("説明文")]
    GameObject fadeOut;

    int count = 0;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            try
            {
                slide[count].GetComponent<TweenPosition>().enabled = true;
                count++;
            }
            catch //Listの範囲超えたら
            {
                fadeOut.SetActive(true);
            }
        }
    }

    public void GotoScene()
    {
        SceneManager.LoadScene("Stage01");
    }
}