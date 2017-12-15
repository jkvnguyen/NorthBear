using UnityEngine;
using System.Collections;

public class Claustrophobie_aktivator : MonoBehaviour {
    GameObject Target = null;
    RaycastHit hit;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        Hit_Target(ray);
    }

    public void Hit_Target(Ray ray)
    {
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("TagName: " + hit.transform.tag);
            Debug.DrawLine(transform.position, hit.point, Color.red);
            if (hit.transform.GetComponent<Claustrophobie>() != null) // If hit object has needed script (avoid highliting other objects)
            {
                if (Target != hit.collider.gameObject) // if I`m not already looking at it
                {
                    Target = hit.collider.gameObject; // make object my target
                    if (Target.GetComponent<Claustrophobie>() != null)
                    {
                        Target.GetComponent<Claustrophobie>().moveTowards(); // Reverse the old one back in here        
                    }            
                }
            }
        }
    }

}
