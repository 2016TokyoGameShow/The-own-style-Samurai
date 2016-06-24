﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ReturnTitleTimer : MonoBehaviour
{
    [SerializeField, Tooltip("説明文")]
    float timer = 300;

    void Awake()
    {
        
    }
    
    void Start()
    {
        StartCoroutine(TimeUpdate());
    }
    
    void Update()
    {
        if(Input.anyKey)
        {
            StopAllCoroutines();
            StartCoroutine(TimeUpdate());
        }
    }

    IEnumerator TimeUpdate()
    {
        yield return new WaitForSeconds(timer);

        SceneManager.LoadScene("Title");
    }
}