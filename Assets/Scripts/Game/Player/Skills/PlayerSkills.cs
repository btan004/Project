using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SkillType
{
	Health,
	Stamina,
	Speed,
	Attack,
	SkillShot,
	Aura
};

public class PlayerSkills
{
	public Skill HealthSkill;
	public Skill StaminaSkill;
	public Skill VelocitySkill;
	public Skill AttackSkill;
	public Skill SkillShotSkill;
	public Skill AuraSkill;

	public int PointsToSpend;

	private AudioSource audioSource;
	private AudioClip accept;
	private AudioClip reject;

	PlayerEquipmentScript EquipmentScript;

	public PlayerSkills(PlayerEquipmentScript equipmentScript, AudioSource audioSource, AudioClip accept, AudioClip reject)
	{
		this.audioSource = audioSource;
		this.accept = accept;
		this.reject = reject;
		

		PointsToSpend = 0;

		setupHealthSkill();
		setupStaminaSkill();
		setupVelocitySkill();
		setupAttackSkill();
		setupSkillShotSkill();
		setupAuraSkill();

		EquipmentScript = equipmentScript;
	}

	public void AddSkillPoints(Difficulty difficulty)
	{
		switch (difficulty) 
		{
			case Difficulty.Easy:
				PointsToSpend += 3;
				break;
			case Difficulty.Normal:
				PointsToSpend += 2;
				break;
			case Difficulty.Hard:
				PointsToSpend += 1;
				break;
		}
	}

	private void setupHealthSkill()
	{
		List<float> healthSkillAmounts = new List<float>();
		/* Health amounts: n, n+1, n+2, n+3, etc */
		switch (WaveSystem.GameDifficulty)
		{
			case Difficulty.Easy:
				healthSkillAmounts.Add(5);
				break;
			case Difficulty.Normal:
				healthSkillAmounts.Add(10);
				break;
			case Difficulty.Hard:
				healthSkillAmounts.Add(15);
				break;
		}
		for (float health = (healthSkillAmounts[0] + 1); health <= 1000; health += 1)
			healthSkillAmounts.Add(health);

		HealthSkill = new Skill(
			"Health",
			"The amount of health that the player has.",
			healthSkillAmounts
		);
	}

	private void setupStaminaSkill()
	{
		List<float> staminaSkillAmounts = new List<float>();
		/* Stamina amounts: 5, 6, 7, 8... */
		staminaSkillAmounts.Add(5);
		for (int stamina = 6; stamina <= 200; stamina += 1)
			staminaSkillAmounts.Add(stamina);

		StaminaSkill = new Skill(
			"Stamina",
			"The amount of stamina that the player has.",
			staminaSkillAmounts
		);
	}

	private void setupVelocitySkill()
	{
		List<float> velocitySkillAmounts = new List<float>(){
			8, 9, 10, 13, 15
		};
		VelocitySkill = new Skill(
			"Velocity",
			"The player's movement speed.",
			velocitySkillAmounts
		);		
	}

	private void setupAttackSkill()
	{
		List<float> attackSkillAmounts = new List<float>();
		for (int attack = 1; attack <= 200; attack += 1)
			attackSkillAmounts.Add(attack);

		AttackSkill = new Skill(
			"Attack",
			"The Player's attack damage.",
			attackSkillAmounts
		);
	}

	private void setupSkillShotSkill()
	{
		List<float> skillShotSkillAmounts = new List<float>() {
			1.0f, 1.5f, 2.0f, 2.5f, 3.0f
		};
		SkillShotSkill = new Skill(
			"Skill Shot",
			"The Player's Skill Shot damage.",
			skillShotSkillAmounts
		);
	}

	private void setupAuraSkill()
	{
		List<float> auraSkillAmounts = new List<float>()
		{
			1.0f, 1.5f, 2.0f, 2.5f, 3.0f
		};
		AuraSkill = new Skill(
			"Aura Skill",
			"The Player's Aura Skill damage.",
			auraSkillAmounts
		);
	}

	public void UpdgradeSkill(SkillType type)
	{
		//if the player has points to spend
		if (PointsToSpend > 0)
		{
			switch (type)
			{
				//and the player would like to upgrade their health skill
				case (SkillType.Health):
					//and the health skill isn't fully upgraded
					if (!HealthSkill.IsFullyUpgraded()) 
					{
						//upgrade the health skill and play the accept sound
						playAcceptSound();
						HealthSkill.Upgrade();
						PointsToSpend--;
						//if the player's new health is a multiple of 5, 
						//upgrade their shield appearance
						if (HealthSkill.Total % 5 == 0)
							EquipmentScript.UpgradeShield();
					}
						//otherwise, play the reject sound
					else playRejectSound();
					break;
				case (SkillType.Stamina):
					if (!StaminaSkill.IsFullyUpgraded())
					{
						playAcceptSound();
						StaminaSkill.Upgrade();
						PointsToSpend--;
					}
					else playRejectSound();
					break;
				case (SkillType.Speed):
					if (!VelocitySkill.IsFullyUpgraded())
					{
						playAcceptSound();
						VelocitySkill.Upgrade();
						PointsToSpend--;
					}
					else playRejectSound();
					break;
				case (SkillType.Attack):
					if (!AttackSkill.IsFullyUpgraded())
					{
						playAcceptSound();

						AttackSkill.Upgrade();

						//if ((AttackSkill.Level - 4) % 5 == 0)
						if (AttackSkill.Total % 5 == 0)
							EquipmentScript.UpgradeWeapon();

						PointsToSpend--;
					}
					else playRejectSound();
					break;
				case (SkillType.SkillShot):
					if (!SkillShotSkill.IsFullyUpgraded())
					{
						playAcceptSound();
						SkillShotSkill.Upgrade();
						PointsToSpend--;
					}
					else playRejectSound();
					break;
				case (SkillType.Aura):
					if (!AuraSkill.IsFullyUpgraded())
					{
						playAcceptSound();
						AuraSkill.Upgrade();
						PointsToSpend--;
					}
					else playRejectSound();
					break;
				default:
					break;
			}
			
		}
	}

	private void playAcceptSound()
	{
		audioSource.clip = accept;
		audioSource.Play();
	}

	private void playRejectSound()
	{
		audioSource.clip = reject;
		audioSource.Play();
	}

	public float GetPlayerHealth() { return HealthSkill.CurrentAmount; }
	public float GetPlayerHealthMax() { return HealthSkill.Total; }
	public float GetPlayerStamina() { return StaminaSkill.CurrentAmount; }
	public float GetPlayerStaminaMax() { return StaminaSkill.Total; }
	public float GetPlayerVelocity() { return VelocitySkill.CurrentAmount; }
	public float GetPlayerDamage() { return AttackSkill.CurrentAmount; }
	public float GetPlayerSkillShotDamage() { return SkillShotSkill.CurrentAmount; }
	public float GetAuraDamage() { return AuraSkill.CurrentAmount; }

	public void AddSkillPoint() { PointsToSpend++; }
}

