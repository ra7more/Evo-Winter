﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class CheckpointManager : ExUnitySingleton<CheckpointManager>
{
    [Serializable]

    //房间分布类
    public class Room
    {
        //房间类型号
        public int type = 0;
        //是否进入过该房间，0否，1是
        public int pass = 0;
        //房间位置x,y
        public int roomX;
        public int roomY;
        //门位置
        public int[] doorDirection = new int[4];
		//房间大小，1小，2中，3大
		public int roomSize = 3;
		public int RoomSize
		{
			get { return roomSize; }
			set { roomSize = value; }
		}
        //构造函数
		public Room(int tp, int x, int y,int[] doorDir,int ps, int rs)
        {
            pass = ps;
            type = tp;
            roomX = x;
            roomY = y;
			roomSize = rs;
            for (int i = 0; i < 4;i++ )
                doorDirection[i] = doorDir[i];
        }
        //设置通过标志
        public void SetPass(int ps)
        {
            pass = ps;
        }
    }
		
	//测试用
	public bool isTest = false;
	public RoomManager.RmType testRoomType = RoomManager.RmType.Box;
	//是否用预设的房间分布
	public bool preset = false;

    //关卡号1-5
    private int checkpointNumber = 0;
    public int CheckpointNumber
    {
        get { return checkpointNumber; }
        set { checkpointNumber = value; }
    }

    //行列
    public int rows = 6;
    public int columns = 6;
	//隐藏房间位置
	public int hiddenRoomX = 0;
	public int hiddenRoomY = 0;
    //房间分布
    public int[,] roomArray;
    //周围房间0：上方，1：下方，2：左方，3：右方
    public int[] surroundRoom = new int[4];
    //房间列表,存放Room类
    public List<Room> roomList = new List<Room>();
    
    //获取下个房间类
    public Room GetNextRoom(int x, int y)
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

    //初始化房间分布列表
//    void InitalRoomLayout()
//    {
//        for (int i = 0; i < rows; i++)
//        {
//            for (int j = 0; j < columns; j++)
//            {
//                if (i == 0 && j == 0 || i == rows - 1 && j == columns - 1)
//                {
//                    roomArray[i, j] = 1;
//                }
//                if (roomArray[i, j] == 1)
//                {
//
//                    int randomDerection = Random.Range(0, 10);
//                    //0,1,2向右走,走到最右向下走
//                    if (randomDerection <= 2)
//                    {
//                        if (i + 1 < rows)
//                            roomArray[i + 1, j] = 1;
//                        else if (j + 1 < columns)
//                            roomArray[i, j + 1] = 1;
//                    }
//                    //3,4,5向下走，走到最下向右走
//                    else if (randomDerection <= 5 && randomDerection >= 3)
//                    {
//                        if (j + 1 < columns)
//                            roomArray[i, j + 1] = 1;
//                        else if (i + 1 < rows)
//                            roomArray[i + 1, j] = 1;
//                    }
//                    //6,7,8向下和向右走
//                    else if (randomDerection <= 8 && randomDerection >= 6)
//                    {
//                        if (i + 1 < rows)
//                            roomArray[i + 1, j] = 1;
//                        if (j + 1 < columns)
//                            roomArray[i, j + 1] = 1;
//                    }
//                    //9向下、向右和向左走
//                    else
//                    {
//                        if (i + 1 < rows)
//                            roomArray[i + 1, j] = 1;
//                        if (j + 1 < columns)
//                            roomArray[i, j + 1] = 1;
//                        if (i > 0)
//                            roomArray[i - 1, j] = 1;
//                    }
//                }
//            }
//        }
//		//消除拥挤的房间
//        for (int i = 0; i < rows; i++)
//            for (int j = 0; j < columns; j++)
//                if (GetNearRoom(i, j) >= 8) roomArray[i , j] = 0;
//		//标记隐藏房间
//		for (int j = 0; rows > 3 && j < columns; j++)
//		{
//			if (j + 1 < columns)
//			{
//				int hidX = Random.Range (1, rows-1);
//				if (roomArray [hidX, j] == 0 && roomArray [hidX, j + 1] == 1)
//				{
//					hiddenRoomX = hidX;
//					hiddenRoomY = j;
//					Debug.Log ("隐藏房间："+hiddenRoomX+","+hiddenRoomY);
//					break;
//				}
//			}
//		}
//		roomArray [hiddenRoomX, hiddenRoomY] = 1;
//        Notify("MapComplete");
//    }
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
			if(roomArray[1,i]>0&&Random.Range(0,2)>0)
			{
				roomArray [0, i] = 1;
			}
			if(roomArray[rows-2,i]>0&&Random.Range(0,2)>0)
			{
				roomArray [rows-1, i] = 1;
			}
		}
		//扩充矩阵列
		for (int i = 0; i < rows; i++) 
		{
			if(roomArray[i,1]>0&&Random.Range(0,3)>1)
			{
				roomArray [i, 0] = 1;
			}
			if(roomArray[i,columns-2]>0&&Random.Range(0,2)>0)
			{
				roomArray [i, columns-1] = 1;
			}
		}


		//消除拥挤的房间
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < columns; j++)
				if (GetNearRoom(i, j) >= 8) roomArray[i , j] = 0;
		//标记隐藏房间
		for (int j = 0; rows > 3 && j < columns; j++)
		{
			if (j + 1 < columns)
			{
				int hidX = Random.Range (1, rows-1);
				if (roomArray [hidX, j] == 0 && roomArray [hidX, j + 1] == 1)
				{
					hiddenRoomX = hidX;
					hiddenRoomY = j;
					Debug.Log ("隐藏房间："+hiddenRoomX+","+hiddenRoomY);
					break;
				}
			}
		}
		roomArray [hiddenRoomX, hiddenRoomY] = 1;

		Notify("MapComplete");
	}

	//初始化预设房间3×3
	void InitalPresetRoom(int i)
	{
		Debug.Log ("预设房间分布"+i);
		if (i == 1) 
		{
			roomArray = new int[,] {
				{1, 1, 0},
				{1, 1, 0},
				{1, 1, 1}
			};
			hiddenRoomX = 1;
			hiddenRoomY = 0;
		} 
		else if (i == 2)
		{
			roomArray = new int[,] {
				{1, 1, 0},
				{1, 1, 0},
				{0, 1, 1}
			};
			hiddenRoomX = 0;
			hiddenRoomY = 1;
		} 
		else if (i == 3) 
		{
			roomArray = new int[,] {
				{1, 1, 1},
				{1, 1, 0},
				{1, 1, 1}
			};
			hiddenRoomX = 1;
			hiddenRoomY = 0;
		} 
		else 
		{
			roomArray = new int[,] {
				{1, 1, 0},
				{0, 1, 0},
				{1, 1, 1}
			};
			hiddenRoomX = 2;
			hiddenRoomY = 0;
		}
	}

    //检索roomArray[x,y]邻近房间
    int GetNearRoom(int x, int y)
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


    //检索roomArray[x,y]周围房间,用于生成门
    int GetSurroundRoom(int x, int y)
    {
        for (int i = 0; i < 4;i++ )
            surroundRoom[i] = 0;
        int count=0;
        //检查右侧
        if (y+1<columns)
        {
            if (roomArray[x, y + 1] >= 1)
            {
                surroundRoom[3] = 1;
                count++;
            }
        }
        //检查左侧
        if (y - 1 >= 0)
        {
            if (roomArray[x, y - 1] >= 1)
            {
                surroundRoom[2] = 1;
                count++;
            }
        }
        //检查上侧
        if (x - 1 >= 0)
        {
            if (roomArray[x - 1, y] >= 1)
            {
                surroundRoom[0] = 1;
                count++;
            }
        }
        //检查下侧
        if (x + 1 < rows)
        {
            if (roomArray[x + 1, y] >= 1)
            {
                surroundRoom[1] = 1;
                count++;
            }
        }
        return count;
    }
    //设置关卡大小
    public void SetRowColumn(int x, int y)
    {
        rows = x;
        columns = y;
    }

    //设置关卡
    public void SetupCheckpoint()
    {
        int k = 0;
        roomList.Clear();
        CheckpointNumber++;
		if (rows < 6 && columns < 6) 
		{
			rows++;
			columns++;
		}
		roomArray = new int[rows, columns];
		//Debug.Log ("roomarray[0,0]:"+roomArray[0,0]+",,,"+roomArray[1,1]);
		//初始化房间布局
		if(preset==false||CheckpointNumber>1) InitalRoomLayout();
		//使用预设的房间布局
		else InitalPresetRoom(Random.Range(0,5));
			//InitalPresetRoom(1);
			//InitalPresetRoom(Random.Range(0,5));

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                
                if (roomArray[i,j]>0)
                {
					int type;
                    int surroundRoomNumber = GetSurroundRoom(i, j);
					//房间类型号，-3隐藏房间，-2BOSS，-1起始，0无，1宝箱，2商店，3祭坛
					if (isTest == false)
						type = Random.Range (1, 15);
					else
						type =(int) testRoomType;
					//int type = 2;
					//设置房间大小
					int rmSize;
					if (type == 1 || type == 2)
						rmSize = 1;
					else if (type == 3)
						rmSize = 3;
					else
						rmSize = Random.Range(2,4);
					//添加房间类
					roomList.Add(new Room(type, i, j, surroundRoom, 0, rmSize));
                    k++;
                    roomArray[i, j] = type;
                }
            }
        }

        //设置起始位置，类型-1
        int startNum = Random.Range(4, roomList.Count - 4);
		while (roomList [startNum].roomX == hiddenRoomX && roomList [startNum].roomY == hiddenRoomY) 
		{
			startNum = Random.Range(4, roomList.Count - 4);
		}
        roomList[startNum].type = -1;
        roomArray[roomList[startNum].roomX, roomList[startNum].roomY] = -1;
        //设置BOSS位置，类型-2
        int bossNum;
		if (startNum > roomList.Count - startNum) {
			bossNum = Random.Range (0, startNum / 3);
			//while (roomList [bossNum].roomX == hiddenRoomX && roomList [bossNum].roomY == hiddenRoomY) 
			//{
			//	bossNum = Random.Range (0, startNum / 3);
			//}
		} 
		else 
		{
			bossNum = Random.Range (startNum + (roomList.Count - startNum) / 3 * 2, roomList.Count);
			//while (roomList [bossNum].roomX == hiddenRoomX && roomList [bossNum].roomY == hiddenRoomY) 
			//{
			//	bossNum = Random.Range (startNum + (roomList.Count - startNum) / 3 * 2, roomList.Count);
			//}
		}
        roomList[bossNum].type = -2;
        roomArray[roomList[bossNum].roomX, roomList[bossNum].roomY] = -2;
		//设置隐藏房间类型
        //TEMP
		if (hiddenRoomX > 0||(CheckpointNumber==1&&preset))
		{
			GetNextRoom (hiddenRoomX, hiddenRoomY).type = -3;
			roomArray [hiddenRoomX, hiddenRoomY] = -3;
		}

        //输出房间布局
        string str = "";
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                str += roomArray[i, j].ToString()+" ";
            }
            str += "\n";
        }
		//Debug.Log ("ROW:" + rows + "COL:" + columns);
        Debug.Log(str);
    }


    //载入关卡
	public void LoadCheckpoint(int[] r, int[] isPass, int[] rms)
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

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {            
                if (roomArray[i,j]!=0)
                {
                    int surroundRoomNumber = GetSurroundRoom(i, j);
                    //int type = Random.Range(1, 6);
					roomList.Add(new Room
						(r[i * columns + j], i, j, surroundRoom, isPass[i * columns + j],rms[i * columns + j]));
					
                }

            }
        }
    
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
}
