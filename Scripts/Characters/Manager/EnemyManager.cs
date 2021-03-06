﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// EnemyManager
/// Brief: For Management of all enemies
/// Author: IfYan
/// Latest Update Time: 2017.5.11
public class EnemyManager : ExUnitySingleton<EnemyManager>{

    List<Character> enemyList = new List<Character>();

    public List<Character> EnemyList
    {
        get { return enemyList; }
        set { enemyList = value; }
    }

   public void ClearAll()
    {
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            enemyList[i].Destroy();   
        }
    }
}
