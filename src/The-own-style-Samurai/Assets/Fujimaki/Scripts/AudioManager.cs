using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {


    private static AudioManager audioManager;
    private static AudioSource audioSource;

    [SerializeField]
    private AudioClip[] SFXFiles;
    [SerializeField]
    private AudioClip[] BGMFiles;

    private static Dictionary<string, AudioClip> sfxDatas;
    private static Dictionary<string, AudioClip> bgmDatas;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        sfxDatas = new Dictionary<string, AudioClip>();
        bgmDatas = new Dictionary<string, AudioClip>();

        for (int i = 0; i < SFXFiles.Length; i++)
        {
            sfxDatas.Add(SFXFiles[i].name, SFXFiles[i]);
        }

        for (int i = 0; i < BGMFiles.Length; i++)
        {
            bgmDatas.Add(BGMFiles[i].name, BGMFiles[i]);
        }
    }
	

    public static void PlaySE(string name, float volume = 1)
    {
        audioSource.PlayOneShot(sfxDatas[name],volume);
    }

    public static void PlayBGM(string name, float volume = 1)
    {
        if (name == "")
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.clip = bgmDatas[name];
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    public static void ChangeVolume(float value)
    {
        audioSource.volume = value;
    }

    public static float GetVolume()
    {
       return audioSource.volume;
    }
}
