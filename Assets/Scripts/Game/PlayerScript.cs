using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	//player input
	private InputHandler		inputHandler;

	//player stats
	public int					Score = 0;
	public static int			Lives = 3;
	public const int			MaxLives = 5;
	public float				Health = 100;
	public float				TotalHealth = 100;
	public float				Stamina = 10;
	public float				TotalStamina = 10;
	public int					Level = 1;
	public int					Experience;
	public int					ExperienceToNextLevel;
	public float				Radius = 0.5f;

	//player movement
	public float				SprintCoefficient = 5.0f;
	public float				StaminaToSprint = 3;
	public float				Velocity = 1f;
	public bool					RanOutOfStamina = false;

	//player attack
	public bool					IsAttacking = false;
	public static float		StaminaToAttack = .5f;
	public static float		AttackCooldown = .25f;
	private float				attackCooldownTimer = 0;
	
	//player aura
	public static bool		IsAuraActive = false;
	public static bool		IsAuraReady = true;
	public static float		AuraDuration = 5f;
	public static float		AuraCooldown = 5f;
	public static float		AuraCost = 3f;
	private static float		auraDurationTimer = 0f;
	private static float		auraCooldownTimer = 0f;

	//player skill shot
	public static bool		IsSkillShotActive = false;
	public static bool		IsSkillShotReady = true;
	public static float		SkillShotCooldown = 2f;
	public static float		SkillShotCost = 3f;
	private static float		skillShotCooldownTimer = 0;


	// Use this for initialization
	void Start () {
		inputHandler = new InputHandler();

		IsAuraActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Check for user input
		inputHandler.Update();

		//Handle player movement
		CheckForMovement();

		//Handle our melee attack, aura attack, and skill shot
		CheckForAttack();
		CheckForAuraAttack();
		CheckForSkillShot();

		//Regen stamina based on time
		if (Stamina < TotalStamina)
			Stamina += Time.deltaTime;

		//Check if the player wants to end the game
		if (inputHandler.WantToQuit) Application.LoadLevel("StartMenuScene");
	}

	private void CheckForMovement()
	{
		//apply it to the player
		Vector3 newMovement = inputHandler.MovementVector * Velocity * Time.deltaTime;

		//if the player wants to sprint
		if (inputHandler.WantToSprint)
		{
			//and they run out of stamina
			if (Stamina <= 0)
			{
				//signal that we have run out of stamina
				RanOutOfStamina = true;
			}

			//if we have run out of stamina
			if (RanOutOfStamina)
			{
				//check if we have enough stamina to sprint again
				if (Stamina > StaminaToSprint) 
				{
					//then disable our lock against sprinting again
					RanOutOfStamina = false;
				}
			}
			//if we have not run out of stamina
			else
			{
				//multiply our movement by our sprint coefficient
				newMovement *= SprintCoefficient;

				//and decrement our stamina for sprinting
				Stamina -= 2.0f * Time.deltaTime;
			}
		}

		//make our player movement
		this.transform.position = (this.transform.position + newMovement);

		//Keep the Player within the map bounds
		transform.BindToArea(-50 + Radius, 50 - Radius, -50 + Radius, 50 - Radius);
	}

	private void CheckForAttack()
	{
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
		if (!IsAttacking && inputHandler.WantToAttack && Stamina > StaminaToAttack)
		{
			//attack: use up stamina and reset our attack cooldown
			IsAttacking = true;
			Stamina -= StaminaToAttack;
			attackCooldownTimer = AttackCooldown;
		}

		//debug: green while attacking, red otherwise
		if (IsAttacking) renderer.material.color = Color.green;
		else renderer.material.color = Color.red;
	}

	private void CheckForAuraAttack()
	{
		//Check if the aura attack is ready:
		//		1.) Aura isnt currently active
		//		2.) Aura cooldown timer is ready
		//		3.) We have enough stamina to use the aura
		IsAuraReady = (!IsAuraActive && auraCooldownTimer <= 0 && Stamina > AuraCost);

		//if we want to use the aura and it is ready
		if (inputHandler.WantToAura && IsAuraReady)
		{
			//set aura to active, use up our stamina, and reset the aura duration timer
			IsAuraActive = true;
			Stamina -= AuraCost;
			auraDurationTimer = AuraDuration;
		}

		//if the aura is active
		if (IsAuraActive) 
		{	
			//decrement our aura duration timer
			auraDurationTimer -= Time.deltaTime;

			//if the aura duration gets down to 0
			if (auraDurationTimer <= 0)
			{
				//disable the aura and reset the cooldown timer
				IsAuraActive = false;
				auraCooldownTimer = AuraCooldown;
			}
		}
		//if the aura isnt active
		else 
		{
			//decrement the cooldown timer
			auraCooldownTimer -= Time.deltaTime;
		}

	}

	private void CheckForSkillShot()
	{
		//Check if the skill shot attack is ready:
		//		1.) Skill Shot isnt currently active
		//		2.) Skill Shot timer is ready
		//		3.) We have enough stamina to use the Skill Shot
		IsSkillShotReady = (!IsSkillShotActive && skillShotCooldownTimer <= 0 && Stamina > SkillShotCost);

		//if we want to use the skill shot and it is ready
		if (inputHandler.WantToSkillShot && IsSkillShotReady)
		{
			//use the skill shot: set active, use up stamina, and reset the cooldown timer
			IsSkillShotActive = true;
			Stamina -= SkillShotCost;
			skillShotCooldownTimer = SkillShotCooldown;
		}

		//decrease our skill shot cooldown timer
		skillShotCooldownTimer -= Time.deltaTime;

		//check if we are ready to use the skill shot again
		if (skillShotCooldownTimer < 0)
		{
			IsSkillShotActive = false;
		}
	}

	public float GetAuraDurationPercentage()
	{
		return auraDurationTimer / AuraDuration;
	}
	
	public float GetAuraCooldownPercentage()
	{
		return (AuraCooldown - auraCooldownTimer) / AuraCooldown;
	}

	public float GetSkillShotCooldownPercentage()
	{
		return (SkillShotCooldown - skillShotCooldownTimer) / SkillShotCooldown;
	}

}
