﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class iTweenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Hashtable args = new Hashtable();
        //args.Add("time", 2f);//设置动画实现消耗时间   time 一定不要写错
        //args.Add("easeType", iTween.EaseType.easeInQuad);//设置动感播放类型 //这个是线形运动 也就是匀速 默认 是加速减速
        //args.Add("x", -20);
        //args.Add("y", 0);
        //args.Add("z", 0);
    
        //iTween.MoveTo(gameObject, args);//第一个参数操作的对象 第二个 设置动画系列hashtable
        //iTween.MoveTo(gameObject, Vector3.zero, 2f);
        Vector3[] paths = this.GetComponent<iTweenPath>().nodes.ToArray();
        iTween.MoveTo(gameObject, iTween.Hash("path", paths, "speed", 10f, "easeType", iTween.EaseType.linear)); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
