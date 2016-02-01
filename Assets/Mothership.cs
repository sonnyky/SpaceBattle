using UnityEngine;
using System.Collections;

public class Mothership : MonoBehaviour {

	public Object sphere;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createSphere() {
		Instantiate (sphere, new Vector3 (0, -2, 0), Quaternion.identity);
	}
}
