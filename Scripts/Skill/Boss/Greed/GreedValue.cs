﻿using UnityEngine;
using System.Collections;

public class GreedValue : MonoBehaviour {


    int value=0;//10个就满

    public int Value
    {
        get { return this.value; }
        set { this.value = value; }
    }


    public int DmgBuff
    {
        get
        {
            if (value <= 1)
                return 1;
            else if (value <= 3)
                return 2;
            else if (value <= 8)
                return 3;
            else if (value <= 15)
                return 4;
            else if (value <= 24)
                return 5;
            else if (value <= 35)
                return 6;
            else if (value <= 48)
                return 7;
            else if (value <= 63)
                return 8;
            else if (value <= 80)
                return 9;
            else
                return 10;

        }
    }

}
