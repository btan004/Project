using UnityEngine;
using System.Collections;
using System;

public class Chaser_AttackPlayer : State<EnemyChaserScript>
{
	static readonly Chaser_AttackPlayer instance = new Chaser_AttackPlayer();

	public static Chaser_AttackPlayer Instance
	{
		get { return instance; }
	}
	static Chaser_AttackPlayer()
	{
	}

	public override void BeforeEnter( EnemyChaserScript e )
	{

	}

	public override void Action( EnemyChaserScript e)
	{
		if (e.IsWithinAttackRange ()) {
			if(e.NextAttack <= 0)
			{
				e.NextAttack = e.AttackRate;
				e.waitingForAnimationDelay = true;
				e.attackAnimationDelayTimer = EnemyChaserScript.AttackAnimationDelay;
				e.IsAttacking = true;
			}
			else
			{
				e.IsAttacking = false;
			}
		}
		else
		{
			e.ChangeState(Chaser_MoveToPlayer.Instance);
		}

		if (e.waitingForAnimationDelay)
		{
			e.attackAnimationDelayTimer -= Time.deltaTime;
			if (e.attackAnimationDelayTimer <= 0)
			{
				// Create sphere attack
				
				Vector3 createPosition = e.transform.position + e.transform.forward;
				GameObject attack = Instantiate(e.EnemyAttackSphere) as GameObject;
				attack.transform.position = createPosition;
				attack.GetComponent<EnemyAttackSphereScript>().SetDamage(e.Damage);
				attack.GetComponent<EnemyAttackSphereScript>().SetForce(e.Force);
				
				e.waitingForAnimationDelay = false;
			}
		}
		e.anim.SetBool("Attacking", e.IsAttacking);
	}
	public override void BeforeExit( EnemyChaserScript e )
	{
		e.anim.SetBool ("Attacking", false);	
	}
}