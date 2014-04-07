using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	//The target for our camera to follow and watch
	public GameObject target;

	//The offset of our camera to the target
	private Vector3 offset;
	
	// Use this for initialization
	void Start () {
		offset = transform.position - target.transform.position;
	}
	
	// Late Update is called after Update
	void LateUpdate () {
		//get our new position
		transform.position = (target.transform.position + offset);

		//make the camera look at the target
		transform.LookAt(target.transform);
	}
}
