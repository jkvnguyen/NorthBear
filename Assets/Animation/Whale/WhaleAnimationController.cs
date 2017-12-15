using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleAnimationController : MonoBehaviour {

	public Animator anim;
	public float speed;
	private Transform t;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		t = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		t.Translate (speed * Time.deltaTime, 0, 0);
	}
}
