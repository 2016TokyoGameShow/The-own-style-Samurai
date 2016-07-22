using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class animationPlay : MonoBehaviour
{
    //[SerializeField, Tooltip("説明文")]
    
    void Awake()
    {
        
    }
    
    void Start()
    {
        GetComponent<Animator>().Play("Die");
    }
    
    void Update()
    {
        
    }
}