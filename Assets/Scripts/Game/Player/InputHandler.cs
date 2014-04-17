using UnityEngine;
using System.Collections;

public class InputHandler {

	//Input
	public static Vector3 MovementVector;
	public static Vector3 DirectionVector;
	public static bool		WantToSprint;
	public static bool		WantToAttack;
	public static bool		WantToAura;
	public static bool		WantToSkillShot;
	public static bool		WantToQuit;

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
		/*
		if (Input.GetAxis("Horizontal Movement") < -.5		|| Input.GetAxis("Horizontal Movement KB") < 0)
			MovementVector += Vector3.left;
		if (Input.GetAxis("Horizontal Movement") > .5		|| Input.GetAxis("Horizontal Movement KB") > 0)
			MovementVector += Vector3.right;
		if (Input.GetAxis("Vertical Movement") < -.5		|| Input.GetAxis("Vertical Movement KB") > 0)
			MovementVector += Vector3.forward;
		if (Input.GetAxis("Vertical Movement") > .5			|| Input.GetAxis("Vertical Movement KB") < 0)
			MovementVector += Vector3.back;
			*/
		if (Input.GetAxis("Horizontal Movement") < -.5 || Input.GetAxis("Horizontal Movement") > .5)
			MovementVector.x = Input.GetAxis("Horizontal Movement");
		if (Input.GetAxis("Vertical Movement") < -.5 || Input.GetAxis("Vertical Movement") > .5)
			MovementVector.z = -1 * Input.GetAxis ("Vertical Movement");
		MovementVector.Normalize();
		
	}

	//supports: xbox controller
	private void CheckDirection()
	{
		//check for changes in our direction
		DirectionVector = Vector3.zero;
		/*
		if (Input.GetAxis("Horizontal Direction") < -.5)	DirectionVector.x += Input.GetAxis("Horizontal Direction");
		if (Input.GetAxis("Horizontal Direction") > .5)		DirectionVector.x -= Input.GetAxis("Horizontal Direction");
		if (Input.GetAxis ("Vertical Direction") > -.5)		DirectionVector.y += Input.GetAxis ("Vertical Direction");
		if (Input.GetAxis ("Vertical Direction") < .5) 		DirectionVector.y -= Input.GetAxis ("Vertical Direction");
		*/
		if (Mathf.Abs(Input.GetAxis("Horizontal Direction")) > .3)
			DirectionVector.x = Input.GetAxis ("Horizontal Direction");
		if (Mathf.Abs(Input.GetAxis("Vertical Direction")) > .3)
			DirectionVector.z = -1 * Input.GetAxis ("Vertical Direction");
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
