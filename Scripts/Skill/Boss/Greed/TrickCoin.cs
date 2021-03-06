﻿using UnityEngine;
using System.Collections;

public class TrickCoin : MonoBehaviour {

    public GameObject[] gnomes;
	// Use this for initialization
	void Start () {
        StartCoroutine(IEnumTrick());
	}

    IEnumerator IEnumTrick()
    {

        float waitTime = Random.value + 1f;
        yield return new WaitForSeconds(waitTime);

        GameObject obj=Instantiate(gnomes[Random.Range(0, 4)],transform.position,Quaternion.identity) as GameObject;
        obj.GetComponent<RoomElement>().Master = GetComponent<RoomElement>().Master;
        GetComponent<RoomElement>().Destroy();

    }
}
