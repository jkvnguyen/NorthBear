using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour {

	public float waterLevel;
	public float floatHeight;
	public float bounceDamp;
	public Vector3 offset;
	public float rotation;

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 upLift;
	private Rigidbody rig;
	private Transform t;

	// Use this for initialization
	void Start () {
		rig = GetComponent <Rigidbody> ();
		t = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		actionPoint = transform.position + transform.InverseTransformDirection (offset);
		forceFactor = ((actionPoint.y - waterLevel) / floatHeight);	

		if (forceFactor > 0f) {
			upLift = -Physics.gravity * (forceFactor - rig.velocity.y * bounceDamp);
			rig.AddForceAtPosition(upLift, actionPoint);
		}
			
		t.rotation = Quaternion.Euler(rotation * Mathf.Sin(Time.time * 1.0f) + (-90), 0f, 0f);
	}
}
