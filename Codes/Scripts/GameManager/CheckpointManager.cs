﻿using UnityEngine;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class CheckpointManager : ExUnitySingleton<CheckpointManager>
{
    #region Test Part
    //测试用
    public bool isTest = false;
    public RmType testRoomType = RmType.Box;
    int[,] TestMap= { { 0, (int)RmType.Test, 0 },{ (int)RmType.Test, (int)RmType.Start, (int)RmType.Test },{ 0, (int)RmType.Test, 0 } };
    #endregion

    #region Variables
    //关卡号1-5
    private int checkpointNumber = 0;
    public int CheckpointNumber
    {
        get { return checkpointNumber; }
        set { checkpointNumber = value; }
    }

    //行列
    public int rows = 3;
    public int columns = 3;

    //房间分布
    public int[,] roomArray;
    //周围房间0：上方，1：下方，2：左方，3：右方
    public int[] surroundRoom = new int[4];
    //房间信息列表,存放RoomInfo
    public List<RoomInfo> roomList = new List<RoomInfo>();
    #endregion

    #region Methods
    //获取位于地图坐标为X,Y的房间对象
    public RoomInfo GetRoomInfo(int x, int y)
    {
   
            for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].roomX == x && roomList[i].roomY==y)
            {
                return roomList[i];
            }
        }
        return null;
    }


    #region GenerateRoomLayout Algothrim
    //初始化房间分布列表
    void InitalRoomLayout()
	{
		for (int i = 1; i < rows-1; i++)
		{
			for (int j = 1; j < columns-1; j++)
			{
				if (i == 1 && j == 1 || i == rows - 2 && j == columns - 2)
				{
					roomArray[i, j] = 1;
				}
				if (roomArray[i, j] == 1)
				{

					int randomDerection = Random.Range(0, 10);
					//0,1,2向右走,走到最右向下走
					if (randomDerection <= 2)
					{
						if (i + 1 < rows-1)
							roomArray[i + 1, j] = 1;
						else if (j + 1 < columns-1)
							roomArray[i, j + 1] = 1;
					}
					//3,4,5向下走，走到最下向右走
					else if (randomDerection <= 5 && randomDerection >= 3)
					{
						if (j + 1 < columns-1)
							roomArray[i, j + 1] = 1;
						else if (i + 1 < rows-1)
							roomArray[i + 1, j] = 1;
					}
					//6,7,8向下和向右走
					else if (randomDerection <= 8 && randomDerection >= 6)
					{
						if (i + 1 < rows-1)
							roomArray[i + 1, j] = 1;
						if (j + 1 < columns-1)
							roomArray[i, j + 1] = 1;
					}
					//9向下、向右和向左走
					else
					{
						if (i + 1 < rows-1)
							roomArray[i + 1, j] = 1;
						if (j + 1 < columns-1)
							roomArray[i, j + 1] = 1;
						if (i > 0)
							roomArray[i - 1, j] = 1;
					}
				}
			}
		}

		//扩充矩阵行
		for (int i = 0; i < columns; i++) 
		{
			if(roomArray[1,i]>0)
			{
                int num = GetNearRoomCount(0, i);
                float rand = Random.value;
                if(rand<(1.15-num*0.15))
                    roomArray [0, i] = 1;
			}
			if(roomArray[rows-2,i]>0)
			{
                int num = GetNearRoomCount(rows-1, i);
                float rand = Random.value;
                if (rand < (1.15 - num * 0.15))
                    roomArray[rows-1, i] = 1;
            }
		}
		//扩充矩阵列
		for (int i = 0; i < rows; i++) 
		{
			if(roomArray[i,1]>0)
			{
                int num = GetNearRoomCount(i, 0);
                float rand = Random.value;
                if (rand < (1.15 - num * 0.15))
                    roomArray[i, 0] = 1;
            }
			if(roomArray[i,columns-2]>0)
			{
                int num = GetNearRoomCount(i, columns-1);
                float rand = Random.value;
                if (rand < (1.15 - num * 0.15))
                    roomArray[i, columns - 1] = 1;
            }
		}


        //消除拥挤的房间
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                if (GetNearRoomCount(i, j) >= 8) roomArray[i, j] = 0;

        Notify("MapComplete");
	}

    //检索roomArray[x,y]邻近房间
    int GetNearRoomCount(int x, int y) 
    {
        int count = 0;
        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
                if (x + i >= 0 && y + j >= 0 && x + i < rows && y + j < columns)
                {
                    if (roomArray[x + i, y + j] >= 1) count++;
                }
        return count;
    }
    #endregion

    /// <summary>
    /// 使用预设地图
    /// </summary>
    /// <param name="map">预设地图</param>
    /// <param name="isTyped">是否已经赋予类型</param>
    void UsePresetMap(int[,] map,int r,int c,bool isTyped)
    {
        roomArray = map;
        columns = r;
        rows = c;
    }


    //检索roomArray[x,y]周围房间,用于生成门
    int SetSurroundRoom(int x, int y)
    {
        for (int i = 0; i < 4;i++ )
            surroundRoom[i] = 0;
        int count=0;
        //检查右侧
        if (y+1<columns)
        {
            if (roomArray[x, y + 1] != 0)
            {
                surroundRoom[3] = 1;
                count++;
            }
        }
        //检查左侧
        if (y - 1 >= 0)
        {
            if (roomArray[x, y - 1] != 0)
            {
                surroundRoom[2] = 1;
                count++;
            }
        }
        //检查上侧
        if (x - 1 >= 0)
        {
            if (roomArray[x - 1, y] != 0)
            {
                surroundRoom[0] = 1;
                count++;
            }
        }
        //检查下侧
        if (x + 1 < rows)
        {
            if (roomArray[x + 1, y] != 0)
            {
                surroundRoom[1] = 1;
                count++;
            }
        }
        return count;
    }


    public RoomInfo[] GetRoomBySurroundRoomCount(int count)
    {
        List<RoomInfo> rooms = new List<RoomInfo>();
        for (int i = 0; i < roomList.Count; i++)
        {


            if(roomList[i].count== count)
                rooms.Add(roomList[i]);
        }
        return rooms.ToArray();

    }
    /// <summary>
    /// 根据房间类型获取Room,返回第一个找到的Room
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public RoomInfo GetRoomByType(RmType type)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].type == type)
            {
                return roomList[i];
            }
        }
        Debug.Log("info is nukl!");
        return null;
    }
    /// <summary>
    /// 根据当前关卡中所有满足指定Type的房间
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public RoomInfo[] GetRoomsByType(RmType type)
    {
        List<RoomInfo> rooms = new List<RoomInfo>();
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].type == type)
            {
                rooms.Add(roomList[i]);
            }
        }
        return rooms.ToArray();
    }


     void  UpdateRoomArray()
    {
        for(int i=0;i<roomList.Count;i++)
        {
           // Debug.Log("X:" + roomList[i].roomX + roomList[i].roomY + "t:" + roomList[i].type);
            roomArray[roomList[i].roomX, roomList[i].roomY] = (int)roomList[i].type;
        }
    }

    #region SutupOrLoadCheckPoint

    //设置关卡地图大小
    public void SetMapSize(int x, int y)
    {
        rows = x;
        columns = y;
    }


    public void SetupPresetCheckpoint()
    {
        roomList.Clear();
        CheckpointNumber = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {

                if (roomArray[i, j] != 0)
                {
                    int surroundRoomNumber = SetSurroundRoom(i, j);
                    //roomArray[i, j] = (int)RmType.Normal;
                    //添加房间类
                    var info = new RoomInfo((RmType)roomArray[i, j], i, j, surroundRoom, 0, 1, surroundRoomNumber);
                    roomList.Add(info);

                }
            }
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            //   100
            //101 102 103
            //   104
            // RoomManager.Instance.SetupRoom(roomList[i]);
            RoomManager.Instance.SetupPresetRoom(roomList[i],false,100+i);
        }
        LogRoomArray();
        Notify("SetupCheckpoint;" + CheckpointNumber);
        Debug.Log("11+" + GetRoomByType(RmType.Start));
        //载入初始房间

        {
            RoomManager.Instance.LoadRoom(GetRoomByType(RmType.Start));
        }

    }

    //设置关卡
    public void SetupCheckpoint(bool shouldSetupStartRoom=true)
    {
        if(isTest)
        {
            UsePresetMap(TestMap,3,3, true);
            SetupPresetCheckpoint();
            isTest = false;
            return;
        }

        roomList.Clear();
        CheckpointNumber++;
		if (rows < 6 && columns < 6) 
		{
			rows++;
			columns++;
		}
		roomArray = new int[rows, columns];
	
        InitalRoomLayout();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
               
                if (roomArray[i,j]!=0)
                {
                    int surroundRoomNumber = SetSurroundRoom(i, j);
		

                    roomArray[i, j] = (int)RmType.Normal;
                    //添加房间类
                    var info = new RoomInfo(RmType.Normal, i, j, surroundRoom, 0, 1,surroundRoomNumber);                 
                    roomList.Add(info);
               
                }
            }
        }

        //设置起始位置
      
        int startNum = Random.Range(4, roomList.Count - 4);
        if(startNum>roomList.Count|| startNum <0)
        {
            Debug.Assert(startNum < roomList.Count, "起始位置超了");
            Debug.Assert(startNum > 0, "起始位置<0");
            roomList[0].type = RmType.Start;
        }
        else
            roomList[startNum].type =RmType.Start;




        //设置BOSS位置
        int bossNum;
        if (startNum > roomList.Count - startNum)
        {
            bossNum = Random.Range(0, startNum / 3);
        }
        else
        {
            bossNum = Random.Range(startNum + (roomList.Count - startNum) / 3 * 2, roomList.Count);
        }
        Debug.Log(151);
        roomList[bossNum].type = (RmType.Boss);


        ////设置隐藏房间类型
        //      //TEMP
        //if (hiddenRoomX > 0||(CheckpointNumber==1&&preset))W
        //{
        //	GetRoomInfo (hiddenRoomX, hiddenRoomY).type = RmType.Hidden;
        //	roomArray [hiddenRoomX, hiddenRoomY] = -3;
        //}

        var rooms = GetRoomBySurroundRoomCount(1);
        for(int i=0;i<rooms.Length;i++)
        {
            if(rooms[i].type==RmType.Normal)
            {
                var rand = Random.value;
                if (rand > 0.67f)
                    rooms[i].type = RmType.Shop;
                else if(rand>0.33f)
                    rooms[i].type = RmType.Box;
                else
                    rooms[i].type = RmType.Altar;
            }
        }

        UpdateRoomArray();




        LogRoomArray();

        //生成每个房间的RoomElements信息
        for (int i=0;i<roomList.Count;i++)
        {
         
            // RoomManager.Instance.SetupRoom(roomList[i]);
            RoomManager.Instance.SetupPresetRoom(roomList[i]);
        }

        Notify("SetupCheckpoint;" + CheckpointNumber);

        //载入初始房间
        if(shouldSetupStartRoom)
        {
            RoomManager.Instance.LoadRoom(GetRoomByType(RmType.Start));
        }
      

        
    }


    public void LogRoomArray()
    {
        //输出房间布局
        string str = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                str += (RmType)roomArray[i, j] + "  ";
            }
            str += "\n";
        }
        //Debug.Log ("ROW:" + rows + "COL:" + columns);
        Debug.Log(str);
    }

    //载入关卡
    public void LoadCheckpoint(bool shouldLoadRoom = true)
    {
        LoadCheckpoint(ProfileManager.Instance.Data.Map, ProfileManager.Instance.Data.IsRoomPassed, ProfileManager.Instance.Data.RoomSize,shouldLoadRoom);
    } 
    public void LoadCheckpoint(int[] r, int[] isPass, int[] rms,bool shouldLoadRoom=true )
    {
        roomList.Clear();
        //CheckpointNumber++;
		if (rows < 6 && columns < 6) 
		{
			rows++;
			columns++;
		}
        roomArray = new int[rows, columns];
        //载入房间布局

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                roomArray[i,j]=r[i*columns+j];

            }
        }
        //载入房间信息
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {            
                if (roomArray[i,j]!=0)
                {
                     var count=SetSurroundRoom(i, j);
                    var info = new RoomInfo
                        ((RmType)r[i * columns + j], i, j, surroundRoom, isPass[i * columns + j], rms[i * columns + j],count);
                    //int type = Random.Range(1, 6);
                    roomList.Add(info);
					
                }

            }
        }

        //载入房间物品信息
        var data = ProfileManager.Instance.Data;
        for (int i=0;i<data.RoomElementID.Length;i++)
        {
            var roominfo=GetRoomInfo(data.RoomElementRoomX[i], data.RoomElementRoomY[i]);
            roominfo.roomElementInfoList = new List<RoomElementInfo>(data.RoomElementID.Length);
            roominfo.roomElementInfoList.Add(new RoomElementInfo((REID)data.RoomElementID[i],                                      
                                                                 new Vector3(data.RoomElementPosX[i],data.RoomElementPosY[i],data.RoomElementPosZ[i]),
                                                                 data.RoomElementState[i]));
        }

        if(shouldLoadRoom)
             RoomManager.Instance.LoadRoom(data.CurRoomX,data.CurRoomY);

        //输出房间布局
        string str = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                str += roomArray[i, j].ToString();
            }
            str += "\n";
        }
        Debug.Log(str);
    }
    #endregion
    #endregion
}