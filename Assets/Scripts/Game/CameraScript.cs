using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	//The target for our camera to follow and watch
	public GameObject target;

	//The offset of our camera to the target
	private Vector3 offset;

	//camera boundaries
	private float cameraLeftMax = -39;
	private float cameraRightMax = 39;
	private float cameraBackMax = -53.5f;
	private float cameraFrontMax = 100;
	
	// Use this for initialization
	void Start () {
		//align the camera to the players x position
		transform.SetPositionX(target.transform.position.x);

		//get our camera offset from the player
		offset = transform.position - target.transform.position;

		//get our camera angle set to look at the player
		transform.LookAt(target.transform);
	}
	
	// Late Update is called after Update
	void LateUpdate () {
		//get our new position
		transform.position = (target.transform.position + offset);

		//make sure the camera stays in the map
		transform.BindToArea(cameraLeftMax, cameraRightMax, cameraBackMax, cameraFrontMax);
	}

}
