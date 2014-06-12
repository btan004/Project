using UnityEngine;
using System;

public class EnemyBuff
{
	public BuffType Type;
	public float Value;
	public float Duration;
	public float Lifetime;

	public EnemyBuff(BuffType type, float value, float duration, float lifetime)
	{
		Type = type;
		Value = value;
		Duration = duration;
		Lifetime = lifetime;
	}
}

public enum BuffType
{
	Damage, Velocity, AttackRate, Health, MaxHealth
}