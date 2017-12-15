using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

    private Animator anim_;
    private ClaustrophobiaGM gm_;
    bool hasBeenTouched = false;
    private AudioSource as_;
    public bool rightButton = false;

    // Use this for initialization
    void Start () {
        anim_ = GetComponent<Animator>();
        gm_ = GameObject.FindWithTag("GameController").GetComponent<ClaustrophobiaGM>();
        as_ = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")

            {
            Debug.Log("Player touched button");
            if (!hasBeenTouched)
            {
                hasBeenTouched = true;
                    anim_.SetTrigger("push");
                    as_.Play();
                    gm_.pushedButton();
                }
            
        }
    }
}
