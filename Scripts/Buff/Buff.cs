﻿using UnityEngine;
using System.Collections;
/// <summary>
/// 包括改变属性（回合/永久），特殊攻击特效等
/// </summary>
public class Buff : ExSubject
{
    public GameObject attackPfb;
    public GameObject statePfb;

    protected GameObject prefabInstance;
    public GameObject effectPrefeb;

    public string[] attackTag={"Monsters"};

    protected System.DateTime oldTime=System.DateTime.Now;

    private string specialTag;
    public string SpecialTag
    {
        get { return specialTag; }
        set { specialTag = value; }
    }

    protected string buffTag;
    private int buffID;
    public int BuffID
    {
        get { return buffID; }
        set { buffID = value; }
    }
   

   
    protected int effectType;
    /// <summary>
    /// 效果类型，1为增益，0为减益
    /// </summary>
    public int EffectType
    {
        get { return effectType; }
        set { effectType = value; }
    }
       
   
    protected int buffDuration;
    /// <summary>
    /// 持续时间
    /// </summary>
    public int BuffDuration
    {
      get { return buffDuration; }
      set { buffDuration = value; }
    }
    
    protected int effectDuration;
    /// <summary>
    /// 效果持续时间，1位永久，0位临时
    /// </summary>
    public int EffectDuration
    {
        get { return effectDuration; }
        set { effectDuration = value; }
    }



    protected Sprite buffSprite;
    protected SpriteRenderer spriteRenderer;

    virtual public void DestroyBuff()
    {
        
        this.gameObject.GetComponent<BuffManager>().BuffList.Remove(this);
        Destroy(this);  
    }

    virtual public string SaveBuff()
    {
        System.DateTime nowTime = System.DateTime.Now;
        //Debug.Log("sub     :" + ((oldTime.Minute - nowTime.Minute) * 60 + oldTime.Second - nowTime.Second));
        return "saving;" + BuffID + ";" + ((oldTime.Minute - nowTime.Minute) * 60 + oldTime.Second - nowTime.Second);
    }

	
    virtual public bool judgeTag(string tag)
    {
        for (int i = 0; i < attackTag.Length; i++)
            if (tag == attackTag[i])
                return true;

        return false;
    }


}
