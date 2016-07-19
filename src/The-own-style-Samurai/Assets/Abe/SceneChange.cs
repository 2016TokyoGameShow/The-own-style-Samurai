using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]
    
    void Awake()
    {
        
    }
    
    void Start()
    {
        SceneManager.LoadScene("Title");
    }
    
    void Update()
    {
        
    }
}