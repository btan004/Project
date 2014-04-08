﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//player stats
	public float velocity = 1f;
	private bool IsSprinting = false;
	public float sprintCoefficient = 2.0f;
	public float sprintCooldown = 3;
	private float sprintCooldownTimer = 0;

	public int Score = 0;
	public int Lives = 3;
	public float Health = 100;
	public float Stamina = 10;
	public float TotalStamina = 10;
	public int Level = 1;
	public int Experience;
	public int ExperienceToNextLevel;

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
		movementVector.Normalize ();

		if (Input.GetKey (KeyCode.LeftShift))
			IsSprinting = true;
		else
			IsSprinting = false;

		//apply it to the player
		Vector3 newMovement = movementVector * velocity * Time.deltaTime;
		bool canSprint = Stamina > 0 && sprintCooldownTimer <= 0;
		if (IsSprinting && canSprint) 
		{
			newMovement *= sprintCoefficient;
			Stamina -= Time.deltaTime;

			if (Stamina < 0)
			{
				sprintCooldownTimer = sprintCooldown;
			}
		} 
		else {
			if (Stamina < TotalStamina)
				Stamina += Time.deltaTime;
			if (sprintCooldownTimer > 0)
				sprintCooldownTimer -= Time.deltaTime;
		}

		this.transform.position = (this.transform.position + newMovement);
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
