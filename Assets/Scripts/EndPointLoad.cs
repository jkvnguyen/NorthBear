using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndPointLoad : MonoBehaviour {

    public string Location;
    Scene lvl2;

    // Use this for initialization
    void Start () {
        lvl2 = SceneManager.GetSceneByName("Level_2");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TriggerLoad()
    {
        SceneManager.LoadScene("Level_2");
    }
}
