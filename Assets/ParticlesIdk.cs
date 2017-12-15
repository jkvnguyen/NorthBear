using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesIdk : MonoBehaviour {
    Object tracker;
    // Use this for initialization
    void Awake () {
        tracker = this;
        Object.DontDestroyOnLoad(tracker);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
