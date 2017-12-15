using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishTracker : MonoBehaviour {

    static int fishCount = 0;
    Object tracker;
    string sceneName;
    private GameObject end;

    // Use this for initialization
    private void Awake()
    {
        tracker = this;
        Object.DontDestroyOnLoad(tracker);
        

    }
    void Start ()
    {
        end = GameObject.FindGameObjectWithTag("Finish");
        end.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {

        //KinectDataClient.NetworkTransport.Recive(clientHostId, clientConnId, clientChannelId, compressBuffer, snowlevel, out error);


        sceneName = SceneManager.GetActiveScene().name;

        if (end == null)
        {
            end = GameObject.FindGameObjectWithTag("Finish");
        }
        if (sceneName == "Level_1")
        {
            if(fishCount >= 1)
            {
                end.SetActive(true);
            }
        }
        else
        {
            end.SetActive(true);
        }

	}

    public static void CollectFish()
    {
        fishCount++;
        Debug.Log(fishCount);
    }

    public static int GetCount()
    {
        return fishCount;
    }
}
