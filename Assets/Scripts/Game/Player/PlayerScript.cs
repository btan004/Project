﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

	//player input
	public InputHandler		inputHandler;

	//player stats
	public int					Score = 0;
	public static int			Lives = 3;
	public const int			MaxLives = 5;
	public float				Health = 100;
	public float				TotalHealth = 100;
	public float				Stamina = 10;
	public float				TotalStamina = 10;
	public int					Level = 1;
	public float				Experience;
	public float				ExperienceToNextLevel;
	public float				Radius = 2f;

	//player movement
	public float				SprintCoefficient = 5.0f;
	public float				StaminaToSprint = 3;
	public float				Velocity = 1f;
	public bool					RanOutOfStamina = false;
	public float				FinalMoveSpeed = 0;

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

	//powerups
	public List<Powerup> ActivePowerups = new List<Powerup>();
	private float healthFromPowerups = 0;
	private float staminaFromPowerups = 0;
	private float movespeedFromPowerups = 1;
	

	// Use this for initialization
	void Start () {
		inputHandler = new InputHandler();
		IsAuraActive = false;
		ExperienceToNextLevel = 100;
	}
	
	// Update is called once per frame
	void Update () {
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
		Stamina = Mathf.Clamp(Stamina + Time.deltaTime, 0, TotalStamina);

		//Health + Stamina from powerup
		Health = Mathf.Clamp(Health + healthFromPowerups, 0, TotalHealth);
		Stamina = Mathf.Clamp(Stamina + staminaFromPowerups, 0, TotalStamina);

		//Check if the player wants to end the game
		if (InputHandler.WantToQuit) Application.LoadLevel("StartMenuScene");
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
				movespeedFromPowerups += p.Amount * Time.deltaTime;

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

	private void CheckForMovement()
	{
		//apply it to the player
		Vector3 newMovement = InputHandler.MovementVector * Velocity * movespeedFromPowerups * Time.deltaTime;

		FinalMoveSpeed = Velocity * movespeedFromPowerups;

		//if the player wants to sprint
		if (InputHandler.WantToSprint)
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
				FinalMoveSpeed *= SprintCoefficient;

				//and decrement our stamina for sprinting
				Stamina -= 2.0f * Time.deltaTime;
			}
		}

		//make our player movement
		this.transform.position = (this.transform.position + newMovement);

		//Keep the Player within the map bounds
		transform.BindToArea(
			MapInfo.MinimumX + Radius, 
			MapInfo.MaximumX - Radius, 
			MapInfo.MinimumZ + Radius, 
			MapInfo.MaximumZ - Radius);

		
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
		if (!IsAttacking && InputHandler.WantToAttack && Stamina > StaminaToAttack)
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
		if (InputHandler.WantToAura && IsAuraReady)
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
		if (InputHandler.WantToSkillShot && IsSkillShotReady)
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
		return Mathf.Clamp01((SkillShotCooldown - skillShotCooldownTimer) / SkillShotCooldown);
	}

	public void ApplyPowerup(Powerup powerup)
	{
		switch(powerup.Type)
		{
			case (PowerupType.Health):
				Health = Mathf.Clamp(Health + powerup.Amount, 0, TotalHealth);
				break;
			case (PowerupType.HealthRegen):
				ActivePowerups.Add(powerup);
				break;
			case (PowerupType.Stamina):
				Stamina = Mathf.Clamp(Stamina + powerup.Amount, 0, TotalStamina);
				break;
			case (PowerupType.StaminaRegen):
				ActivePowerups.Add(powerup);
				break;
			case (PowerupType.MovementSpeed):
				ActivePowerups.Add(powerup);
				break;
			case (PowerupType.Experience):
				Experience += (int) powerup.Amount;
				break;
			default:
				break;
		}
	}

	public void ApplyDamage(float damage)
	{
		if (Health <= damage) Health = 0;
		else Health -= damage;
	}
}
