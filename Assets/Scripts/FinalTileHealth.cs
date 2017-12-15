using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalTileHealth : MonoBehaviour {

    float startHealth = 1000;
    float tileHP;
    float weatherFact = 1;
    float decayRate;
    public AudioClip crack;
    private bool canCrack1 = true;
    private bool canCrack2 = true;
    private bool canCrack3 = true;
    AudioSource audio;
   // public string Location;   // This is the next scene 

    float currentTime;
    //Material surface;
    public Material decayStage1;
    public Material decayStage2;
    //int permitDecay = 0;
    
    //I like tags but i cant remeber how to get them...
    // probably horrible
    // do we need to make the tile change appearance as it 
    // is it a material?
    // so there's 3 in total?
    //how fast do we want these decaying
    // how do we load a new material through a script?
    

	void Start ()
    {
        tileHP = startHealth;
        audio = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        decayRate = 1 * weatherFact;
        if(tileHP <= 0)
        {
            if (canCrack3)
            {
                audio.PlayOneShot(crack, 0.7F);
                canCrack3 = false;
            }
            this.gameObject.SetActive(false);
        }
        if(tileHP < 700)
        {
            if(canCrack1)
            {
                audio.PlayOneShot(crack, 0.7F);
                canCrack1 = false;
            }
            this.gameObject.GetComponentInChildren<Renderer>().material = decayStage1;

           


        }
        if (tileHP < 350)
        {
            if (canCrack2)
            {
                audio.PlayOneShot(crack, 0.7F);
                canCrack2 = false;
            }
            this.gameObject.GetComponentInChildren<Renderer>().material = decayStage2;
        }
	}
    
    
    public void Decay()
    {

        if (tileHP != 0)
        {

            Debug.Log("decay");
            tileHP = tileHP - decayRate;
            /*
            currentTime = Time.time;
            if((int) currentTime % 1 == 0)
            { 
                   
                
            }
            */
            
        }


    }
    
}
