using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadow : MonoBehaviour {

    public Transform player;
    private Transform myTransform;
	// Use this for initialization
	void Start ()
    {
        myTransform = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        myTransform.position = player.position - new Vector3(0, 0, 0);

	}
}
