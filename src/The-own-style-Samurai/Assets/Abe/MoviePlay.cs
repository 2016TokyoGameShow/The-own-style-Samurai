using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoviePlay : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]

    private MovieTexture movieTexture;



    void Awake()
    {
        
    }
    
    void Start()
    {
        movieTexture = (MovieTexture)GetComponent<UITexture>().mainTexture;
        movieTexture.Play();
    }
    
    void Update()
    {
        if(!movieTexture.isPlaying || Input.anyKeyDown)
        {
            SceneChanger.FadeStart("Title");
        }
    }
}