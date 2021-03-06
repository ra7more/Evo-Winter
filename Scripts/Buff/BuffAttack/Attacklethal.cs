﻿using UnityEngine;
using System.Collections;

public class Attacklethal : BuffAttack
{

    int enemyID;

    protected override void Trigger()
    {
        if (JudgeTrigger())
        {
            CharacterManager.Instance.CharacterList[enemyID].Hp = 0;
        }

    }
    protected override void Create(int ID)
    {
        base.Create(ID);

        //添加特效
        GameObject pfb = Resources.Load("Buffs/Attack/Attacklethal") as GameObject;
        Vector3 s = new Vector3(this.gameObject.GetComponent<CharacterSkin>().body.transform.position.x, this.gameObject.GetComponent<CharacterSkin>().body.transform.position.y, -1);
        prefabInstance = Instantiate(pfb);
        prefabInstance.transform.position = s;
        prefabInstance.transform.parent = this.gameObject.GetComponent<CharacterSkin>().body.transform;
        prefabInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    }

    // Use this for initialization
    void Start()
    {

    }

    public override void OnNotify(string msg)
    {
        string bID = "";

        bID = UtilManager.Instance.MatchFiledFormMsg("AttackHit", msg, 0);
        if (bID == "Monster")
        {
            bID = UtilManager.Instance.MatchFiledFormMsg("AttackHit", msg, 1);
            enemyID = int.Parse(bID);
            Trigger();
        }
    }
}
