using UnityEngine;
using System.Collections;
//Author: Zach Shron


public class FailChain : MonoBehaviour {

    //Script dictates behavior of incorrect chains

	
	void OnTriggerEnter(Collider other)
    {
        //If a hand enters the collision space
        if (other.tag == "Hand")
        {
            //trigger pulling animation and sound and spawn spiders
            this.GetComponentInParent<Animator>().SetTrigger("Pull");
            this.GetComponentInParent<AudioSource>().Play();
            this.GetComponent<SpiderSpawn2>().Spawn();
        }
	}
    // apparently we're paying for thousands of terribly thought out construction projects actually
    // its just not detecting collison
    // this script is the one i used for the last project and its exactly the same
}
