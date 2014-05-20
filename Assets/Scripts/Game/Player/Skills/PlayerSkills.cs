﻿using UnityEngine;
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

	PlayerEquipmentScript EquipmentScript;

	public PlayerSkills(PlayerEquipmentScript equipmentScript)
	{
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
		/* Health amounts: 100, 120, 140, ..., 960, 980, 1000 */
		for (int health = 100; health <= 1000; health += 20)
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
		List<float> attackSkillAmounts = new List<float>() {
			10, 20, 30, 40, 50
		};
		AttackSkill = new Skill(
			"Attack",
			"The Player's attack damage.",
			attackSkillAmounts
		);
	}

	private void setupSkillShotSkill()
	{
		List<float> skillShotSkillAmounts = new List<float>() {
			20, 50, 100, 150, 200
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
			10, 20, 30, 40, 50
		};
		AuraSkill = new Skill(
			"Aura Skill",
			"The Player's Aura Skill damage.",
			auraSkillAmounts
		);
	}

	public void UpdgradeSkill(SkillType type)
	{
		if (PointsToSpend > 0)
		{
			switch (type)
			{
				case (SkillType.Health):
					if (!HealthSkill.IsFullyUpgraded())
					{
						HealthSkill.Upgrade();
						PointsToSpend--;
						EquipmentScript.UpgradeShield();
					}
					break;
				case (SkillType.Stamina):
					if (!StaminaSkill.IsFullyUpgraded())
					{
						StaminaSkill.Upgrade();
						PointsToSpend--;
					}
					break;
				case (SkillType.Speed):
					if (!VelocitySkill.IsFullyUpgraded())
					{
						VelocitySkill.Upgrade();
						PointsToSpend--;
					}
					break;
				case (SkillType.Attack):
					if (!AttackSkill.IsFullyUpgraded())
					{
						AttackSkill.Upgrade();
						PointsToSpend--;
						EquipmentScript.UpgradeWeapon();
					}
					break;
				case (SkillType.SkillShot):
					if (!SkillShotSkill.IsFullyUpgraded())
					{
						SkillShotSkill.Upgrade();
						PointsToSpend--;
					}
					break;
				case (SkillType.Aura):
					if (!AuraSkill.IsFullyUpgraded())
					{
						AuraSkill.Upgrade();
						PointsToSpend--;
					}
					break;
				default:
					break;
			}
			
		}
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

