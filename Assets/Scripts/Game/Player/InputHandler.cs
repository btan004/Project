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
	public static bool		WantToSpendSkillPoint;
	public static bool		WantToChangeSkillLeft;
	public static bool		WantToChangeSkillRight;
	public const double		AnalogTolerance = 0.5;

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

		CheckSpendSkillPoint();
		CheckSkillChange();
	}

	//Supports: xbox controller and keyboard
	private void CheckMovement()
	{		
		//Debug.Log("Movement Axis: (" + Input.GetAxis("Horizontal Movement") + ", " + Input.GetAxis("Vertical Movement") + ")");
		MovementVector = new Vector3(Input.GetAxis("Horizontal Movement"), 0, Input.GetAxis("Vertical Movement"));
		if (MovementVector.magnitude < AnalogTolerance) MovementVector = Vector3.zero;
		MovementVector.Normalize();
	}

	//supports: xbox controller
	private void CheckDirection()
	{
		//check for changes in our direction
		DirectionVector = Vector3.zero;
		if (Mathf.Abs(Input.GetAxis("Horizontal Direction")) > AnalogTolerance)
			DirectionVector.x = Input.GetAxis ("Horizontal Direction");
		if (Mathf.Abs(Input.GetAxis("Vertical Direction")) > AnalogTolerance)
			DirectionVector.z = -1 * Input.GetAxis ("Vertical Direction");
		DirectionVector.Normalize();
	}

	//supports xbox controller and keyboard
	private void CheckSprint()
	{
		if ((Input.GetAxis("Sprint") > 0.5f))
			WantToSprint = true;
		else
			WantToSprint = false;
	}

	//supports: xbox controller
	private void CheckMeleeAttack()
	{
		//WantToAttack = (Input.GetAxis("Attack") < -0.5f); 
		WantToAttack = Input.GetButton("A Button");
	}

	//supports: xbox controller
	private void CheckAura()
	{
		WantToAura = Input.GetButton("X Button");
	}

	//supports: xbox controller
	private void CheckSkillShot()
	{
		WantToSkillShot = Input.GetButton("B Button");
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
	
	private void CheckSpendSkillPoint()
	{
		//WantToSpendSkillPoint = Input.GetButton("SpendSkillPoint");
		WantToSpendSkillPoint = Input.GetButton("Y Button");
	}

	private void CheckSkillChange()
	{
		WantToChangeSkillLeft = false;
		WantToChangeSkillRight = false;

		if (Input.GetAxis("SkillSelect") < -.5)
			WantToChangeSkillLeft = true;
		if (Input.GetAxis("SkillSelect") > .5)
			WantToChangeSkillRight = true;

	}

}
