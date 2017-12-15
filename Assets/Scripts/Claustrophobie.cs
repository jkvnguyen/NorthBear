using UnityEngine;
using System.Collections;

public class Claustrophobie : MonoBehaviour {
    private float difficulty = Data.difficulty;
    private float mc = 0f;
    public bool backWall = false;

	// Use this for initialization
	void Start () {
        if (difficulty == 1) {
            mc = 0.2f;
        } else if (difficulty == 2)
        {
            mc = 0.7f;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void moveTowards()
    {
        Vector3 vec = transform.position + transform.right;
        transform.position = Vector3.MoveTowards(transform.position,vec,0.2f +mc);

        if (backWall == true)
        {
            GetComponent<GetTogether>().getClose();
        }
    }
}
