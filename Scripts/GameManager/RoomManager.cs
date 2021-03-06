﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class RoomManager : ExUnitySingleton<RoomManager>
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

	//音乐
	public AudioClip[] BackgroundMusic;

	//房间类型枚举,房间类型号，-2BOSS，-1起始，0无，1宝箱，2商店，3祭坛，4隐藏房间
	public enum RmType {Boss = -2, Start, Non, Box, Shop, Altar, Normal};
    //最大最小敌人数量
    public int maxEnemyNumber = 5;
    public int minEnemyNumber = 2;
    //房间类型
    private int roomType;
	//宝箱房间号//房间类型号，-2BOSS，-1起始，0无，1宝箱，2商店，3祭坛，4隐藏房间
    private int boxTypeRoom = 1;
	//房间大小，1小，2中，3大
	private int roomSize = 3;
	public int RoomSize
	{
		get { return roomSize; }
		set { roomSize = value; }
	}
    //行列
    private int rows = 3;
    private int columns = 12;
    //墙上物件随机个数，地上物件随机个数
    private Count wallElementsCount = new Count(2, 6);
    private Count groundElementsCount = new Count(2, 6);
    //墙上物件，地上物件
    public GameObject[] wallElements;
    public GameObject[] groundElements;
    public GameObject[] doors;
    public GameObject[] stair;
	public GameObject[] handle;
	//宁静类物品
	public GameObject[] peace;
	//恐怖类物品
	public GameObject[] terror;
	//功能类物品
	public GameObject[] function;
	//祭坛房间
	public GameObject[] altar;
	//商店
	public GameObject[] shop;
	//牌子
	public GameObject[] plate;
    //小怪
    public GameObject[] enemys;
	//Boss
	public GameObject[] boss;
	//金币
	public GameObject[] coin;

    //房间坐标位置
    public int roomX;
    public int roomY;
	//隐藏房间的门
	public bool hiddenDoor = false;
    //门的位置(参数传递设置)
    private int[] doorDirection = new int[4];
    public int[] DoorDirection
    {
        get { return doorDirection; }
        set { doorDirection = value; }
    }
	//大房间的门
    private Vector3 door0 = new Vector3(0f, 1.4f * 2f,  0f);
    private Vector3 door1 = new Vector3(0f, -4.7f * 2f, 0f);
    private Vector3 door2 = new Vector3(-12.8f, -1.46f * 2f, 0f);
    private Vector3 door3 = new Vector3(12.8f,  -1.46f * 2f, 0f);
	//中房间的左右门
	private Vector3 door22 = new Vector3(-8.8f, -1.46f * 2f, 0f);
	private Vector3 door32 = new Vector3(8.8f,  -1.46f * 2f, 0f);
	//小房间的左右门
	private Vector3 door23 = new Vector3(-5.8f, -1.46f * 2f, 0f);
	private Vector3 door33 = new Vector3(5.8f,  -1.46f * 2f, 0f);
    //单个物件长度
    private int objLen = 10;
    //宝箱位置
    private int boxPos = 0;

    private Transform boardHolder;
    //列表ID:0 镜子
    private List<Vector3> mirrorPosition = new List<Vector3>();
    //列表ID:1 图片
    private List<Vector3> picturePosition = new List<Vector3>();
    //列表ID:2 门
    private List<Vector3> doorPosition = new List<Vector3>();
    //列表ID:3 地上物品
    private List<Vector3> groundPosition = new List<Vector3>();
    //列表ID:4 雕像
    private List<Vector3> statuePosition = new List<Vector3>();
    //列表ID:5 爪子
    private List<Vector3> clawPosition = new List<Vector3>();
    //列表ID:6 固定位置
    private List<Vector3> settledPosition = new List<Vector3>();


    void Start()
    {
        EnemyManager.Instance.AddObserver(this);
    }
		

    //墙上物体位置向量
    /* 门Y:1.4       X最小间距2
     * 雕像Y：1.54   X最小间距2
     * 镜子Y：1.88   X最小间距1
     * 图片Y：2.09   X最小间距1
     * */

    //初始化列表
    void InitialiseList()
    {
        //镜子位置列表
        mirrorPosition.Clear();
        for (float x = -1 * columns; x < columns; x+=2f)
        {

            if (x != 0) mirrorPosition.Add(new Vector3(x, 1.8f * 2f, 0f));
        }
        //图片位置列表
        picturePosition.Clear();
        for (float x = -1 * columns; x < columns; x+=2f)
        {
            if (x != 0) picturePosition.Add(new Vector3(x, 2.0f * 2f, 0f));
        }
        //门位置列表
        doorPosition.Clear();

        if (doorDirection[0] == 1)
        {
            doorPosition.Add(door0);
        }
        if (doorDirection[1] == 1)
        {
            doorPosition.Add(door1);
        }
        if (doorDirection[2] == 1)
        {
			if(roomSize == 1) doorPosition.Add(door23);
			else if(roomSize == 2) doorPosition.Add(door22);
            else doorPosition.Add(door2);
        }
        if (doorDirection[3] == 1)
        {
			if(roomSize == 1) doorPosition.Add(door33);
			else if(roomSize == 2) doorPosition.Add(door32);
            else doorPosition.Add(door3);
        }    

        //doorPosition.Add(new Vector3(x, 1.4f, 0f));
       
        //雕像位置列表
        for (float x = -1 * columns; x < columns; x += 2f)
        {
            if (x != 0) statuePosition.Add(new Vector3(x, 1.54f * 2f, 0f));
        }

        //爪子位置列表
        for (float x = -1 * columns; x < columns; x += 2f)
        {
            if (x != 0) clawPosition.Add(new Vector3(x, 0.79f * 2f, 0f));
        }

        //地上物体列表
        groundPosition.Clear();
        for (float x = -1 * columns; x < columns; x+=2f)
        {
            for (float y = -1 * rows; y < 0; y+=2f)
            {
                groundPosition.Add(new Vector3(x, y, 0f));
            }
        }

        //固定位置列表
        settledPosition.Clear();
		switch (roomSize)
		{
		case 1://小房间
			settledPosition.Add(new Vector3(-3.5f, -0.5f, 0f));
			settledPosition.Add(new Vector3(-3.5f, -6.0f, 0f));
			settledPosition.Add(new Vector3(-2.5f, -2.5f, 0f));
			settledPosition.Add(new Vector3(2.5f, -6.5f, 0f));
			settledPosition.Add(new Vector3(3.5f, -2.0f, 0f));
			settledPosition.Add(new Vector3(4.5f, -5.5f, 0f));
			break;
		case 2://中房间
			settledPosition.Add(new Vector3(-6f, -1.5f, 0f));
			settledPosition.Add(new Vector3(-3.5f, -0.5f, 0f));
			settledPosition.Add(new Vector3(-3.5f, -6.0f, 0f));
			settledPosition.Add(new Vector3(-2.5f, -2.5f, 0f));
			settledPosition.Add(new Vector3(2.5f, -6.5f, 0f));
			settledPosition.Add(new Vector3(3.5f, -2.0f, 0f));
			settledPosition.Add(new Vector3(4.5f, -5.5f, 0f));
			settledPosition.Add(new Vector3(6f, -3.5f, 0f));
			break;
		case 3://大房间
			settledPosition.Add(new Vector3(-12f, -0.5f, 0f));
			settledPosition.Add(new Vector3(-11f, -6.0f, 0f));
			settledPosition.Add(new Vector3(-9f, -3f, 0f));
			settledPosition.Add(new Vector3(-6f, -1.5f, 0f));
			settledPosition.Add(new Vector3(-3.5f, -0.5f, 0f));
			settledPosition.Add(new Vector3(-3.5f, -6.0f, 0f));
			settledPosition.Add(new Vector3(-2.5f, -2.5f, 0f));
			settledPosition.Add(new Vector3(2.5f, -6.5f, 0f));
			settledPosition.Add(new Vector3(3.5f, -2.0f, 0f));
			settledPosition.Add(new Vector3(4.5f, -5.5f, 0f));
			settledPosition.Add(new Vector3(6f, -3.5f, 0f));
			settledPosition.Add(new Vector3(9f, -0.5f, 0f));
			settledPosition.Add(new Vector3(9f, -6.5f, 0f));
			settledPosition.Add(new Vector3(12f, -6.0f, 0f));
			break;
		}
       

    }



    //随机位置
    Vector3 RandomPosition(int wallElementID)
    {
        int randomIndex;
        Vector3 randomPosition=new Vector3(0f,0f,0f);
        switch (wallElementID)
        {
            //镜子位置
            case 0:
                randomIndex = Random.Range(0, mirrorPosition.Count);
                randomPosition = mirrorPosition[randomIndex];
                RemoveWallPosition(randomIndex);
                break;
            //图片位置
			case 1:
				randomIndex = Random.Range (0, picturePosition.Count);
				randomPosition = picturePosition [randomIndex];
                RemoveWallPosition(randomIndex);
                break;
            //雕像位置
            case 2:
                randomIndex = Random.Range(0, statuePosition.Count);
                randomPosition = statuePosition[randomIndex];
                RemoveWallPosition(randomIndex);
                break;
            //地上物体位置
            case 3:
                //randomIndex = Random.Range(0, groundPosition.Count);
                //randomPosition = groundPosition[randomIndex];
                //groundPosition.RemoveAt(randomIndex);
                randomIndex = Random.Range(0, settledPosition.Count);
                randomPosition = settledPosition[randomIndex];
                settledPosition.RemoveAt(randomIndex);
                break;
            //爪子位置
            case 4:
                randomIndex = Random.Range(0, clawPosition.Count);
                randomPosition = clawPosition[randomIndex];
                RemoveWallPosition(randomIndex);
                break;

        }
        return randomPosition;
    }

    //移除墙上物体占用位置
    void RemoveWallPosition(int randomIndex)
    {
        mirrorPosition.RemoveAt(randomIndex);
        picturePosition.RemoveAt(randomIndex);
        //doorPosition.RemoveAt(randomIndex);
        statuePosition.RemoveAt(randomIndex);
        clawPosition.RemoveAt(randomIndex);
    }


    //随机布局墙上物体
    void LayoutWallAtRandom(GameObject[] objectArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);
        Vector3 randomPosition = new Vector3(0f, 0f, 0f);
        for (int i = 0; i < objectCount; i++)
        {
            
            int wallElementsID = Random.Range(0, objectArray.Length);
            //放置镜子
            if(wallElementsID==0) randomPosition = RandomPosition(0);
            //放置图片
            if (wallElementsID == 3||wallElementsID==4) randomPosition = RandomPosition(1);
            //放置雕像
            if (wallElementsID == 1)
            {
                randomPosition = RandomPosition(2);
            }
            //放置爪子
            if (wallElementsID == 2)
            {
                randomPosition = RandomPosition(4);
            }

            GameObject objectChoice = objectArray[wallElementsID];
            GameObject roomElement = Instantiate(objectChoice, randomPosition, Quaternion.identity) as GameObject;
            roomElement.transform.SetParent(GameObject.Find("WallElements").transform);
            //房间物件存入列表

           // RoomElementManager.Instance.RoomElementList.Add(roomElement.GetComponent<RoomElement>());

        }
    }
    //随机布局地上物体
    void LayoutGroundAtRandom(GameObject[] objectArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);
        for (int i = 0; i < objectCount; i++)
        {
			if (roomSize < 3)
				objectCount--;
            GameObject objectChoice;
            Vector3 randomPosition = RandomPosition(3);
			//房间类型号，-2BOSS，-1起始，0无，1宝箱，2商店，3祭坛，4隐藏房间
			//宝箱房
			if (roomType == (int)RmType.Box) {
				objectCount--;
				objectChoice = objectArray [objectArray.Length - 1];
			} 
			//商店
			else if (roomType == (int)RmType.Shop) {
				// 地摊摊主
				objectCount = -1;
				randomPosition = new Vector3 (0, -2f, -2f);
				objectChoice = shop [Random.Range (0, shop.Length)];
				//道具
				Vector3 pricePosition;
				Vector3[] itemPosition = {
					new Vector3(-3f,-0.5f,0f),new Vector3(-3f,-4f,0f),new Vector3(-3f,-7.5f,0f),
					new Vector3( 3f,-0.5f,0f),new Vector3( 3f,-4f,0f),new Vector3( 3f,-7.5f,0f)};
				ItemManager.Instance.ItemsTransform = this.transform;

				for (int j = 0; j < 6; j++) {
					ItemManager.Instance.ItemsTransform.position = itemPosition[j];
					var shopItem = ItemManager.Instance.CreateItemDrop(false, false, true,itemPosition[j]);
					shopItem.Value = Random.Range (5,11);
					//牌子
					pricePosition = new Vector3 (itemPosition [j].x, itemPosition [j].y + 0.5f, 0f);
					GameObject itemPlate = Instantiate(plate[0], pricePosition, Quaternion.identity) as GameObject;
					itemPlate.transform.SetParent(GameObject.Find("GroundElements").transform);
					itemPlate.GetComponent<Plate> ().SetPrice (shopItem.Value);

				}
			}
			//祭坛
			else if (roomType == (int)RmType.Altar) {
				//主祭坛
				objectCount = -1;
				randomPosition = new Vector3 (0, -2f, -2f);
				objectChoice = altar [Random.Range (0, altar.Length)];
				//祭坛其他摆设
				GameObject elseChoice;
				Vector3 elsePosition;
				//摆设1
				elseChoice = terror [Random.Range (0, terror.Length)];
				elsePosition = new Vector3 (-7f, -1.5f, -1.5f);
				GameObject altarElse1 = Instantiate(elseChoice, elsePosition, Quaternion.identity) as GameObject;
				altarElse1.transform.SetParent(GameObject.Find("GroundElements").transform);
				//摆设2
				elseChoice = terror [Random.Range (0, terror.Length)];
				elsePosition = new Vector3 (7f, -1.5f, -1.5f);
				GameObject altarElse2 = Instantiate(elseChoice, elsePosition, Quaternion.identity) as GameObject;
				altarElse2.transform.SetParent(GameObject.Find("GroundElements").transform);
			} 
			//其他
			else if(roomType >= (int)RmType.Normal){
				objectChoice = objectArray[Random.Range(objLen, objectArray.Length-1)];
				//随机生成机关开关
				LayoutHandle();
			}
			else objectChoice = objectArray[Random.Range(objLen, objectArray.Length-1)];

            GameObject roomElement = Instantiate(objectChoice, randomPosition, Quaternion.identity) as GameObject;
            roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);

        }
    }
    //随机布局小怪
    void LayoutEnemyAtRandom(GameObject[] objectArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);
        int randomIndex = Random.Range(0, groundPosition.Count) ;
        for (int i = 0; i < objectCount; i++)
        {
            GameObject objectChoice;
            Vector3 randomPosition = groundPosition[randomIndex];
            //groundPosition.RemoveAt(randomIndex);
			if(roomType >= (int)RmType.Normal||roomType == (int)RmType.Boss){
                objectChoice = objectArray[Random.Range(0, objectArray.Length)];
				//遗传算法
				Genetic(objectChoice);

                GameObject enemy = Instantiate(objectChoice, randomPosition, Quaternion.identity) as GameObject;
                enemy.transform.SetParent(GameObject.Find("GroundElements").transform);
                //小怪存入列表
                //EnemyManager.Instance.EnemyList.Add(enemy.GetComponent<Character>());

                if (randomIndex < groundPosition.Count - 1 && groundPosition[randomIndex] != null) randomIndex++;
                else if (randomIndex > 0 && groundPosition[0] != null) randomIndex--;
            }
            
        }
    }
    //设置门的位置
    public void SetDoorDierction(int[] doorDir)
    {
        for (int i = 0; i < 4; i++)
        {
            doorDirection[i] = doorDir[i];
        }
    }
    //布局门
    void LayoutDoor()
    {
        int j = 0;
        for (int i = 0; i < 4; i++)
        {
			if (doorDirection [i] > 0) {
				GameObject objectChoice;
				//下门
				if (i == 1) {

					//非正常门
					if(CheckpointManager.Instance.GetNextRoom (roomX + 1, roomY)!=null)
					{
						//BOSS门
						if (CheckpointManager.Instance.GetNextRoom (roomX + 1, roomY).type == -2)
							objectChoice = doors [5];
						//隐藏门
						else if (CheckpointManager.Instance.GetNextRoom (roomX + 1, roomY).type == -3) 
						{
							if (hiddenDoor)
								objectChoice = doors [0];
							else objectChoice = null;
						}
						//正常门
						else
							objectChoice = doors [0];
					}

					//正常门
					else
						objectChoice = doors [0];

				}
				//上门
				else if (i == 0)
				{
					//非正常门
					if (CheckpointManager.Instance.GetNextRoom (roomX - 1, roomY) != null) 
					{
						//BOSS门
						if (CheckpointManager.Instance.GetNextRoom (roomX - 1, roomY).type == -2)
							objectChoice = doors [4];
						//隐藏门
						else if (CheckpointManager.Instance.GetNextRoom (roomX - 1, roomY).type == -3) 
						{
							if (hiddenDoor)
								objectChoice = doors [1];
							else objectChoice = null;
						}
						//正常门
						else
							objectChoice = doors [1];
					}

					//正常门
					else
						objectChoice = doors [1];
				}

				//左右BOSS门
				else if ((i == 2 && CheckpointManager.Instance.GetNextRoom (roomX, roomY - 1).type == -2)
				         || (i == 3 && CheckpointManager.Instance.GetNextRoom (roomX, roomY + 1).type == -2))
					objectChoice = doors [2];
				//左隐藏门
				else if (i == 2 && CheckpointManager.Instance.GetNextRoom (roomX, roomY - 1).type == -3)
				{
					if (hiddenDoor) objectChoice = doors [3];
					else objectChoice = null;
				}
				//右正常门
				else objectChoice = doors [3];
                //生成
				if (objectChoice != null) 
				{
					GameObject roomElement = Instantiate (objectChoice, doorPosition [j], Quaternion.identity) as GameObject;
					roomElement.GetComponent<Door> ().SetPosition (i);
					roomElement.transform.SetParent (GameObject.Find ("WallElements").transform);
				}
				j++;
			} 
        }
    }		

    //设置房间位置
    void SetRoomXY(int x, int y, int tp)
    {
        roomX = x;
        roomY = y;
        roomType = tp;
    }

    //特定生成函数中门位置
	void InitDoorList(int x, int y)
    {
        doorPosition.Clear();

        if (doorDirection[0] == 1)
        {
            doorPosition.Add(door0);
        }
        if (doorDirection[1] == 1)
        {
            doorPosition.Add(door1);
        }
        if (doorDirection[2] == 1)
        {
			if (CheckpointManager.Instance.GetNextRoom (x, y).roomSize == 1)
				doorPosition.Add (door23);
			else if (CheckpointManager.Instance.GetNextRoom (x, y).roomSize == 2)
				doorPosition.Add (door22);
			else
				doorPosition.Add (door2);
        }
        if (doorDirection[3] == 1)
        {
			if (CheckpointManager.Instance.GetNextRoom (x, y).roomSize == 1)
				doorPosition.Add (door33);
			else if (CheckpointManager.Instance.GetNextRoom (x, y).roomSize == 2)
				doorPosition.Add (door32);
			else
				doorPosition.Add (door3);
        }    
    }

    //在最后一个房间生成楼梯
    void LayoutStair()
    {
		Vector3 position;
		if(roomSize == 1)
        	position = new Vector3(4.5f,-0.8f,0f);
		else if(roomSize == 2)
			position = new Vector3(7.5f,-0.8f,0f);
		else 
			position = new Vector3(11.5f,-0.8f,0f);
        GameObject objectChoice = stair[0];
        GameObject roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
        roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);

    }

	//随机生成机关开关
	void LayoutHandle()
	{
		if (Random.Range (0, 10) > 0 && 
			(roomX != CheckpointManager.Instance.hiddenRoomX || roomY !=CheckpointManager.Instance.hiddenRoomY+1 )) 
		{
			Vector3 handlePosition = RandomPosition(3);
			GameObject myHandle = Instantiate(handle[0], handlePosition, Quaternion.identity) as GameObject;
			myHandle.transform.SetParent(GameObject.Find("GroundElements").transform);
		}
	}

    //清空
    public void ClearAll()
    {
        //清除位置
        mirrorPosition.Clear();
        picturePosition.Clear();
        doorPosition.Clear();
        groundPosition.Clear();
        statuePosition.Clear();
        clawPosition.Clear();
        //清除物件
        RoomElementManager.Instance.ClearAll();
        //清除敌人
       // EnemyManager.Instance.ClearAll();

    }

    //设置场景,类型号，门位置,房间x，房间y，房间大小r
	public void SetupScene(int tp, int[] dp, int x, int y,int r,bool isClear=true)
    {
		roomSize = r;
        if(isClear)
            ClearAll();
        SetDoorDierction(dp);
        InitialiseList();
        SetRoomXY(x, y, tp);
        LayoutEnemyAtRandom(enemys, minEnemyNumber, maxEnemyNumber);
        LayoutWallAtRandom(wallElements, wallElementsCount.minimum, wallElementsCount.maximum);
        LayoutGroundAtRandom(groundElements, groundElementsCount.minimum, groundElementsCount.maximum);
        LayoutDoor();
		GameManager.Instance.LayoutWall (x, y);

        //if (x == CheckpointManager.Instance.rows - 1 && y == CheckpointManager.Instance.columns - 1)
        if(tp == -2)
        {
            LayoutStair();
            //布局BOSS，测试小怪
            LayoutEnemyAtRandom(boss, 1, 1);
            SoundManager.Instance.PlayBackGroundMusic(BackgroundMusic[1]);
        }
        else
            SoundManager.Instance.PlayBackGroundMusic(BackgroundMusic[0]);

        //Debug.Log("lIST" + RoomElementManager.Instance.RoomElementList[0].RoomElementID);
        //Notify("EnterRoom;Unknow");
    }

    //小怪数目为0,生成一个宝箱
    public override void OnNotify(string msg)
    {
        string content = UtilManager.Instance.GetMsgField(msg, 0);
        //string[] str = UtilManager.Instance.GetMsgFields(msg);
        if (content == "ClearRoom")
        {
            //Debug.Log("CLEAR ROOM CLEAR ROOM");
            Vector3 randomPosition = RandomPosition(3);
            GameObject objectChoice = groundElements[boxPos];
            GameObject roomElement = Instantiate(objectChoice, randomPosition, Quaternion.identity) as GameObject;
            //roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
        }
    }

	//遗传算法
	void Genetic(GameObject objectChoice)
	{

	}









    //加载确定场景 类型号tp,房间号xy,门位置 int数组roomX[],roomY[],id[],posiX[],posiY[],posiZ[],房间大小roomS
	public void LoadScene(int tp,int x,int y,int[] dp,int[] roomX,int[] roomY,int[] id,float[] posiX,float[] posiY,float[] posiZ,int roomS)
    {
        //Debug.Log("LoadScene进入" + x + ";" + y);
        ClearAll();

        SetDoorDierction(dp);
		InitDoorList(x, y);
        SetRoomXY(x, y, tp);
		LayoutDoor();
		GameManager.Instance.LayoutWall (x, y);
		if (tp == -2)
        {
            LayoutStair();
        }
		roomSize = roomS;

        int count = 0;
        for (int i = 0; i < roomX.Length; i++)
        {
         
            {
                if (roomY[i] == y && roomX[i] == x)
                {
                    //Debug.Log("选中物体:"+id[count]);
                    count = i;
                    Vector3 position = new Vector3(posiX[count],posiY[count],posiZ[count]);
                    GameObject objectChoice;
                    GameObject roomElement = null;

					int roomElementState = ProfileManager.Instance.Data.RoomElementState[count];
					//if(roomElementState>0)Debug.Log ("状态位:"+roomElementState);

					if (id [count] >= 1000 && id [count] < 2000) 
					{
						//ItemManager.Instance.ItemsTransform = this.transform;
						//ItemManager.Instance.ItemsTransform.position = position;
						//ItemManager.Instance.CreateItemID(id[count]);
						ItemManager.Instance.CreateItemID (id [count], position);
						//Debug.Log ("Create item ID:   " + id [count]);
					} 
					else if (id [count] >= 2100 && id [count] < 3000) 
					{
						//Debug.Log ("生成小怪:" + count);
						objectChoice = enemys[Random.Range(0,enemys.Length)];
						roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
						roomElement.transform.SetParent(GameObject.Find("GroundElements").transform); 
					}
					else 
                    switch (id[count])
                    {
                        case 0:
                            //Missile
                            break;
						case (int)RoomElement.REID.Box:
                            //Debug.Log("选中箱子");
							objectChoice = groundElements [0];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							roomElement.GetComponent<Box> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Mirror:
                            //Debug.Log("选中镜子");
                            objectChoice = wallElements[0];
                            roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            roomElement.transform.SetParent(GameObject.Find("WallElements").transform);
							roomElement.GetComponent<Mirror> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Door:
                            //objectChoice = doors[1];
                            //roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            //roomElement.transform.SetParent(GameObject.Find("WallElements").transform);
                            break;
						case (int)RoomElement.REID.Statue:
                            //Debug.Log("选中雕塑");
							objectChoice = wallElements [1];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("WallElements").transform);
							roomElement.GetComponent<Statue> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Claw:
                            //Debug.Log("选中爪子");
							objectChoice = wallElements [2];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("WallElements").transform);
							roomElement.GetComponent<Claw> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Picture1:
                            //Debug.Log("选中图一");
							objectChoice = wallElements [3];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("WallElements").transform);
						Debug.Log ("选中图1，Y：" + position.y);
							//roomElement.transform.localPosition = position;
							roomElement.GetComponent<Picture1> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Picture2:
                            //Debug.Log("选中图二");
							objectChoice = wallElements [4];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("WallElements").transform);
							roomElement.GetComponent<Picture2> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Skull:
                            //Debug.Log("选中骷髅");
							objectChoice = groundElements [1];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Skull> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.SkullLight:
                            //Debug.Log("选中骷髅灯");
							objectChoice = groundElements [2];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<SkullLight> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Bottle1:
                            //Debug.Log("选中瓶子一");
							objectChoice = groundElements [5];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Bottle1> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Bottle2:
                            //Debug.Log("选中瓶子二");
							objectChoice = groundElements [6];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							//roomElement.transform.localPosition = position;
							roomElement.GetComponent<Bottle2> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Gone:
                            //Debug.Log("骨头");
                            objectChoice = groundElements[4];
                            roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
                            roomElement.transform.localPosition = position;
							roomElement.GetComponent<Bone> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Rod:
                            //Debug.Log("选中杆子");
                            objectChoice = groundElements[7];
                            roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
                            roomElement.transform.localPosition = position;
							roomElement.GetComponent<Rod> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Stone:
                            //Debug.Log("选中石头");
                            objectChoice = groundElements[8];
                            roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
                            roomElement.transform.localPosition = position;
							roomElement.GetComponent<Stone> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Trap:
                            //Debug.Log("选中陷阱");
                            objectChoice = groundElements[9];
                            roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
                            roomElement.transform.localPosition = position;
							roomElement.GetComponent<Trap> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Stair:
                            //Debug.Log("选中楼梯");
                            objectChoice =stair[0];
                            roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
                            roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
                            roomElement.transform.localPosition = position;
							roomElement.GetComponent<Stair> ().RoomElementState = roomElementState;
                            break;
						case (int)RoomElement.REID.Altar1:
							//Debug.Log("选中祭坛1");
							objectChoice =altar[0];
							roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Altar> ().RoomElementState = roomElementState;
							break;
						case (int)RoomElement.REID.Altar2:
							//Debug.Log("选中祭坛2");
							objectChoice =altar[1];
							roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Altar> ().RoomElementState = roomElementState;
							break;
						case (int)RoomElement.REID.Shop:
							//Debug.Log("选中商店");
							objectChoice =shop[0];
							roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Shop> ().RoomElementState = roomElementState;
							break;
						case (int)RoomElement.REID.Plate:
							//Debug.Log("选中牌子");
							objectChoice =plate[0];
							roomElement = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent(GameObject.Find("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Plate> ().RoomElementState = roomElementState;
							break;
						case (int)RoomElement.REID.Coin:
							//Debug.Log("选中金币");
							objectChoice = coin [0];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							roomElement.transform.localPosition = position;
							///roomElement.GetComponent<Coin> ().RoomElementState = roomElementState;
							break;
						case (int)RoomElement.REID.Handle:
							//Debug.Log("选中机关开关");
							objectChoice = handle [0];
							roomElement = Instantiate (objectChoice, position, Quaternion.identity) as GameObject;
							roomElement.transform.SetParent (GameObject.Find ("GroundElements").transform);
							roomElement.transform.localPosition = position;
							roomElement.GetComponent<Handle> ().RoomElementState = roomElementState;
							break;

                    } 

                }
            }
        }
        //Notify("EnterRoom;Know");
    }


    //加载小怪 房间号 int数组roomX[],roomY[],id[],posiX[],posiY[],posiZ[]
    public void LoadEnemy(int x, int y, int[] id, float[] posiX, float[] posiY, float[] posiZ)
    {
        for (int count = 0; count < posiX.Length; count++)
        {

            Vector3 position = new Vector3(posiX[count], posiY[count], posiZ[count]);
            GameObject objectChoice;
            GameObject enemy;

			objectChoice = enemys[Random.Range(0,enemys.Length)];
            enemy = Instantiate(objectChoice, position, Quaternion.identity) as GameObject;
            enemy.transform.SetParent(GameObject.Find("GroundElements").transform);
        }

       //Notify("EnterRoom;Know");
    }


    public void DropBox(Vector3 position, int minNum = 1, int maxNum = 2)
    {

        Vector3 startPoint = position + new Vector3(0, 1, 0);
        int num = Random.Range(minNum, maxNum);
        for (int i = 0; i < num; i++)
        {
            Vector3 deltaPos = new Vector3((Random.value - 0.5f) * 5, (Random.value - 0.5f) * 5);
            GameObject ins = Instantiate(groundElements[boxPos], startPoint, Quaternion.identity) as GameObject;
            Vector3[] paths = new Vector3[3];
            paths[0] = startPoint;
            paths[1] = startPoint + deltaPos / 3 + new Vector3(0, 1.5f, 0);
            paths[2] = startPoint + deltaPos;
            iTween.MoveTo(ins, iTween.Hash("path", paths, "speed", 20f, "easeType", iTween.EaseType.linear));
        }
    }
}
