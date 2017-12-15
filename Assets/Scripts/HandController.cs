using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour
{
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId touchPad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
	private SteamVR_TrackedObject trackedObj;
	private InteractableObject closestObject;
	private InteractableObject interactingObject;
	HashSet<InteractableObject> objectsHoveringOver = new HashSet<InteractableObject>();
    public GameObject player;
    public bool Jump = false;
   
    public CameraController cam;
    public CameraControllerJump cam2;
	public Camera head;
   // static public bool pickUp = false;
   // public static GameObject interacting = null;


	void Start ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
        Data.crouched = Data.UNCROUCHED;
	}


	void Update ()
	{
        
        if (controller == null)
		{
			Debug.Log("Controller not initialized");
			return;
		}
        if (Jump)
        {
            if (controller.GetPressUp(touchPad))
            {
                /*
                Vector2 dir = controller.GetAxis();
                float rotation = -head.GetComponent<Transform>().rotation.eulerAngles.y;
                float sin = Mathf.Sin(rotation * Mathf.Deg2Rad);
                float cos = Mathf.Cos(rotation * Mathf.Deg2Rad);
                float tx = dir.x;
                float ty = dir.y;

                dir.x = (cos * tx) - (sin * ty);
                dir.y = (sin * tx) + (cos * ty);

                if (dir.x < -0.2)
                {
                    if (dir.y < -0.2)
                        cam2.Move("Backward and Left");
                    else if (dir.y > 0.2)
                        cam2.Move("Forward and Left");
                    else
                        cam2.Move("Left");

                }
                else if (dir.x > 0.2)
                {
                    if (dir.y < -0.2)
                        cam2.Move("Backward and Right");
                    else if (dir.y > 0.2)
                        cam2.Move("Forward and Right");
                    else
                        cam2.Move("Right");

                }
                else
                {
                    if (dir.y < -0.2)
                        cam2.Move("Backward");
                    else if (dir.y > 0.2)
                        cam2.Move("Forward");

                }
                */

            }
        }
        else
        {
            if (controller.GetTouch(touchPad))
            {
                /*
                Vector2 dir = controller.GetAxis();
                float rotation = -head.GetComponent<Transform>().rotation.eulerAngles.y;
                float sin = Mathf.Sin(rotation * Mathf.Deg2Rad);
                float cos = Mathf.Cos(rotation * Mathf.Deg2Rad);
                float tx = dir.x;
                float ty = dir.y;

                dir.x = (cos * tx) - (sin * ty);
                dir.y = (sin * tx) + (cos * ty);

                if (dir.x < -0.2)
                {
                    if (dir.y < -0.2)
                        cam.Move("Backward and Left");
                    else if (dir.y > 0.2)
                        cam.Move("Forward and Left");
                    else
                        cam.Move("Left");

                }
                else if (dir.x > 0.2)
                {
                    if (dir.y < -0.2)
                        cam.Move("Backward and Right");
                    else if (dir.y > 0.2)
                        cam.Move("Forward and Right");
                    else
                        cam.Move("Right");

                }
                else
                {
                    if (dir.y < -0.2)
                        cam.Move("Backward");
                    else if (dir.y > 0.2)
                        cam.Move("Forward");

                }
                */

            }
        }

        if (controller.GetPressDown(triggerButton))
		{
            Debug.Log("trigger");
            /*
            if (pickUp)
            {
                interacting.SetActive(false);
                PlayerController.count++;
            }
            */

            float minDistance = float.MaxValue;	
			float distance;

			foreach (InteractableObject item in objectsHoveringOver)
			{
				distance = (item.transform.position - transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closestObject = item;
				}

			}

			interactingObject = closestObject;
			closestObject = null;

			if (interactingObject)
			{
                if (interactingObject.IsInteracting())
                {
                    Debug.Log("Special Dropped");
                    interactingObject.EndInteraction(this);
                }

                Debug.Log("Grabbed");
                interactingObject.BeginInteraction(this);
			}
            


        }
        
		if(controller.GetPressUp(triggerButton) && interactingObject != null)
        {
            Debug.Log("Dropped");
			interactingObject.EndInteraction(this);
        }

        if (controller.GetPressDown(gripButton) && (Data.crouched == Data.UNCROUCHED))
        {
            //player.GetComponent<Transform>().Translate(new Vector3(0, -100, 0) * Time.deltaTime);
            Vector3 temp = player.GetComponent<Transform>().position;

            temp.y = (temp.y - 50) * Time.deltaTime;

            player.GetComponent<Transform>().position =  temp;
            Data.crouched = Data.CROUCHED;
        }

        if (controller.GetPressUp(gripButton) && (Data.crouched == Data.CROUCHED))
        {
            //player.GetComponent<Transform>().Translate(new Vector3(0, 100, 0) * Time.deltaTime);
            Vector3 temp = player.GetComponent<Transform>().position;

            temp.y = (temp.y + 50) * Time.deltaTime;

            player.GetComponent<Transform>().position = temp;
            Data.crouched = Data.UNCROUCHED;
        }
        
    }
    
	private void OnTriggerEnter(Collider collider)
	{
        
        InteractableObject collidedObject = collider.GetComponent<InteractableObject>();
       // Debug.Log(collidedObject);
        if (collidedObject)
			objectsHoveringOver.Add(collidedObject);

		
	}

	private void OnTriggerExit(Collider collider)
	{
		InteractableObject collidedObject = collider.GetComponent<InteractableObject>();

		if (collidedObject)
			objectsHoveringOver.Remove(collidedObject);

	}
    

}
