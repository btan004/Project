using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill
{
	public string Name;
	public string Description;
	public int Level;
	public float CurrentAmount;
	public float NextAmount;

	public static int MaxUpgrades;
	protected List<float> amounts;

	public Skill(string name, string description, List<float> amounts)
	{
		Name = name;
		Description = description;
		Level = 1;
		this.amounts = amounts;

		CurrentAmount = amounts[0];

		MaxUpgrades = amounts.Count;

		if (Level != MaxUpgrades)
			NextAmount = amounts[Level];
		else
			NextAmount = CurrentAmount;
	}

	public void Upgrade()
	{
		if (Level < MaxUpgrades)
			Level++;

		CurrentAmount = amounts[Level - 1];

		MaxUpgrades = amounts.Count;

		if (Level != MaxUpgrades)
			NextAmount = amounts[Level];
		else
			NextAmount = CurrentAmount;
	}
}

