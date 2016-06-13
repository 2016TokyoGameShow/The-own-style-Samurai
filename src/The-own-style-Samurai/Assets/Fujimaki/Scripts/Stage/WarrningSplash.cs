﻿using UnityEngine;
using System.Collections;

public class WarrningSplash : MonoBehaviour {


    private GameObject target;

    [SerializeField]
    private GameObject attackArea;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = target.transform.position + new Vector3(0, 3.5f, 0);

        }else
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize(float time,GameObject target)
    {
        this.target = target;
        Destroy(this.gameObject, 1);

    }
}
