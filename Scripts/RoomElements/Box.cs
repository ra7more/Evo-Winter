﻿using UnityEngine;
using System.Collections;

public class Box : RoomElement
{
    private Animator animator;
    private bool isOpen;
    //NEED public AudioClip getBox;
    public AudioClip openBox;
    void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;

        //NEED SoundManager.instance.PlaySingle(getBox);
    }

    public override void Awake()
    {
        base.Awake();
        RoomElementID = 1;
        this.tag = "DynamicGroundElement";
    }
    void Update()
    {
       
    }

    public void OpenBox()
    {
        //打开宝箱的动画和声音
        animator.SetTrigger("OpenBox");
		RoomElementState = 1;
        isOpen = true;
        SoundManager.Instance.PlaySoundEffect(openBox);
        //NEED Item item=ItemManager.getInstance().GenerateItem();
        //item.transfrom.setParent(this.transform);

        if (Random.value * 100 < 20)
            EsscenceManager.Instance.CreateEsscence((int)(Random.value * 4), this.transform.position);
        else
        {
           // ItemManager.Instance.ItemsTransform = new Transform();
            ItemManager.Instance.ItemsTransform.position = this.transform.position+new Vector3(0,1,0);
            ItemManager.Instance.CreateItemDrop(false, false, true);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("箱子碰撞物标签：" + other.tag);
        if (other.tag == "Weapon" )
            if (other.GetComponentInParent<Character>().IsWeaponDmg>0&&isOpen==false&& other.GetComponentInParent<Character>().Camp==0)
            {
                OpenBox();
            }

		if ((other.tag == "Player"||other.tag=="Missile")&&isOpen==false)
        {
            OpenBox();
        }
    }
}
