﻿using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour
{

    private Animator animator;
    //NEED public AudioClip getBox;
    //NEED public AudioClip openBox;
    void Awake()
    {
    
        animator = GetComponent<Animator>();
        //NEED SoundManager.instance.PlaySingle(getBox);
    }

    void Update()
    {
       
    }

    void OpenBox()
    {
        //打开宝箱的动画和声音
        animator.SetTrigger("OpenBox");
        //NEED SoundManager.instance.PlaySingle(openBox);   
        //NEED Item item=ItemManager.getInstance().GenerateItem();
        //item.transfrom.setParent(this.transform);
    }
}
