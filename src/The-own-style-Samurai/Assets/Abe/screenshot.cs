using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class screenshot : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        
    }
    
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Return) ) {
            // スクリーンショット保存
            Application.CaptureScreenshot(Application.streamingAssetsPath + "screenshot.png");
        }
    }
}