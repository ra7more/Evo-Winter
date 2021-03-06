﻿using UnityEngine;
using System.Collections;

public class Coin : Item {

    int value=1;
    bool canGet = false;
    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="other">与其碰撞的GameObj</param>
    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && canGet)
        {
            CoinManager.Instance.GetCoin(value);
            ItemManager.Instance.SendMsg("Get_Coin;" + this.gameObject.transform.position.x + ";" + this.gameObject.transform.position.y + ";" + this.gameObject.transform.position.z+";"+CoinManager.Instance.Wealth);
            this.Destroy();
        }

    }

    void Start()
    {
        StartCoroutine(CanBeGot());
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Awake()
    {
        base.Awake();
        roomElementID = 21;
    }

    public override void OnNotify(string msg)
    {
        base.OnNotify(msg);

    }

    IEnumerator CanBeGot()
    {
        yield return new WaitForSeconds(1.5f);
        canGet = true;
    }
}
