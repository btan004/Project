using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	//The target for our camera to follow and watch
	public GameObject target;

	//The offset of our camera to the target
	private Vector3 offset;

	//camera boundaries
	private float cameraLeftMax = -41;
	private float cameraRightMax = 41;
	private float cameraBackMax = -53.5f;
	
	// Use this for initialization
	void Start () {
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
		KeepCameraInMap();
	}

	void KeepCameraInMap()
	{
		if (transform.position.x < cameraLeftMax) SetCameraPositionX(cameraLeftMax);
		if (transform.position.x > cameraRightMax) SetCameraPositionX(cameraRightMax);
		if (transform.position.z < cameraBackMax) SetCameraPositionZ(cameraBackMax);
	}

	private void SetCameraPositionX(float position)
	{
		Vector3 temp = transform.position;
		temp.x = position;
		transform.position = temp;
	}
	
	private void SetCameraPositionY(float position)
	{
		Vector3 temp = transform.position;
		temp.y = position;
		transform.position = temp;
	}
	
	private void SetCameraPositionZ(float position)
	{
		Vector3 temp = transform.position;
		temp.z = position;
		transform.position = temp;
	}
}
