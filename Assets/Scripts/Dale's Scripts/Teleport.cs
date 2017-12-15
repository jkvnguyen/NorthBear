using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

    public GameObject Player; // This is the Player gameObject 
    public string Location;   // This is the next scene 

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.Equals(Player))
        {
            Application.LoadLevel(Location);
        }

    }
}
