using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {
    // Use this for initialization
    private bool hasBeenTouched = false;
    public bool rightTorch = false; 
    private Animator anim_;
    private ClaustrophobiaGM gm_;
    private AudioSource as_;

	void Start () {
        anim_ = GetComponent<Animator>();
        if (GameObject.FindWithTag("GameController") != null)
        {
            gm_ = GameObject.FindWithTag("GameController").GetComponent<ClaustrophobiaGM>();
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }
        as_ = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Torche pulled");
        if (!hasBeenTouched) { 
            if (collision.tag == "Player")
            {
                hasBeenTouched = true;
                anim_.SetTrigger("pull");
                as_.Play();
                gm_.pulledTorch(rightTorch);
            }
        }
    }
}
