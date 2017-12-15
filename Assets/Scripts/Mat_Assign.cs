using UnityEngine;

public class Mat_Assign : MonoBehaviour {
    public int pCube3 { get; private set; }

    // Use this for initialization
    void Start () {
        //GameObject("pCube3");

        // Assigns a material named "Assets/Resources/DEV_Orange" to the object.
        Material newMat = Resources.Load("3", typeof(Material)) as Material;
        //GetComponent<pCube3> = newMat;

        new Material(Shader.Find(" Glossy"));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
