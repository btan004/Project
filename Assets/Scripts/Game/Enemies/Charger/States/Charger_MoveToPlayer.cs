using UnityEngine;
using System.Collections;
using System;

public class Charger_MoveToPlayer : State<EnemyChargerScript>
{
	static readonly Charger_MoveToPlayer instance = new Charger_MoveToPlayer();
	
	public static Charger_MoveToPlayer Instance
	{
		get { return instance; }
	}
	static Charger_MoveToPlayer()
	{
	}


	public override void BeforeEnter( EnemyChargerScript e )
	{

	}

	public override void Action( EnemyChargerScript e)
	{
		if( EnemyBaseScript.player )
		{
			if (e.IsMoving && !e.IsWithinAttackRange()) {
				// Get player location
				Vector3 playerLocation = EnemyBaseScript.player.transform.position;

				//Before moving, check if enemy still alive.
				//There's an issue where an enemy still tries to move, but they are
				//dead right before they perform the navmesh move

				if( e.Health > 0 )
				{
					// Move enemy using navmesh 
					e.GetComponent<NavMeshAgent>().SetDestination(playerLocation);
				}
				
				// Make sure the enemy stays on the ground plane
				//e.transform.SetPositionY(1);
			}
			else if( e.IsWithinAttackRange() )
			{
				e.ChangeState(Charger_AttackPlayer.Instance);
			}
			e.anim.SetFloat ("Speed", Convert.ToSingle(!e.IsWithinAttackRange()));
		}
		else
		{
			Debug.Log ("[Charger_MoveToPlayer] WARNING: player entity null does not exist");
		}
	}

	public override void BeforeExit( EnemyChargerScript e )
	{

	}
}