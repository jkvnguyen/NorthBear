using System.Collections;
using UnityEngine;

public class CameraControllerJump : MonoBehaviour


{
    //private Rigidbody rBody;
    private Transform myTransform;
    public float fMult = 1;
    private float yVel;

    void Start()
    {
       //rBody = this.GetComponent<Rigidbody>();
       myTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {

    }

    public void Move(string direction)
    {
        Vector3 position = myTransform.position;

        //rBody.AddForce(Vector3.forward * fMult);
        if (direction == "Forward")
            position.z = position.z + 0.57f;
        //new Vector3(0, 0, 1) * fMult;
        else if (direction == "Backward")
            position.z = position.z - 0.57f;
        //rBody.velocity = new Vector3(0, 0, -1) * fMult;
        else if (direction == "Right")
            position.x = position.x + 0.57f;
        //rBody.velocity = new Vector3(1, 0, 0) * fMult;
        else if (direction == "Left")
            position.x = position.x - 0.57f;
        //rBody.velocity = new Vector3(-1, 0, 0) * fMult;
        else if (direction == "Forward and Right")
        {
            position.z = position.z + 0.57f;
            position.x = position.x + 0.57f;
        }
        //rBody.velocity = new Vector3(1, 0, 1) * fMult;
        else if (direction == "Forward and Left")
        {
            position.z = position.z + 0.57f;
            position.x = position.x - 0.57f;
        }
        //rBody.velocity = new Vector3(-1, 0, 1) * fMult;
        else if (direction == "Backward and Right")
        {
            position.z = position.z - 0.57f;
            position.x = position.x + 0.57f;
        }
        //rBody.velocity = new Vector3(1, 0, -1) * fMult;
        else if (direction == "Backward and Left")
        {
            position.z = position.z - 0.57f;
            position.x = position.x - 0.57f;
        }
        // rBody.velocity = new Vector3(-1, 0, -1) * fMult;

        //rBody.velocity = new Vector3(rBody.velocity.x, yVel, rBody.velocity.z);
        myTransform.position = position;
    }

}