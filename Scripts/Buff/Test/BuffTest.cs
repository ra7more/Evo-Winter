﻿using UnityEngine;
using System.Collections;

public class BuffTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
       StartCoroutine(Test());
       
	}
    IEnumerator Test() 
    {
        yield return new WaitForSeconds(1.0f);
        BuffManager.Instance.CreateBuff(100001);
    
    
    }
	
}
