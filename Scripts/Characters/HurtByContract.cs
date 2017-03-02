﻿using UnityEngine;
using System.Collections;

public class HurtByContract : MonoBehaviour {

    public int camp;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 2D碰撞检测
    /// </summary>
    /// <param name="other">与其碰撞的GameObj</param>
    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.tag == "Enemy")
        {
            Debug.Log("Enemy hurt!" + other.GetComponent<Character>().Health);
            other.GetComponent<Character>().Health--;
        }
       
    }
}
