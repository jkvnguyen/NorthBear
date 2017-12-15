using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
    private ClaustrophobiaGM gm_;

    // Use this for initialization
    void Start () {
        gm_ = GameObject.FindWithTag("GameController").GetComponent<ClaustrophobiaGM>();
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Wall")
        {
            gm_.dropCelling();
        }
    }
}
