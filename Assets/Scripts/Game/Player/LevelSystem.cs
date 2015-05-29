using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
public class LevelSystem
{
	//level and experience 
	public int		CurrentLevel;
	public float	CurrentExperience;
	public float	ExperienceToNextLevel;
	public static int MaxLevel = 35;
	public List<float> ExperienceRequiredPerLevel = new List<float>();

	//skills
	private PlayerSkills playerSkills;

	public LevelSystem()
	{
		CurrentLevel = 1;
		CurrentExperience = 0;
		for (float level = 1; level <= 35; level++)
			ExperienceRequiredPerLevel.Add(100f);
		ExperienceToNextLevel = ExperienceRequiredPerLevel[CurrentLevel - 1];
		playerSkills = new PlayerSkills();
	}

	public void ApplyExperience(float experience)
	{
		CurrentExperience += experience;

		if (CurrentExperience >= ExperienceToNextLevel && CurrentLevel < MaxLevel)
		{
			CurrentExperience -= ExperienceToNextLevel;
			CurrentLevel++;
			//playerSkills.AddSkillPoint();
			ExperienceToNextLevel = ExperienceRequiredPerLevel[CurrentLevel - 1];
		}
	}

	public PlayerSkills GetPlayerSkills() { return playerSkills; }
}

*/