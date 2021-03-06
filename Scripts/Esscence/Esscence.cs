﻿using UnityEngine;
using System.Collections;

public class Esscence : RoomElement
{


    #region Variables
    int esscenceID;
    public Sprite[] spriteArray;
    //名称描述图片类别ID
    static public string[] esscenceName = { "贪婪精华", "懒惰精华", "傲慢精华", "暴怒精华" };
    static public string[] esscenceDescrible = { "由贪婪残念凝结而成的精华,封印着强大的力量", 
                                                 "由懒惰残念凝结而成的精华,封印着强大的力量", 
                                                 "由傲慢残念凝结而成的精华,封印着强大的力量", 
                                                 "由暴怒残念凝结而成的精华,封印着强大的力量" };

    static public int[] esscenceType = { 1, 3, 2, 0 };
    static public string[,] esscenceSkillDescribe = {
                                             { "一次性道具的使用次数+1",
                                              "主动道具充能变快", 
                                              "财富越多，幸运越高",
                                              "每使用3个道具回复一格体力", 
                                              "财富是世上最神奇的事物,可以驱使一切" },
                                             { "每结束一个房间回复一格生命", 
                                               "主动道具的充能灵魂数减少1/3", 
                                               "进入房间时,有一定几率使敌人攻击速度-1", 
                                               "不会被击倒",
                                               "被攻击时,有一定概率增加一个护盾" },
                                             { "攻击时有一定几率使对方中毒", 
                                               "攻击时有一定几率回复一格血量", 
                                               "驱使血奴为其战斗", 
                                               "攻击时有一定概率击退敌人",
                                               "攻击时召唤一只蝙蝠" },
                                             { "每个房间造成10点伤害后攻击速度+1", 
                                               "攻击一定概率使对方难以移动", 
                                               "低血量时造成的攻击伤害+1",
                                               "在一个房间中受2点伤害，攻速+1",
                                               "攻击时一定几率暴击" }
                                             };
    static public string[,] esscenceSkillDetailDescribe = {
                                             { "一次性道具的使用次数+1，道具的掉落概率增加", "主动道具充能时，一定概率增加的灵魂数+1", "强大的欲望使得自己可以为了自己财富失去本性。财富（有用过的道具，详解见贪婪第五被动）越多，幸运越高（比例暂定）", "贪婪的欲望使得其想占有一切财富，越来越多的财富使其不断兴奋，变得更强。每使用3个道具回复一格体力", "在贪婪看来，财富是世上最神奇的事物，可以驱使一切。" },
                                             { "嗜睡：\n每结束一个房间回复一格生命", "天赋：锻造\n主动道具的充能灵魂数减少1/3", "进入房间时，懒惰的欲望使敌方昏昏欲睡，导致行动迟缓（进入房间时，有一定几率使敌方的攻击速度-1所有敌人，独自判定", "优秀种族天赋给予了矮人们强大的抗性（不会被击倒，但是会被打断击退和硬控）","被攻击时，有一定概率增加一个护盾（抵挡3点伤害）" },
                                             { "攻击时用自己最拿手的剧毒诅咒吞噬对方，有一定几率使对方中毒", "吸血", "被吸血鬼击杀的敌人都有可能变成吸血鬼虚弱的血奴，为其战斗", "过度的傲慢致使生人勿进，攻击时有一定概率击退敌人，打断攻击", "攻击时召唤一只蝙蝠" },
                                             { "狼人对血的渴望使得他们越来越强（每个房间造成10点伤害后攻击速度+1）", "狼人利用增加尖锐的爪子撕裂对方的伤口，是对方难以移动", "当自己的生命少于3格时（不包括三格）时，处于暴走状态，造成的攻击伤害+1", "草原上的孤狼越战越勇，带着丝丝怒意，使自己变得更强，更快。在一个房间中收到超过两点伤害，攻速+1", "带着怒气的攻击将造成意想不到的伤害，攻击时一定几率暴击" }
                                             };
    static public string[,] esscenceSkillName={
                                                { "哈", "哈哈", "哈哈哈", "欲望：占有" ,"欲望：驱使"},
                                                { "嗜睡", "天赋：锻造", "欲望：永恒安眠", "天赋：坚甲" ,"懒惰"},
                                                { "诅咒：毒噬", "吸血", "天赋：血奴", "欲望：唯我独尊" ,"天赋：变身蝙蝠"},
                                                { "本性：嗜血", "天赋：撕裂", "欲望：血之暴怒", "天赋：孤狼血统" ,"欲望：缠怒"}
                                              };
    public Sprite[] esscenceSkillSprite;
    static public Sprite[] esscenceSprite;

    bool playerIn = false;
    #endregion

    public void Create(int ID) 
    {
        esscenceID = ID;
        GetComponent<SpriteRenderer>().sprite=spriteArray[esscenceID];   
    }

    void PlayerGet() 
    {
        Notify("Get_Esscence;" + esscenceID);
        UIManager.Instance.RemoveObserver(this);
        Player.Instance.Character.RemoveObserver(this);

        GameObject pfb = Resources.Load("Buffs/devil") as GameObject;
        Vector3 s = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -1);
        pfb.GetComponent<SpriteRenderer>().sprite = spriteArray[esscenceID];
        GameObject prefabInstance = Instantiate(pfb);
        prefabInstance.transform.position = s;
        prefabInstance.transform.parent = this.gameObject.transform;


        base.Destroy();
    }



    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="other">与其碰撞的GameObj</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Notify("Player_Get_Esscence;" + esscenceID);
            playerIn = !playerIn;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Notify("Player_Leave_Esscence;" + esscenceID);
            playerIn = !playerIn;
        }
    }


	// Use this for initialization
	void Start () {
        this.AddObserver(UIManager.Instance);
        Player.Instance.Character.AddObserver(this);
        this.AddObserver(EsscenceManager.Instance);
	}

    public override void OnNotify(string msg)
    {
        string[] str = UtilManager.Instance.GetMsgFields(msg);
        if (str[0] == "AttackStart" && playerIn)
        {
            PlayerGet();
        }
    }

    void Awake() 
    {
        esscenceSprite = esscenceSkillSprite;
    
    }
}
