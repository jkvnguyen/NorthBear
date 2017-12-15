using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour {

	GameObject player;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	void Awake () {
		Object.DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
        if (SceneManager.GetActiveScene().name == "EndScenceGood2" || SceneManager.GetActiveScene().name == "EndSceneBad")
        {
            this.GetComponent<AudioSource>().enabled = false;
        }
        else
        {
            if(this.GetComponent<AudioSource>().enabled == false)
            {
                this.GetComponent<AudioSource>().enabled = true;
            }
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        this.transform.position = player.transform.position;
    }
}
