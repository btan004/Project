using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

	public static bool INVULNERABLE;

	//player input
	public InputHandler		inputHandler;
	public CursorScript		cursor;

	//player skills and leveling
	public PlayerSkills Skills;
	public PlayerEquipmentScript Equipment;
	//public LevelSystem LevelSystem;

	//player stats
	public static float		Score = 0;
	public static int		Lives;
	public const int		MaxLives = 5;

	public float			Radius = 2f;

	//player movement
	public bool 			CanSprint;
	public float			SprintCoefficient = 5.0f;
	public float			StaminaToSprint = 3;

	public bool				RanOutOfStamina = false;
	public float			FinalMoveSpeed = 0;

	//knockback
	public float			Mass = 10f;
	private Vector3			knockback;

	//player attack
	public static bool		IsStartingToAttack = false;	//when the animation starts playing
	public static bool		IsAttacking = false;				//when the damage is dealt
	public bool				IsAttackReady = false;
	public static float		StaminaToAttack = .5f;
	public static float		AttackCooldown = .25f;
	public float			attackCooldownTimer = 0;
	private bool			waitingForAnimationDelay;
	public const float		AttackAnimationDelay = 0.36f;
	private float			attackAnimationDelayTimer;


	//player aura
	public static bool		IsAuraActive = false;
	public static bool		IsAuraReady = true;
	public static float		AuraDuration = 5f;
	public static float		AuraCooldown = 5f;
	public static float		AuraCost = 3f;
	private static float		auraDurationTimer = 0f;

	private static float		auraCooldownTimer = 0f;
	public static float		AuraForce = 1f;

	//player skill shot
	public bool					IsSkillShotActive = false;
	public static bool		IsSkillShotReady = true;
	public static float		SkillShotCooldown = 2f;
	public static float		SkillShotCost = 3f;
	private static float		skillShotCooldownTimer = 0;

	//powerups
	public List<Powerup> ActivePowerups = new List<Powerup>();
	private float healthFromPowerups = 0;
	private float staminaFromPowerups = 0;
	private float movespeedFromPowerups = 1;

	// Animations
	public Animator anim;
	private static int player_attackingState = Animator.StringToHash ("AttackLayer.attacking");
	private static int player_StateBaseLayer = 0;
	private static int player_StateAttackLayer = 1;
	private AnimatorStateInfo currentAtkState;
	public Animation PlayerAnimation;
	public bool IsMoving;
	public bool IsHit;

	public bool debugvar = false;

	// Use this for initialization
	void Start () {
		INVULNERABLE = true;
		WaveSystem.GameDifficulty = Difficulty.Easy;

		anim = GetComponent<Animator>();
		currentAtkState = anim.GetCurrentAnimatorStateInfo (player_StateAttackLayer);

		inputHandler = new InputHandler();
		IsAuraActive = false;

		//LevelSystem = new LevelSystem();
		//Skills = LevelSystem.GetPlayerSkills();
		//Skills.AddSkillPoint();
		Skills = new PlayerSkills (Equipment);
		Skills.AddSkillPoints (WaveSystem.GameDifficulty);

		//get the proper amount of lives
		Lives = WaveSystem.LivesPerDifficulty[(int)WaveSystem.GameDifficulty];

		renderer.material.color = Color.red;
		knockback = new Vector3();
		Mass = 10f;




		Physics.IgnoreLayerCollision (9, 9);

		PlayerAnimation.animation["attack"].speed = 2.5f;
	}
	
	// Update is called once per frame
	void Update () {
		//check for death
		CheckForDeath();
		//Clear animation variables
		//ClearAnimationInfo();

		currentAtkState = anim.GetCurrentAnimatorStateInfo (player_StateAttackLayer);

		//Check for user input
		inputHandler.Update();

		//Update our active powerups
		UpdateActivePowerups();

		//Handle player movement
		CheckForMovement();

		//Handle our melee attack, aura attack, and skill shot
		CheckForAttack();
		CheckForAuraAttack();
		CheckForSkillShot();

		//Regen stamina based on time
		Skills.StaminaSkill.CurrentAmount = Mathf.Clamp(Skills.GetPlayerStamina() + Time.deltaTime, 0, Skills.GetPlayerStaminaMax());

		//Health + Stamina from powerup
		Skills.HealthSkill.CurrentAmount = Mathf.Clamp(Skills.GetPlayerHealth() + healthFromPowerups, 0, Skills.GetPlayerHealthMax());
		Skills.StaminaSkill.CurrentAmount = Mathf.Clamp(Skills.GetPlayerStamina() + staminaFromPowerups, 0, Skills.GetPlayerStaminaMax());

		//Check if the player wants to end the game
		if (InputHandler.WantToQuit) Application.LoadLevel("StartMenuScene");

		//animate the player
		//AnimateSkeleton(IsHit, IsAttacking, IsMoving);
	}

	private void UpdateActivePowerups()
	{
		//reset our powerup pools
		healthFromPowerups = 0;			//flat amount
		staminaFromPowerups = 0;		//flat amount
		movespeedFromPowerups = 1;		//multiplier

		//for all powerups that are active on the player
		foreach (Powerup p in ActivePowerups)
		{
			//if they are health regen powerups, add to the health from powerups pool
			if (p.Type == PowerupType.HealthRegen)
				healthFromPowerups += p.Amount * Time.deltaTime;

			//if they are stamina regen powerups, add to the stamina from powerups pool
			if (p.Type == PowerupType.StaminaRegen)
				staminaFromPowerups += p.Amount * Time.deltaTime;

			//if they are move speed powerups, add to the movespeed from powerups pool
			if (p.Type == PowerupType.MovementSpeed)
				movespeedFromPowerups *= p.Amount;

			//make sure we havent exceeded our movement speed cap
			if (movespeedFromPowerups > PowerupInfo.MovementSpeedCap)
				movespeedFromPowerups = PowerupInfo.MovementSpeedCap;

			//reduce the duration of the powerup
			p.Duration -= Time.deltaTime;
		}

		//go back and remove any powerups that have died
		ActivePowerups.RemoveAll(IsPowerupDead);
	}

	private static bool IsPowerupDead(Powerup p)
	{
		return (p.Duration <= 0);
	}

	private void CheckForDeath()
	{
		if (Skills.GetPlayerHealth() <= 0) {
			if (Lives > 1) {
				Lives--;
				Skills.HealthSkill.CurrentAmount = Skills.GetPlayerHealthMax();
			}
			else {
				if (!INVULNERABLE)
					Application.LoadLevel("StartMenuScene");
			}
		}
	}

	private void CheckForMovement()
	{
		//apply it to the player
		IsMoving = false;
		Vector3 newMovement = InputHandler.MovementVector * Skills.GetPlayerVelocity() * movespeedFromPowerups * Time.deltaTime;

		FinalMoveSpeed = Skills.GetPlayerVelocity() * movespeedFromPowerups;

		CanSprint = (Skills.GetPlayerStamina () >= StaminaToSprint);

		//if the player wants to sprint
		CanSprint = false;

		//and they run out of stamina
		if (Skills.GetPlayerStamina() <= 0)
		{
			//signal that we have run out of stamina
			RanOutOfStamina = true;
		}

		//if we have run out of stamina
		if (RanOutOfStamina)
		{
			//check if we have enough stamina to sprint again
			if (Skills.GetPlayerStamina() > StaminaToSprint) 
			{
				//then disable our lock against sprinting again
				RanOutOfStamina = false;
			}
		}
		//if we have not run out of stamina
		else
		{
			CanSprint = true;
			if (InputHandler.WantToSprint)
			{
				//multiply our movement by our sprint coefficient
				newMovement *= SprintCoefficient;
				FinalMoveSpeed *= SprintCoefficient;

				//and decrement our stamina for sprinting
				Skills.StaminaSkill.CurrentAmount -= 2.0f * Time.deltaTime;
			}
		}

		//make our player movement
		if (newMovement != Vector3.zero)
		{
			//this.transform.position = (this.transform.position + newMovement);
			this.GetComponent<CharacterController>().Move(newMovement);
			IsMoving = true;
		}

		//apply knockback
		ApplyKnockback();

		//turn the player to the cursor
		PlayerAnimation.transform.rotation = Quaternion.LookRotation(cursor.transform.position - transform.position);

		anim.SetFloat ("Speed", Mathf.Clamp(InputHandler.MovementVector.magnitude, 0, 1));

		//make sure our player stays on the ground plan
		this.transform.SetPositionY(1);
	}

	private void CheckForAttack()
	{
		IsAttacking = false;

		IsAttackReady = (!IsAttacking && attackCooldownTimer <= 0 && Skills.GetPlayerStamina() > StaminaToAttack);
	
		if (InputHandler.WantToAttack && IsAttackReady && currentAtkState.nameHash != player_attackingState )
		{
			//IsAttacking = true;
			Skills.StaminaSkill.CurrentAmount -= StaminaToAttack;
			attackCooldownTimer = AttackCooldown;

			waitingForAnimationDelay = true;
			attackAnimationDelayTimer = AttackAnimationDelay;
		}

		attackCooldownTimer -= Time.deltaTime;

		if (attackCooldownTimer < 0)
		{
			IsAttackReady = true;
		}
		Debug.Log ( anim.IsInTransition(player_StateAttackLayer) );
		debugvar = anim.IsInTransition (player_StateAttackLayer);
		if (waitingForAnimationDelay)
		{
			attackAnimationDelayTimer -= Time.deltaTime;
			if (attackAnimationDelayTimer <= 0)
			{
				IsAttacking = true;
				waitingForAnimationDelay = false;
			}
		}
		anim.SetBool("Attacking", waitingForAnimationDelay);
	}

	private void CheckForAuraAttack()
	{
		//Check if the aura attack is ready:
		//		1.) Aura isnt currently active
		//		2.) Aura cooldown timer is ready
		//		3.) We have enough stamina to use the aura
		IsAuraReady = (!IsAuraActive && auraCooldownTimer <= 0 && Skills.GetPlayerStamina() > AuraCost);

		//if we want to use the aura and it is ready
		if (InputHandler.WantToAura && IsAuraReady)
		{
			//set aura to active, use up our stamina, and reset the aura duration timer
			IsAuraActive = true;
			Skills.StaminaSkill.CurrentAmount -= AuraCost;
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
		IsSkillShotActive = false;

		//Check if the skill shot attack is ready:
		//		1.) Skill Shot isnt currently active
		//		2.) Skill Shot timer is ready
		//		3.) We have enough stamina to use the Skill Shot
		IsSkillShotReady = (!IsSkillShotActive && skillShotCooldownTimer <= 0 && Skills.GetPlayerStamina() > SkillShotCost);

		//if we want to use the skill shot and it is ready
		if (InputHandler.WantToSkillShot && IsSkillShotReady)
		{
			//use the skill shot: set active, use up stamina, and reset the cooldown timer
			IsSkillShotActive = true;
			Skills.StaminaSkill.CurrentAmount -= SkillShotCost;
			skillShotCooldownTimer = SkillShotCooldown;
		}

		//decrease our skill shot cooldown timer
		skillShotCooldownTimer -= Time.deltaTime;

		//check if we are ready to use the skill shot again
		if (skillShotCooldownTimer < 0)
		{
			IsSkillShotReady = true;
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
		return Mathf.Clamp01((SkillShotCooldown - skillShotCooldownTimer) / SkillShotCooldown);
	}

	public void ApplyPowerup(Powerup powerup)
	{
		switch(powerup.Type)
		{
			case (PowerupType.Health):
				Skills.HealthSkill.CurrentAmount = Mathf.Clamp(Skills.GetPlayerHealth() + powerup.Amount, 0, Skills.GetPlayerHealthMax());
				break;
			case (PowerupType.HealthRegen):
				ActivePowerups.Add(powerup);
				break;
			case (PowerupType.Stamina):
				Skills.StaminaSkill.CurrentAmount = Mathf.Clamp(Skills.GetPlayerStamina() + powerup.Amount, 0, Skills.GetPlayerStaminaMax());
				break;
			case (PowerupType.StaminaRegen):
				ActivePowerups.Add(powerup);
				break;
			case (PowerupType.MovementSpeed):
				ActivePowerups.Add(powerup);
				break;
			case (PowerupType.Experience):
				ApplyExperience(powerup.Amount);
				break;
			default:
				break;
		}
	}

	public void ApplyDamage(float damage)
	{
		if (Skills.GetPlayerHealth() <= damage) Skills.HealthSkill.CurrentAmount = 0;
		else Skills.HealthSkill.CurrentAmount -= damage;
	}

	public void AddKnockback(Vector3 direction, float force)
	{
		direction = new Vector3 (direction.x,0,direction.z);
		direction.Normalize ();
		knockback = direction * (force / Mass);
	}

	protected void ApplyKnockback()
	{
		//this.transform.position = this.transform.position + knockback;
		this.GetComponent<CharacterController>().Move(knockback*Time.deltaTime);


		knockback = Vector3.Lerp(knockback, Vector3.zero, 5 * Time.deltaTime);
	}

	public void ApplyExperience(float experience)
	{
		//LevelSystem.ApplyExperience(experience);
		Score += (experience * 10f);
	}

	public void ClearAnimationInfo()
	{
		IsMoving = false;
		IsStartingToAttack = false;
		IsAttacking = false;
		IsHit = false;
	}

	public void AnimateSkeleton(bool isHit, bool isAttacking, bool isMoving)
	{

		if (IsHit)
		{
			PlayerAnimation.Play("gethit");
		}
		else if (IsAttacking && !PlayerAnimation.IsPlaying("attack"))
		{
			PlayerAnimation.Play("attack");
		}
		else if (IsMoving && !PlayerAnimation.IsPlaying("run") && !PlayerAnimation.IsPlaying("attack"))
		{
			PlayerAnimation.Play("run");
		}
		else if (!PlayerAnimation.isPlaying)
		{
			PlayerAnimation.Play("idle");
		}

	}
}
