using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherControl2 : MonoBehaviour {

 public int snowvalue = 0;

    // Use this for initialization
    void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {

        int stack1 = 0;
        int stack2 = 0;
        int stack3 = 0;

        float cube2X = GameObject.Find("Cube.002").GetComponent<Transform>().position.x;
        float cube2Z = GameObject.Find("Cube.002").GetComponent<Transform>().position.z;

        float cube9X = GameObject.Find("Cube.009").GetComponent<Transform>().position.x;
        float cube9Z = GameObject.Find("Cube.009").GetComponent<Transform>().position.z;

        float islandX = GameObject.Find("Island").GetComponent<Transform>().position.x;
        float islandZ = GameObject.Find("Island").GetComponent<Transform>().position.z;

        if (Mathf.Abs(islandX - cube9X) <= 1 && Mathf.Abs(islandZ - cube9Z) <= 1)
        {
            stack1 = 1;
        }
        else stack1 = 0;
        if (Mathf.Abs(islandX - cube2X) <= 1 && Mathf.Abs(islandZ - cube2Z) <= 1)
        {
            stack2 = 1;
        }
        else stack2 = 0;

        if (Mathf.Abs(cube2X - cube9X) <= 1 && Mathf.Abs(cube2Z - cube9Z) <= 1)
        {
            stack3 = 1;
        }
        else stack3 = 0;

        snowvalue = stack1 + stack2 + stack3;

        Debug.Log(snowvalue);


    }
}
