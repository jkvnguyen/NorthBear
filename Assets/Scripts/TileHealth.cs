using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHealth : MonoBehaviour
{

    float startHealth = 500;
    public float tileHP;
    float weatherFact = 1;
    float decayRate;
    public AudioClip crack;
    public AudioClip crackAgain;
    public AudioClip crackFinal;
    private bool canCrack1 = true;
    private bool canCrack2 = true;
    private bool canCrack3 = true;
    AudioSource audio;

    float currentTime;
    //Material surface;
    public Material decayStage1;
    public Material decayStage2;
    //int permitDecay = 0;




    void Start()
    {
        tileHP = startHealth;
        Debug.Log("started");
        audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        decayRate = 1 * weatherFact;
        if (tileHP <= 0)
        {
            if (canCrack3)
            {
                audio.PlayOneShot(crackFinal, 0.7F);
                canCrack3 = false;
            }
            this.gameObject.SetActive(false);
        }
        if (tileHP < 350)
        {
            if (canCrack1)
            {
                audio.PlayOneShot(crack, 0.7F);
                canCrack1 = false;
            }
            this.gameObject.GetComponentInChildren<Renderer>().material = decayStage1;

            /*if (!source.isPlaying)
            {
                SceneManager.LoadScene(Location);
            }*/

        }
        if (tileHP < 175)
        {
            if (canCrack2)
            {
                audio.PlayOneShot(crackAgain, 0.7F);
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