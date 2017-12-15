using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimedTeleport : MonoBehaviour {

    
    public string Location;
    public float wait;


    void Update() {
        wait -= Time.deltaTime;

        if(wait < 0f) {
            SceneManager.LoadScene(Location);
        }
    }
}
