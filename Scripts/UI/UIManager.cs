﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*UIManager
 *@Brief 负责管理游戏UI交互
 *@Author YYF
 *@Time 17.1.25 
 *@Update at 17.1.26
 */
/// <summary>
/// <para>Brief 负责管理游戏UI元素</para>
/// <para>Author YYF</para>
/// <para>Time 17.1.26</para>
/// </summary>
public class UIManager : ExUnitySingleton<UIManager>
{
    //8个子构件

    private LittleMap littleMap;    //小地图

    public LittleMap LittleMap
    {
        get { return littleMap; }
    }
    private PlayerInfo playerInfo;  //玩家信息

    public PlayerInfo PlayerInfo
    {
        get { return playerInfo; }
    }
    private PlayerHealth playerHealth;  //玩家血量

    public PlayerHealth PlayerHealth
    {
        get { return playerHealth; }
    }
    private Popup popup;    //弹窗

    public Popup Popup
    {
        get { return popup; }
    }
    private EsscencesDisplayer esscencesDisplayer;  //精华数目展示模块

    public EsscencesDisplayer EsscencesDisplayer
    {
        get { return esscencesDisplayer; }
    }
    private MoveBall moveBall;  //移动球

    public MoveBall MoveBall
    {
        get { return moveBall; }
    }
    private AttackButtonManager attackButtonManager;    //攻击按钮管理者

    public AttackButtonManager AttackButtonManager
    {
        get { return attackButtonManager; }
    }
    private ItemButtonManager itemButtonManager;    //道具按钮管理者

    public ItemButtonManager ItemButtonManager
    {
        get { return itemButtonManager; }
    }


    void Start()
    {
        littleMap = LittleMap.Instance;
        playerInfo = PlayerInfo.Instance;
        playerHealth = PlayerHealth.Instance;
        esscencesDisplayer = EsscencesDisplayer.Instance;
        popup = Popup.Instance;
        moveBall = MoveBall.Instance;
        attackButtonManager = AttackButtonManager.Instance;
        itemButtonManager = ItemButtonManager.Instance;

        Player.Instance.Character.AddObserver(this);
        RoomManager.Instance.AddObserver(this);
        ItemManager.Instance.AddObserver(this);
        EsscenceManager.Instance.AddObserver(this);
    }


    public override void OnNotify(string msg)
    {
        //Debug.Log("UIManager recieved :"+msg);

        string[] str = UtilManager.Instance.GetMsgFields(msg);
        if (str[0] == "EnterRoom")
        {
            littleMap.UpdateLittleMap();
            DialogInfo.Instance.RandomDialog();
        }
 


        if (str[0] == "MapComplete")
            littleMap.InitLittleMap();


        //ItemManager Msg

        if (str[0] == "DisposableItem_Destroy")
            UIManager.Instance.ItemButtonManager.DestroyDisposableItem();
        if (str[0] == "InitiativeItem_Destroy")
            UIManager.Instance.ItemButtonManager.DestroyInitiativeItem();
        if (str[0] == "Player_Get_DisposableItem" || str[0] == "Player_Get_InitiativeItem" || str[0] == "Player_Get_ImmediatelyItem")
        {
            //TODO 显示道具信息

            UIManager.Instance.popup.SetItemDetailPopup(
                    ItemManager.Instance.itemsTable.GetItemName(int.Parse(str[1])), ItemManager.Instance.itemsTable.GetItemIntro(int.Parse(str[1])), ItemManager.Instance.itemsTable.GetItemType(int.Parse(str[1])), ItemManager.Instance.itemsTable.GetItemQuality(int.Parse(str[1])));
            UIManager.Instance.popup.itemDetailPopup.SetActive(true);
        }
        if (str[0] == "Player_Leave_DisposableItem" || str[0] == "Player_Leave_InitiativeItem" || str[0] == "Player_Leave_ImmediatelyItem")
        {

            //TODO 取消显示道具信息
            UIManager.Instance.popup.itemDetailPopup.SetActive(false);

        }
        if (str[0] == "Get_DisposableItem")//玩家拾取一次性道具
        {
            UIManager.Instance.popup.itemDetailPopup.SetActive(false);
            Sprite sp = ItemManager.Instance.GetDisposableItemsSprite();
            UIManager.Instance.ItemButtonManager.AddDisposableItem(sp);
        }
        if (str[0] == "Get_InitiativeItem")//玩家拾取主动道具
        {
            UIManager.Instance.popup.itemDetailPopup.SetActive(false);
            Sprite sp = ItemManager.Instance.GetInitiativeItemSprite();
            UIManager.Instance.ItemButtonManager.AddInitiativeItem(sp);
          
        }
        if (str[0] == "Get_ImmediatelyItem")//玩家拾取主动道具
        {
            UIManager.Instance.popup.itemDetailPopup.SetActive(false);
        }
        if (str[0] == "InitiativeItem_Energy_Number")
        {
            //TODO 改变主动道具的能量显示
            itemButtonManager.SetEnergy(int.Parse(str[1])/100);
        }

        //Esscence
        if (str[0] == "Player_Get_Esscence")
        {
            //TODO 显示道具信息

            UIManager.Instance.popup.SetItemDetailPopup(
                    ItemManager.Instance.itemsTable.GetItemName(int.Parse(str[1])), ItemManager.Instance.itemsTable.GetItemIntro(int.Parse(str[1])), ItemManager.Instance.itemsTable.GetItemType(int.Parse(str[1])), ItemManager.Instance.itemsTable.GetItemQuality(int.Parse(str[1])));
            UIManager.Instance.popup.itemDetailPopup.SetActive(true);
        }
        if (str[0] == "Player_Leave_Esscence")
        {

            //TODO 取消显示道具信息
            UIManager.Instance.popup.itemDetailPopup.SetActive(false);

        }

        if (str[0] == "Get_Esscence")//玩家拾取一次性道具
        {
            UIManager.Instance.popup.itemDetailPopup.SetActive(false);
            
            //UIManager.Instance.ItemButtonManager.AddDisposableItem(sp);
        }


        //Player Msg

        if (str[0]== "HealthChanged")
            UIManager.Instance.playerHealth.Health = (int)Player.Instance.Character.Health;
        if (str[0] == "AttackDamageChanged")
            AttriInfo.Instance.Atk = Player.Instance.Character.AttackDamage;
        if (str[0] == "AttackSpeedChangedChanged")
            AttriInfo.Instance.Atk = Player.Instance.Character.AttackSpeed;
        if (str[0] == "AttackRangeChanged")
            AttriInfo.Instance.Atk = Player.Instance.Character.AttackRange;
        if (str[0] == "HitRecoverChanged")
            AttriInfo.Instance.Atk = Player.Instance.Character.HitRecover;
        if (str[0] == "LuckChanged")
            AttriInfo.Instance.Atk = Player.Instance.Character.Luck;
        if (str[0] == "MoveSpeedChanged")
            AttriInfo.Instance.Atk = Player.Instance.Character.MoveSpeed;

        if (str[0] == "RaceChanged")
            CreerInfo.Instance.SetRace(int.Parse(str[2]));
        if (str[0] == "WeaponChanged")
            CreerInfo.Instance.SetCreer(int.Parse(str[2]));

    }





    ItemObserver itemObserver = new ItemObserver(); //Player的观察者

    internal ItemObserver ItemObserver
    {
        get { return itemObserver; }
        set { itemObserver = value; }
    }







}

class ItemObserver : ExSubject
{
    public override void OnNotify(string msg)
    {

      
        


    }
}