using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Door")
        {
            col.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
