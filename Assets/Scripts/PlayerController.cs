using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{

    //public float speed;
    //public Text countText;
    //public Text winText;

    //private Rigidbody rb;
    public AudioClip pickup;
    AudioSource audio;

    void Start ()
	{
        //rb = GetComponent<Rigidbody>();
        //setCountText ();
        //winText.text = "";
        audio = GetComponent<AudioSource>();
    }

	void FixedUpdate ()
	{

	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up")) 
		{
            // HandController.pickUp = true;
            // HandController.interacting = other.gameObject;
            audio.PlayOneShot(pickup, 0.7F);
            other.gameObject.SetActive (false);
            FishTracker.CollectFish();
			//setCountText ();
		}
	}
    /*
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            HandController.pickUp = false;
            HandController.interacting = null;

        }
    }
    */



}
