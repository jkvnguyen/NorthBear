using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
	protected new Rigidbody rigidbody;
	public bool currentlyInteracting;
	private HandController attachedHand;
	private Transform interactionPoint;
    public float velocityFactor;
    public float rotationFactor;
    //private float velocityFactor = 20000f;
    //private float rotationFactor = 50f;
    private Vector3 posDelta;  // posDelta = (hand_pos - obj_pos)
	private Quaternion rotationDelta;
	private float angle;
	private Vector3 axis;

	protected void Start ()
	{
		rigidbody = GetComponent<Rigidbody>();
		interactionPoint = new GameObject().transform;
		velocityFactor /= rigidbody.mass;
	}
		
	protected void Update ()
	{
		if (attachedHand && currentlyInteracting)
		{
           // Debug.Log(attachedHand);
			posDelta = attachedHand.transform.position - interactionPoint.position;
			this.rigidbody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

			rotationDelta = attachedHand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
			rotationDelta.ToAngleAxis(out angle, out axis);

			if (angle > 180)
				angle -= 360;

			this.rigidbody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
		}
	}

	public void BeginInteraction(HandController hand)
	{
		attachedHand = hand;
		interactionPoint.position = hand.transform.position;
		interactionPoint.rotation = hand.transform.rotation;
		interactionPoint.SetParent(transform, true);
		currentlyInteracting = true;
	}

	public void EndInteraction(HandController hand)
	{
		if (hand == attachedHand)
		{
			attachedHand = null;
			currentlyInteracting = false;
		}

	}

	public bool IsInteracting()
	{
		return currentlyInteracting;
	}

}
