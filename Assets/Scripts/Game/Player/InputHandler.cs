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
	public static bool		IsPaused;
	public static float		PauseCooldown = 3.0f;
	private float				pauseTimer;
	private float				realTimePrevious;
	static int temp;

	//Debug variables
	public bool		WantToSpawnEnemy;

	// Use this for initialization
	public InputHandler () {
		MovementVector = Vector3.zero;
		DirectionVector = Vector3.zero;
		WantToSprint = false;
		IsPaused = false;

		pauseTimer = 0.0f;
		temp = 0;
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

		if (Input.GetButton("Y Button")) WaveSystem.ForceSpawnWave = true;

		CheckPause();
	}

	//Supports: xbox controller and keyboard
	private void CheckMovement()
	{
		//Debug.Log("Movement Axis: (" + Input.GetAxis("Horizontal Movement") + ", " + Input.GetAxis("Vertical Movement") + ")");
		MovementVector = new Vector3(Input.GetAxis("Horizontal Movement"), 0, Input.GetAxis("Vertical Movement"));
		//if (MovementVector.magnitude < AnalogTolerance) MovementVector = Vector3.zero;
		//MovementVector.Normalize();
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
		WantToAttack = (Input.GetAxis("Attack") < -0.5f); 
		//WantToAttack = Input.GetButton("A Button");
	}

	//supports: xbox controller
	private void CheckAura()
	{
		WantToAura = Input.GetButton("Aura");
	}

	//supports: xbox controller
	private void CheckSkillShot()
	{
		WantToSkillShot = Input.GetButton("Skill Shot");
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
		WantToSpendSkillPoint = Input.GetButton("SpendSkillPoint");
		//WantToSpendSkillPoint = Input.GetButton("Y Button");
	}

	private void CheckSkillChange()
	{
		WantToChangeSkillLeft = false;
		WantToChangeSkillRight = false;

		//if (Input.GetAxis("SkillSelect") < -.5)
		//	WantToChangeSkillLeft = true;
		//if (Input.GetAxis("SkillSelect") > .5)
		//	WantToChangeSkillRight = true;
		WantToChangeSkillLeft = Input.GetButton("X Button") || (Input.GetAxis("SkillSelect") < -.5);
		WantToChangeSkillRight = Input.GetButton("B Button") || (Input.GetAxis("SkillSelect") > .5);

	}


	private void CheckPause()
	{
		temp++;

		float deltaTime = Time.realtimeSinceStartup - realTimePrevious;


		//Debug.Log("Pause Timer: " + pauseTimer.ToString());
		if (pauseTimer <= 0)
		{
			if (Input.GetButton("Start Button"))
			{
				Debug.Log ("deltaTime: " + deltaTime);
				Debug.Log("Frame " + temp + ", Pause Timer 1: " + pauseTimer.ToString());
				pauseTimer = PauseCooldown;
				Debug.Log("Frame " + temp + ", Pause Timer 2: " + pauseTimer.ToString());

				IsPaused = !IsPaused;
				
				if (IsPaused) Time.timeScale = 0.0f;
				if (!IsPaused) Time.timeScale = 1.0f;
				Debug.Log("Pause Status: " + IsPaused.ToString());
				pauseTimer = 3.0f;
			}
		}
		pauseTimer -= deltaTime;

		realTimePrevious = Time.realtimeSinceStartup;
	}

}
