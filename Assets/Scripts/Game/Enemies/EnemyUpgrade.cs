using UnityEngine;
using System.Collections;

public class EnemyUpgrade
{
	public float Health;
	public float Velocity;
	public float Damage;
	public float AttackRate;
	public float Experience;

	public EnemyUpgrade(float health, float velocity, float damage, float attackRate, float experience)
	{
		Health = health;
		Velocity = velocity;
		Damage = damage;
		AttackRate = attackRate;
		Experience = experience;
	}
}

