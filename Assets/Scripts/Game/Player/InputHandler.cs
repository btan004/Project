using UnityEngine;
using System.Collections;

public class InputHandler
{

	//Input
	public static Vector3 MovementVector;
	public static Vector3 DirectionVector;
	public static bool WantToSprint;
	public static bool WantToAttack;
	public static bool WantToAura;
	public static bool WantToSkillShot;
	public static bool WantToQuit;
	public static bool WantToSpendSkillPoint;
	public static bool WantToChangeSkillUp;
	public static bool WantToChangeSkillDown;

	public static bool WantToStartGame;
	public static bool WantToChangeDifficulty;
	public static bool WantToViewControls;

	public const double AnalogTolerance = 0.5;
	public static bool IsPaused;
	public static float PauseCooldown = 3.0f;
	private float pauseTimer;
	private float realTimePrevious;
	static int temp;

	private static double disabledTimer;
	public static double disabledTimerCooldown = 2.0;


	//Debug variables
	public bool WantToSpawnEnemy;

	// Use this for initialization
	public InputHandler()
	{
		MovementVector = Vector3.zero;
		DirectionVector = Vector3.zero;
		WantToSprint = false;
		IsPaused = false;

		pauseTimer = 0.0f;
		temp = 0;

		disabledTimer = 0;
	}

	public static void DisableInput()
	{
		Debug.LogError("Disabling Input for 2.0 seconds...");

		disabledTimer = disabledTimerCooldown;
		//while input it disabled, make sure values are all false
		MovementVector = Vector3.zero;
		DirectionVector = Vector3.zero;
		WantToSprint = false;
		WantToAttack = false;
		WantToAura = false;
		WantToQuit = false;
		WantToSpendSkillPoint = false;
		WantToChangeSkillUp = false;
		WantToChangeSkillDown = false;
		WantToStartGame = false;
		WantToChangeDifficulty = false;
		WantToViewControls = false;
	}

	// Update is called once per frame
	public void Update()
	{
		disabledTimer -= Time.deltaTime;

		if (disabledTimer <= 0)
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

			CheckForceSpawnWave();


			CheckWantToStartGame();
			CheckWantToChangeDifficulty();
			CheckWantToViewControls();
			CheckForQuit();
		}
		else
		{
			//while input it disabled, make sure values are all false
			MovementVector = Vector3.zero;
			DirectionVector = Vector3.zero;
			WantToSprint = false;
			WantToAttack = false;
			WantToAura = false;
			WantToQuit = false;
			WantToSpendSkillPoint = false;
			WantToChangeSkillUp = false;
			WantToChangeSkillDown = false;
			WantToStartGame = false;
			WantToChangeDifficulty = false;
			WantToViewControls = false;
		}
	}

	private void CheckForceSpawnWave()
	{
		//WaveSystem.ForceSpawnWave = Input.GetButton("Y Button");
		if (Input.GetButton("Y Button"))
			WaveSystem.instance.StartWaveCountdown();
	}

	private void CheckWantToStartGame()
	{
		WantToStartGame = Input.GetButton("A Button");
	}

	private void CheckWantToChangeDifficulty()
	{
		WantToChangeDifficulty = Input.GetButton("B Button");
	}

	private void CheckWantToViewControls()
	{
		WantToViewControls = Input.GetButton("X Button");
	}

	//supports: keyboard
	private void CheckForQuit()
	{
		WantToQuit = Input.GetButton("Y Button");
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
			DirectionVector.x = Input.GetAxis("Horizontal Direction");
		if (Mathf.Abs(Input.GetAxis("Vertical Direction")) > AnalogTolerance)
			DirectionVector.z = -1 * Input.GetAxis("Vertical Direction");
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
		WantToChangeSkillUp = false;
		WantToChangeSkillDown = false;

		//if (Input.GetAxis("SkillSelect") < -.5)
		//	WantToChangeSkillLeft = true;
		//if (Input.GetAxis("SkillSelect") > .5)
		//	WantToChangeSkillRight = true;
		WantToChangeSkillUp = (Input.GetAxis("SkillSelect") < -.5);
		WantToChangeSkillDown = (Input.GetAxis("SkillSelect") > .5);

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
				Debug.Log("deltaTime: " + deltaTime);
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
