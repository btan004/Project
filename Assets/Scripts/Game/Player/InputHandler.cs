using UnityEngine;
using System.Collections;

public class InputHandler {

	//Input
	public Vector3 MovementVector;
	public Vector3 DirectionVector;
	public bool		WantToSprint;
	public bool		WantToAttack;
	public bool		WantToAura;
	public bool		WantToSkillShot;
	public bool		WantToQuit;

	//Debug variables
	public bool		WantToSpawnEnemy;

	// Use this for initialization
	public InputHandler () {
		MovementVector = Vector3.zero;
		DirectionVector = Vector3.zero;
		WantToSprint = false;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		//Check for user input for the player actions
		CheckMovement();
		CheckDirection();
		CheckSprint();
		CheckMeleeAttack();
		CheckAura();
		CheckSkillShot();
		CheckForSpawningButton();
	}

	//supports: xbox controller and keyboard
	private void CheckMovement()
	{		
		//check for changes in our movement
		MovementVector = Vector3.zero;
		if (Input.GetAxis("Horizontal Movement") < -.5		|| Input.GetAxis("Horizontal Movement KB") < 0)
			MovementVector += Vector3.left;
		if (Input.GetAxis("Horizontal Movement") > .5		|| Input.GetAxis("Horizontal Movement KB") > 0)
			MovementVector += Vector3.right;
		if (Input.GetAxis("Vertical Movement") < -.5			|| Input.GetAxis("Vertical Movement KB") > 0)
			MovementVector += Vector3.forward;
		if (Input.GetAxis("Vertical Movement") > .5			|| Input.GetAxis("Vertical Movement KB") < 0)
			MovementVector += Vector3.back;
		MovementVector.Normalize();
		
	}

	//supports: xbox controller
	private void CheckDirection()
	{
		//check for changes in our direction
		DirectionVector = Vector3.zero;
		if (Input.GetAxis("Horizontal Direction") < -.5)	DirectionVector += Vector3.left;
		if (Input.GetAxis("Horizontal Direction") > .5)		DirectionVector += Vector3.right;
		if (Input.GetAxis("Vertical Direction") > -.5)		DirectionVector += Vector3.forward;
		if (Input.GetAxis("Vertical Direction") < .5)		DirectionVector += Vector3.back;
		DirectionVector.Normalize();
	}

	//supports xbox controller and keyboard
	private void CheckSprint()
	{
		if (Input.GetButton("Sprint"))
			WantToSprint = true;
		else
			WantToSprint = false;
	}

	//supports: xbox controller
	private void CheckMeleeAttack()
	{
		WantToAttack = Input.GetButton("Attack");
	}

	//supports: xbox controller
	private void CheckAura()
	{
		WantToAura = (Input.GetAxis("Aura") > 0.5f);
	}

	//supports: xbox controller
	private void CheckSkillShot()
	{
		WantToSkillShot = (Input.GetAxis("Skill Shot") < -0.5f);
	}

	//supports: keyboard
	private void CheckForQuit()
	{
		WantToQuit = Input.GetKey("escape");
	}

	//On button press, set up bool so it will spawn enemies in SpawnScript.cs
	private void CheckForSpawningButton()
	{
		WantToSpawnEnemy = Input.GetButton("Spawn Debug");
	}
	
}
