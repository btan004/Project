using UnityEngine;
using System.Collections;

public class MeleeAttackBoxScript : MonoBehaviour {

	public CursorScript cursor;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = cursor.transform.position;

		Vector3 direction = (cursor.player.transform.position - cursor.transform.position);
		Quaternion rotation = Quaternion.LookRotation (direction);
		this.transform.rotation = rotation;

		this.renderer.enabled = cursor.player.IsAttacking;
	}
}

/*
void Update()
{    private Quaternion _lookRotation;
	private Vector3 _direction;
	//find the vector pointing from our position to the target
	_direction = (Target.position - transform.position).normalized;
	
	//create the rotation we need to be in to look at the target
	_lookRotation = Quaternion.LookRotation(_direction);
	
	//rotate us over time according to speed until we are in the required rotation
	transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
}
*/