using UnityEngine;
using System.Collections;
using System;

public class Sniper_MoveToPlayer : State<EnemySniperScript>
{
	static readonly Sniper_MoveToPlayer instance = new Sniper_MoveToPlayer();
	
	public static Sniper_MoveToPlayer Instance
	{
		get { return instance; }
	}
	static Sniper_MoveToPlayer()
	{
	}


	public override void BeforeEnter( EnemySniperScript e )
	{

	}

	public override void Action( EnemySniperScript e)
	{
		if( EnemyBaseScript.player )
		{
			if (e.IsMoving && !e.IsWithinAttackRange()) {
				// Get player location
				Vector3 playerLocation = EnemyBaseScript.player.transform.position;
				
				// Move enemy using navmesh 
				e.GetComponent<NavMeshAgent>().SetDestination(playerLocation);
				
				// Make sure the enemy stays on the ground plane
				//e.transform.SetPositionY(1);
			}
			else if( e.IsWithinAttackRange() )
			{
				e.GetComponent<NavMeshAgent>().Stop();
				e.ChangeState(Sniper_AttackPlayer.Instance);
			}
			e.anim.SetFloat ("Speed", Convert.ToSingle(!e.IsWithinAttackRange()));
		}
		else
		{
			Debug.Log ("[Sniper_MoveToPlayer] WARNING: player entity null does not exist");
		}
	}

	public override void BeforeExit( EnemySniperScript e )
	{

	}
}