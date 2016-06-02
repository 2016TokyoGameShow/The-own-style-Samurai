using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    private static AudioSource audioSource;

    [SerializeField]
    private AudioClip[] SFXFiles;
    [SerializeField]
    private AudioClip[] BGMFiles;

    private static Dictionary<string, AudioClip> sfxDatas;
    private static Dictionary<string, AudioClip> bgmDatas;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();

        sfxDatas = new Dictionary<string, AudioClip>();
        bgmDatas = new Dictionary<string, AudioClip>();

        for(int i = 0; i < SFXFiles.Length; i++)
        {
            sfxDatas.Add(SFXFiles[i].name, SFXFiles[i]);
        }

        for (int i = 0; i < BGMFiles.Length; i++)
        {
            bgmDatas.Add(BGMFiles[i].name, BGMFiles[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void PlaySE(string name, float volume = 1)
    {
        audioSource.PlayOneShot(sfxDatas[name],volume);
    }

    public static void PlayBGM(string name, float volume = 1)
    {
        audioSource.clip = bgmDatas[name];
        audioSource.volume = volume;
    }
}
