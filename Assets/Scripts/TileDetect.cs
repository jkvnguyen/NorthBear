using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TileDetect : MonoBehaviour {

    private Transform myTransform;
    public Transform player;
    public RawImage health;
    private MeshRenderer tileMesh;
    private GameObject lastHit = null;
    private GameObject oldHit = null;
    float deathCount = 0;
    float deathDelay = 0;
    float startTime;
    float currentTime;
    bool isDestructable = false;

    bool canSplash;
    bool canRoar;
    public AudioClip splash;
    public AudioClip roar;
    AudioSource audio;

    // Use this for initialization
    void Start ()
    {
        canSplash = true;
        canRoar = true;
        myTransform = this.GetComponent<Transform>();
        Color C = health.color;
        C.a = 0;
        health.color = C;
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
      
       
        if(deathCount == 500)
        {
            Application.LoadLevel("EndSceneBad");
        }

        myTransform.position = player.position;

        
        int layerMask = 1 << 9;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, Mathf.Infinity, layerMask))
        {


            if (hit.collider.gameObject.GetComponent<TileHealth>().tileHP > 0)
            {
                canSplash = true;
                hit.collider.gameObject.GetComponent<TileHealth>().Decay();
                hit.collider.gameObject.GetComponent<TileHealth>().Decay();
                if (deathCount >= 50)
                {
                    deathCount = deathCount - 50;
                }
                else
                {
                    deathCount = 0;
                    canRoar = true;
                }
                deathDelay = 0;

            }


        }

        layerMask = 1 << 4;

        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, Mathf.Infinity, layerMask))
        {

            deathDelay++;

            if (deathDelay >= 100)
            {
                if(canSplash)
                {
                    audio.PlayOneShot(splash, 0.7f);
                    canSplash = false;
                }
                deathCount++;
                Color C = health.color;
                if(deathCount > 350 && canRoar)
                {
                    audio.PlayOneShot(roar, 0.7f);
                    canRoar = false;
                }
                C.a = deathCount / 500;
                health.color = C;
            }


        }

        
        layerMask = 1 << 10;

        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, Mathf.Infinity, layerMask))
        {

            //SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex+1);
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Level_1")))
            {
                Application.LoadLevel("Level_2");
            }
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Level_2")))
            {
                Application.LoadLevel("Level_3");
            }
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Level_3")))
            {
                Application.LoadLevel("Level_4");
            }
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Level_4")))
            {
                Application.LoadLevel("Level_5");
            }
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Level_5")))
            {
                Application.LoadLevel("Level_6");
            }
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Level_6")))
            {

                if (FishTracker.GetCount() >= 7)
                {
                    Application.LoadLevel("EndSceneGood2");
                }
                if (FishTracker.GetCount() < 7)
                {
                    Application.LoadLevel("EndSceneBad");
                }

            }
            //Application.LoadLevel(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).ToString());



        }






    }


}
