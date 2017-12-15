using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationController : MonoBehaviour {

	public float speed;
	public Animator anim;
	public float timeToTurn;
	public float rotation;

	bool isWalk;
	bool isRotate;

	private Transform t;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		t = GetComponent<Transform> ();
		isWalk = true;

	}

	// Update is called once per frame
	void Update () {
		
		if (isWalk) {
			bearMove ();
		}
		if (isRotate) {
			bearRotate ();
		}
				
		if (SceneManager.GetActiveScene ().name == "EndSceneBad") {
			anim.SetBool ("Mad", true);
		} else {
			anim.SetBool ("Mad", false);
		}

		if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Walk")) {
			isWalk = false;
		} else {
			isWalk = true;
			isRotate = false;
		}



		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Stand")) {
			isRotate = false;
			StartCoroutine (Delay ());

		}






		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Eat")){
			isWalk = false;
		}
	}


	void bearMove(){
		t.Translate (speed * Time.deltaTime, 0, 0);
	}

	void bearRotate(){
		t.Rotate (0, rotation, 0);
	}

	IEnumerator Delay()
	{
		if (timeToTurn > 0) {
			anim.speed = 0;
		}
		yield return new WaitForSeconds(timeToTurn);
		anim.speed = 1;
		isWalk = true;
		isRotate = true;



	}


}
