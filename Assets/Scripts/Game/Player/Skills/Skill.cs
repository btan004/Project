using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill
{
	public string Name;
	public string Description;
	public int Level;
	public float CurrentAmount;
	public float Total;
	public float NextTotal;

	public int MaxUpgrades;
	protected List<float> amounts;

	public Skill(string name, string description, List<float> amounts)
	{
		Name = name;
		Description = description;
		Level = 0;
		this.amounts = amounts;

		CurrentAmount = this.amounts[Level];
		Total = this.amounts[Level];
		MaxUpgrades = this.amounts.Count - 1;
		
		if (Level < MaxUpgrades)
		{
			NextTotal = this.amounts[Level + 1];
		}
		else
		{
			NextTotal = Total;
		}
	}

	public void Upgrade()
	{
		//if we can upgrade
		if (Level < MaxUpgrades)
		{
			//level up
			Level++;

			//get the difference
			float change = NextTotal - Total;

			//and increase our current amount by it
			CurrentAmount += change;

			//get our new total
			Total = this.amounts[Level];

			//get our new next total
			if (Level < MaxUpgrades)
			{
				NextTotal = this.amounts[Level + 1];
			}
			else
			{
				NextTotal = Total;
			}
		}
	}

	public bool IsFullyUpgraded() { return Level == MaxUpgrades; }
}

