﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public enum RoomDomain
{
    RoomGround,
    RoomWall,
    RoomBlocks,
    UnLimited
}
/*
 * 0:发射物Missile
 * 1:箱子Box
 * 2:镜子Mirror
 * 3:门Door
 * 4:雕像Statue
 * 5:爪子Claw
 * 6:图一Picture1
 * 7:图二Picture2
 * 8:骷髅Skull
 * 9:骷髅灯SkullLight
 * 10:瓶子一Bottle1
 * 11:瓶子二Bottle2
 * 12:骨头Gone
 * 13:杆Rod
 * 14:石头Stone
 * 15:陷阱Trap
 * 16:楼梯Stair
 * 17:祭坛一Altar1
 * 18:祭坛二Altar2
 * 19:商店Shop
 * 20:牌子Plate
 * 21:金币Coin
 * 22:开关Handle
 * 1000+:道具
 * 2000+ 人物
 * */
public enum REID
{
    XXX=-1,
    Unknown = 0,
    //房间物品
    Box = 1,
    Mirror,

    Statue ,
    Claw ,
    Picture1 ,
    Picture2 ,
    Skull,
    SkullLight,
    Bottle1 ,
    Bottle2,
    Bone ,
    Rod ,
    Stone ,
    Trap ,
    Stair ,
    Altar1 ,
    Altar2 ,
    Shop ,
    Plate,
    Coin ,
    Handle,
    FrontDoorDecoration,
    DoorFront,
    DoorBack,
    DoorLeft,
    DoorRight,



    Missle=50,
    //道具
    Item=1000,
    ItemEnd=1999,
    Character= 2000,
    //小怪
    GnomeBlaster = 2100,
    GnomeWarrior = 2101,
    GnomeAlchemist = 2102,
    GnomeArcher = 2103,
    GnomePatrol = 2104,
    GnomeAdvancedArcher = 2105,
    GnomeScootWarrior = 2106,
    GnomeCheerer = 2107,
    GnomePharmacist = 2108,
    GnomeKing = 2109,

    PygmyPuppet1 = 2200,
    PygmyPuppet2 = 2201,
    PygmyPuppet3 = 2202,
    PygmyTrainGunner = 2203,
    PygmyRobber = 2204,
    PygmySummoner = 2205,
    PygmyIceGunner = 2206,
    PygmyFireGunner = 2207,
    PygmySiegePuppet = 2208,
    PygmyKing = 2209,

    VampireBat = 2300,
    VampireWarrior = 2301,
    VampireCrossbower = 2302,
    VampireMage = 2303,
    VampireAssassin = 2304,
    VampireBlaster = 2305,
    VampireCrackWarrior = 2306,
    VampireHuntter = 2307,
    VampireSummoner = 2308,
    VampireSpiritCaller = 2309,
    VampireKing = 2310,
    CharacterEnd = 2999,
};
/// <summary>
/// RoomElement
/// Brief:房间元素,具备MonoBehavoir以及Notify/OnNotify的功能
/// Latest Update At 17.5.28
/// </summary>
[System.Serializable]
//单个房间元素信息
public struct RoomElementInfo
{
    public REID ID;
    public Vector3 Position;
    public int State;

    public RoomElementInfo(REID id = 0, Vector3 position = new Vector3(), int state = 0)
    {
        ID = id;
        Position = position;
        State = state;
    }
}

public class RoomElement : ExSubject
{

    #region Varibles
    public REID roomElementID=0;
    public int roomElementState = 0;
    public AudioClip roomElementSound;
    public RoomDomain belongDomain=RoomDomain.RoomGround;


    /// <summary>
    /// 是否在进入房间的时候销毁,默认为true
    /// </summary>
    bool isDestoryOnEnterRoom = true;

    /// <summary>
    /// 是否随Master死亡而死亡
    /// </summary>
    bool isDieWithMaster = true;


    GameObject bloodBarInstance;

    //主人
    RoomElement master;
    //从属者
    List<RoomElement> servants = new List<RoomElement>();

    int maxHp = 10;
    float hp;
    float healthPercent = 1f;


    #endregion

    #region Methods

    #region Getter&Setter


    public REID RoomElementID
    {
        get { return roomElementID; }
        set { roomElementID = value; }
    }



	public int RoomElementState
	{
		get { return roomElementState; }
		set { roomElementState = value; }
	}
    public bool IsDieWithMaster
    {
        get { return isDieWithMaster; }
        set { isDieWithMaster = value; }
    }
    public bool IsDestoryOnEnterRoom
    {
        get { return isDestoryOnEnterRoom; }
        set { isDestoryOnEnterRoom = value; }
    }
    //0.生命Health,代表玩家的血量
    //float hpValue;  
    public virtual float Hp
    {
        get { return hp; }
        set {
         
            StringBuilder s = new StringBuilder(30).Append("HealthChanged;").Append((int)Hp).Append(";");
            hp = value;
            HealthPercent = Hp / maxHp;
            Notify(s.Append((int)Hp).Append(";").Append(this.tag).ToString());
        }
    }
    /// <summary>
    /// 生命百分比
    /// </summary>
    public float HealthPercent
    {
        get { return healthPercent; }
        set
        {
            float tmp = healthPercent;
            healthPercent = value;
            Notify("HealthPercent;" + healthPercent + ";" + tmp);
        }
    }

    public RoomElement Master
    {
        get { return master; }
        set { 
            master = value;
        
        if (!master.Servants.Contains(this))
              master.Servants.Add(this);
        }
    }

    public List<RoomElement> Servants
    {
        get { return servants; }
        set { servants = value; }
    }
    #endregion

    #region Virtual Methods
    public virtual void Awake () {
        //this.tag = "RoomElement";
        RoomElementManager.Instance.RoomElementList.Add(this);
	}
    public virtual void Destroy()
    {
        if (Master != null)
            Master.Servants.Remove(this);

        RoomElementManager.Instance.RoomElementList.Remove(this);
        Destroy(this.gameObject);
    }
    public virtual void KillServants()
    {
        for (int i = Servants.Count-1; i >= 0; i--)
        {
            if (Servants[i] == null)
                continue;
            RoomElement re = Servants[i].GetComponent<RoomElement>();
            if (re.isDieWithMaster)
                re.Die();
        }
    }
    public virtual void Die() {
        this.Destroy();
    }


    /// <summary>
    /// 当被观察者靠近并按下攻击键时触发该事件
    /// </summary>
    public virtual void CloseAttackEvent()
    { 
    }
    /// <summary>
    /// 当该gameobject被击中触发该事件
    /// </summary>
    public virtual void HitEvent()
    {

    }
    /// <summary>
    /// 当该gameobject被进入时触发
    /// </summary>
    public virtual void EnterEvent()
    {

    }


    public override void OnNotify(string msg)
    {
        string[] str = UtilManager.Instance.GetMsgFields(msg);
        // Debug.Log("altarmsg:" + msg);
        if (str[0] == "AttackStart" && str[1] == "J")
        {
            CloseAttackEvent();
        }
//		if (str [0] == "MissileEnterBottle") 
//		{
//			Trriger();
//		}

    }
    #endregion

    #region Other Methods
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        SoundManager.Instance.PlaySoundEffect(clip);
    }

    public void EnableBloodBar(bool isEnable)
    {

        if(isEnable)
        {
            if (bloodBarInstance)
                bloodBarInstance.SetActive(true);
            else
                bloodBarInstance = Instantiate(RETable.Instance.bloodBarPrefab, transform,false) as GameObject;
        }
        else
        {
            if (bloodBarInstance)
                bloodBarInstance.SetActive(false);
        }
    }


    public virtual RoomElementInfo GetInfo()
    {
        return new RoomElementInfo(RoomElementID, transform.position, RoomElementState);

    }

    #endregion

    #endregion
}
