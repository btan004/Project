using UnityEngine;
using System.Collections;
using System;

public class Charger_AttackPlayer : State<EnemyChargerScript>
{
	static readonly Charger_AttackPlayer instance = new Charger_AttackPlayer();

	public static Charger_AttackPlayer Instance
	{
		get { return instance; }
	}
	static Charger_AttackPlayer()
	{
	}

	public override void BeforeEnter( EnemyChargerScript e )
	{

	}

	public override void Action( EnemyChargerScript e)
	{
		//Attacking
		//The follow shows how attacking works:
		//1. If the unit is ready to attack
		//2.	Reset attack cooldown
		//3.	Start the animation countdown
		//4.	Set "attacking" to true
		//5. If still waiting for animation delay
		//6.	animationDelay -= deltaTime
		//7.	If done waiting for animation delay
		//8.		Perform all logic to apply an attack
		//9.		Set waiting for animation to false
		if (e.IsWithinAttackRange ()) {
			if(e.NextAttack <= 0)
			{
				e.NextAttack = e.AttackRate;
				e.waitingForAnimationDelay = true;
				e.attackAnimationDelayTimer = EnemyChargerScript.AttackAnimationDelay;
				e.IsAttacking = true;
			}
			else
			{
				e.IsAttacking = false;
			}
		}
		else
		{
			e.ChangeState(Charger_MoveToPlayer.Instance);
		}

		if (e.waitingForAnimationDelay)
		{
			e.attackAnimationDelayTimer -= Time.deltaTime;
			if (e.attackAnimationDelayTimer <= 0)
			{
				// Create sphere attack
				
				Vector3 createPosition = e.transform.position + e.transform.forward;
				GameObject attack = GameObject.Instantiate(e.EnemyAttackSphere) as GameObject;
				attack.transform.position = createPosition;
				attack.GetComponent<SphereCollider>().radius = 1.2f;
				attack.GetComponent<EnemyAttackSphereScript>().SetDamage(e.Damage);
				attack.GetComponent<EnemyAttackSphereScript>().SetForce(e.Force);
				
				e.waitingForAnimationDelay = false;
			}
		}
		e.anim.SetBool("Attacking", e.IsAttacking);
	}
	public override void BeforeExit( EnemyChargerScript e )
	{
		e.anim.SetBool ("Attacking", false);	
	}
}