using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Opening : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]
    
    public void Play()
    {
        Debug.Log("aaaa");
        AudioManager.PlayBGM("");
        AudioManager.PlaySE("selectSE");
    }
}