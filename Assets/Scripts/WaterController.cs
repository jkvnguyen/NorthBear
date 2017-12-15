﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

	public float height;
	public float time;
	// Use this for initialization
	void Start () {
		iTween.MoveBy(this.gameObject, iTween.Hash( "y", height, "time", time,"loopType","pingpong","easetype",iTween.EaseType.easeInOutSine));

			}
	
	// Update is called once per frame
	void Update () {
		
	}
}