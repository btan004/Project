using UnityEngine;
using System.Collections;

public class Powerup
{
	public PowerupType Type;
	public float Amount;
	public float Duration;
	public float Lifetime;

	public Powerup(PowerupType type, float amount, float duration, float lifetime)
	{
		Type = type;
		Amount = amount;
		Duration = duration;
		Lifetime = lifetime;
	}

	public Powerup(PowerupScript script)
	{
		Type = script.Type;
		Amount = script.Amount;
		Duration = script.Duration;
		Lifetime = script.Lifetime;
	}
}

