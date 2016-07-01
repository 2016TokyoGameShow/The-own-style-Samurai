using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleAudio : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]

    private static TitleAudio instance = null;

    new AudioSource audio;

    public static TitleAudio Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void StopBGM()
    {
        audio.Stop();
    }
}