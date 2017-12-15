using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowBounds : MonoBehaviour {

    public RawImage white;
    
    float boundsFade = 0;
    bool fade = false;
	// Use this for initialization
	void Start ()
    {
        white.color = new Color(white.color.r, white.color.g, white.color.b, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Color C = white.color;
        
        if (fade)
        {
            if(boundsFade == 0)
            {
                boundsFade = boundsFade + 50;
            }
            boundsFade++;
            C.a = boundsFade / 256;
            white.color = C;
        }
        else
        {
            if (boundsFade >= 255)
            {
                boundsFade = boundsFade - 50;
            }
            boundsFade--;
            C.a = boundsFade / 256;
            white.color = C;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In bounds");
        fade = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("In bounds");
        fade = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Out of bounds");
        fade = true;
    }
}
