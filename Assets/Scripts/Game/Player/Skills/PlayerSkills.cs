using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SkillType
{
	Health,
	Stamina,
	Speed
};

public class PlayerSkills
{
	public Skill HealthSkill;
	public Skill StaminaSkill;
	public Skill SpeedSkill;

	public int PointsToSpend;


	public PlayerSkills()
	{
		PointsToSpend = 0;

		List<float> healthSkillAmounts = new List<float>(){
			200, 250, 300, 350, 400
		};
		HealthSkill = new Skill(
			"Health",
			"The amount of health that the player has.",
			healthSkillAmounts
		);

		List<float> staminaSkillAmounts = new List<float>(){
			10, 15, 20, 25, 30
		};
		StaminaSkill = new Skill(
			"Stamina",
			"The amount of stamina that the player has.",
			staminaSkillAmounts
		);

		List<float> speedSkillAmounts = new List<float>(){
			1, 1.1f, 1.2f, 1.3f, 1.4f
		};
		SpeedSkill = new Skill(
			"Speed",
			"A speed multiplier for the player.",
			speedSkillAmounts
		);
	}

	public void UpdgradeSkill(SkillType type)
	{
		if (PointsToSpend > 0)
		{
			switch (type)
			{
				case (SkillType.Health):
					HealthSkill.Upgrade();
					break;
				case (SkillType.Stamina):
					StaminaSkill.Upgrade();
					break;
				case (SkillType.Speed):
					SpeedSkill.Upgrade();
					break;
				default:
					break;
			}
			PointsToSpend--;
		}
	}

	public float GetPlayerHealth() { return HealthSkill.CurrentAmount; }
	public float GetPlayerStamina() { return StaminaSkill.CurrentAmount; }
	public void AddSkillPoint() { PointsToSpend++; }
}

