using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDontDestroy : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Object.DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
