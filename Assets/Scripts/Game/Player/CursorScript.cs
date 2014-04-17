using UnityEngine;
using System.Collections;

public class CursorScript : MonoBehaviour {

	//private Vector3 previousMousePosition;
	public PlayerScript player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (InputHandler.DirectionVector != Vector3.zero)
			this.transform.position = player.transform.position + (InputHandler.DirectionVector * player.Radius);
	}
}
