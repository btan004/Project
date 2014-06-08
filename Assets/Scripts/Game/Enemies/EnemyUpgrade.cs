using UnityEngine;
using System.Collections;

public class EnemyUpgrade
{
	public float Health;
	public float Velocity;
	public float Damage;
	public float AttackRate;

	public EnemyUpgrade(float health, float velocity, float damage, float attackRate)
	{
		Health = health;
		Velocity = velocity;
		Damage = damage;
		AttackRate = attackRate;
	}
}

