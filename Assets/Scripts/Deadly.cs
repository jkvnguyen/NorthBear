using UnityEngine;
using System.Collections;

public class Deadly : MonoBehaviour {
    public float damage = 35f;
    private int counter = 0;
    private bool damDelt = false;
    //Script defines how damage is dealt
    private GameObject player;



    void FixedUpdate()
    {
        // Control for how often damage can be dealt
        counter++;
        if (counter % 100 == 0)
        {
            damDelt = false;
        }

    }
    // Use this for initialization
    void Start () {
	    player = GameObject.FindGameObjectWithTag("Body");
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Body")
        {
            //if dealing damage is approved
            if (damDelt == false)
            {
                //Deal Damage, reset counter and damage approval
                player.GetComponent<HealthScript>().TakeDamage(damage);
                damDelt = true;
                counter = 1;
                //Debug.Log("Hit");
            }
        }
    }
}
