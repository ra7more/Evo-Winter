﻿using UnityEngine;
using System.Collections;

public class Skull : RoomElement {

	// Use this for initialization
    public override void Awake()
    {
        base.Awake();
        RoomElementID = 8;
        this.tag = "DynamicGroundElement";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
