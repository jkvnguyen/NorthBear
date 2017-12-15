using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private Rigidbody rBody;
    public float fMult = 1;
    private float yVel;

    void Start ()
    {
        rBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        
    }

	public void Move (string direction)
	{
        yVel = rBody.velocity.y;

        //rBody.AddForce(Vector3.forward * fMult);
        if (direction == "Forward")
            rBody.velocity =  new Vector3(0, 0, 1) * fMult;
        else if (direction == "Backward")
            rBody.velocity = new Vector3(0, 0, -1) * fMult;
        else if (direction == "Right")
            rBody.velocity = new Vector3(1, 0, 0) * fMult;
        else if (direction == "Left")
            rBody.velocity = new Vector3(-1, 0, 0) * fMult;
        else if (direction == "Forward and Right")
            rBody.velocity = new Vector3(1, 0, 1) * fMult;
        else if (direction == "Forward and Left")
            rBody.velocity = new Vector3(-1, 0, 1) * fMult;
        else if (direction == "Backward and Right")
            rBody.velocity = new Vector3(1, 0, -1) * fMult;
        else if (direction == "Backward and Left")
            rBody.velocity = new Vector3(-1, 0, -1) * fMult;

        rBody.velocity = new Vector3(rBody.velocity.x, yVel, rBody.velocity.z);
	}

}
