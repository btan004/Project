using UnityEngine;
using System.Collections;
using System;

public class AttackPlayer : State<EnemyChaserCloneScript>
{
	static readonly AttackPlayer instance = new AttackPlayer();

	public static AttackPlayer Instance
	{
		get { return instance; }
	}
	static AttackPlayer()
	{
	}

	public override void BeforeEnter( EnemyChaserCloneScript e )
	{

	}

	public override void Action( EnemyChaserCloneScript e)
	{
		if (e.IsWithinAttackRange ()) {
			if(e.NextAttack <= 0)
			{
				e.NextAttack = e.AttackRate;
				e.waitingForAnimationDelay = true;
				e.attackAnimationDelayTimer = EnemyChaserCloneScript.AttackAnimationDelay;
				e.IsAttacking = true;
			}
			else
			{
				e.IsAttacking = false;
			}
		}
		else
		{
			e.ChangeState(MoveToPlayer.Instance);
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
				attack.GetComponent<EnemyAttackSphereScript>().SetDamage(e.Damage);
				attack.GetComponent<EnemyAttackSphereScript>().SetForce(e.Force);
				
				e.waitingForAnimationDelay = false;
			}
		}
		e.anim.SetBool("Attacking", e.IsAttacking);
	}
	public override void BeforeExit( EnemyChaserCloneScript e )
	{
		e.anim.SetBool ("Attacking", false);	
	}
}