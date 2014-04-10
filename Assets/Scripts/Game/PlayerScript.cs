using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//player stats
	public int Score = 0;
	public static int Lives = 3;
	public const int MaxLives = 5;
	public float Health = 100;
	public float TotalHealth = 100;
	public float Stamina = 10;
	public float TotalStamina = 10;
	public int Level = 1;
	public int Experience;
	public int ExperienceToNextLevel;
	
	//player movement
	public float velocity = 1f;
	private bool WantToSprint = false;
	public bool RanOutOfStamina = false;
	public float sprintCoefficient = 5.0f;
	public float StaminaToSprint = 3;
	private float radius = 0.5f;

	//player attack
	public bool IsAttacking = false;
	public static float AttackCooldown = .25f;
	private float attackCooldownTimer = 0;
	public static float StaminaToSwing = .5f;

	//player aura
	public static bool IsAuraActive = false;
	public static bool IsAuraReady = true;
	public static float auraDuration = 5f;
	public static float auraDurationTimer = 0f;
	public static float auraCooldown = 5f;
	public static float auraCooldownTimer = 0f;
	public static float auraCost = 3f;

	//player skill shot
	public static float skillShotCooldown = 5f;
	public static float skillShotCooldownTimer = 0;
	public static float skillCost = 3f;

	// Use this for initialization
	void Start () {
		IsAuraActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Move our Player
		MovePlayer();

		//Keep the Player within the map bounds
		transform.BindToArea(-50 + radius, 50 - radius, -50 + radius, 50 - radius);

		CheckForAttack();

		CheckForAuraAttack();

		//CheckForSkillShotAttack();

		if (Stamina < TotalStamina)
			Stamina += Time.deltaTime;

		//Check if the player wants to end the game
		if (Input.GetKey("escape"))
		{
			Application.LoadLevel("StartMenuScene");
		}
	}

	#region MovePlayer
	private void MovePlayer()
	{
		//Get a Movement Vector from user input
		Vector3 movementVector = Vector3.zero;
		if (Input.GetAxis("Horizontal Movement") < -.5) movementVector += Vector3.left;
		if (Input.GetAxis("Horizontal Movement") > .5) movementVector += Vector3.right;
		if (Input.GetAxis("Vertical Movement") > -.5) movementVector += Vector3.forward;
		if (Input.GetAxis("Vertical Movement") < .5) movementVector += Vector3.back;
		movementVector.Normalize ();

		if (Input.GetButton("Sprint"))
			WantToSprint = true;
		else
			WantToSprint = false;

		//apply it to the player
		Vector3 newMovement = movementVector * velocity * Time.deltaTime;

		if (WantToSprint)
		{
			if (Stamina <= 0)
			{
				RanOutOfStamina = true;
			}

			if (RanOutOfStamina)
			{
				if (Stamina > StaminaToSprint) 
				{
					RanOutOfStamina = false;
				}
			}
			else
			{
				newMovement *= sprintCoefficient;
				Stamina -= 2.0f * Time.deltaTime;
			}

		}

		this.transform.position = (this.transform.position + newMovement);
	}

	#endregion

	private void CheckForAttack()
	{
		bool wantToAttack 		= Input.GetButton("Attack");

		//if already attacking
		if (IsAttacking)
		{
			//increment the attack cooldown timer
			attackCooldownTimer -= Time.deltaTime;

			//check if the cooldown is finished
			if (attackCooldownTimer  < 0)
			{
				IsAttacking = false;
			}
		}

		//if we are not attacking anymore
		if (!IsAttacking && wantToAttack && Stamina > StaminaToSwing)
		{
			//attack
			IsAttacking = true;
			Stamina -= StaminaToSwing;
			attackCooldownTimer = AttackCooldown;
		}

		if (IsAttacking) renderer.material.color = Color.green;
		else renderer.material.color = Color.red;
	}

	private void CheckForAuraAttack()
	{
		bool wantToTriggerAura = (Input.GetAxis("Aura") > 0.5f);

		IsAuraReady = (!IsAuraActive && auraCooldownTimer <= 0 && Stamina > auraCost);

		//if the aura is not active and we want to turn it on
		if (wantToTriggerAura && IsAuraReady)
		{
			IsAuraActive = true;
			Stamina -= auraCost;
			auraDurationTimer = auraDuration;
		}

		if (IsAuraActive) 
		{	
			auraDurationTimer -= Time.deltaTime;
			if (auraDurationTimer <= 0)
			{
				IsAuraActive = false;
				auraCooldownTimer = auraCooldown;
			}
		}
		else 
		{
			auraCooldownTimer -= Time.deltaTime;
		}

	}

	private void CheckForSkillShot()
	{
		bool wantToSkillShot = (Input.GetAxis("Skill Shot") < -0.5f);

	}

	

}
