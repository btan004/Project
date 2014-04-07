using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//player stats
	public float velocity = 1f;

	public int Score = 0;
	public int Lives = 3;
	public float Health = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Move our Player
		MovePlayer();

		//Keep the Player within the map bounds
		KeepPlayerInMap();

		//Check if the player wants to end the game
		if (Input.GetKey("escape"))
		{
			Application.LoadLevel("StartMenuScene");
		}
	}

	private void MovePlayer()
	{
		//Get a Movement Vector from user input
		Vector3 movementVector = Vector3.zero;
		if (Input.GetKey("a")) movementVector += Vector3.left;
		if (Input.GetKey("d")) movementVector += Vector3.right;
		if (Input.GetKey("w")) movementVector += Vector3.forward;
		if (Input.GetKey("s")) movementVector += Vector3.back;

		//apply it to the player
		this.transform.position = (this.transform.position + (movementVector * velocity * Time.deltaTime));
	}

	private void KeepPlayerInMap()
	{
		if (transform.position.x < -50) SetPlayerPositionX(-50);
		if (transform.position.x > 50) SetPlayerPositionX(50);
		if (transform.position.z < -50) SetPlayerPositionZ(-50);
		if (transform.position.z > 50) SetPlayerPositionZ(50);
	}

	private void SetPlayerPositionX(float position)
	{
		Vector3 temp = transform.position;
		temp.x = position;
		transform.position = temp;
	}

	private void SetPlayerPositionY(float position)
	{
		Vector3 temp = transform.position;
		temp.y = position;
		transform.position = temp;
	}

	private void SetPlayerPositionZ(float position)
	{
		Vector3 temp = transform.position;
		temp.z = position;
		transform.position = temp;
	}
	
}
