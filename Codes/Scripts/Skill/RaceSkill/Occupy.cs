﻿using UnityEngine;
using System.Collections;

public class Occupy : Skill {

    int itemUsingNumber=0, itemUsingNumber_Max = 3;


    public override void Create(int ID)
    {
        base.Create(ID);
        ItemManager.Instance.AddObserver(this);
    }

    public override void Trigger()
    {
        if (itemUsingNumber < itemUsingNumber_Max)
            itemUsingNumber++;
        else
        {
            itemUsingNumber = 0;
            this.GetComponent<Character>().Hp++;
        }
    }

    public override void skillDestory()
    {
        itemUsingNumber = 0;

        ItemManager.Instance.RemoveObserver(this);
        base.skillDestory();
    }

    public override void OnNotify(string msg)
    {
        if (UtilManager.Instance.GetFieldFormMsg(msg, -1) == "UseItem_Skill_ID")
        {
            Trigger();
        }
        if (UtilManager.Instance.GetFieldFormMsg(msg, -1) == "UseItem_Buff_ID")
        {
            Trigger();
        }
           
    }
}